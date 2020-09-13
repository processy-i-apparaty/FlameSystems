using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using FlameBase.Enums;
using FlameBase.FlameMath;
using FlameBase.Helpers;
using FlameBase.Models;
using FlameBase.RenderMachine.Models;
using Matrix = FlameBase.FlameMath.Matrix;

namespace FlameBase.RenderMachine
{
    public static class RenderMachine
    {
        public const string StateRenderEnded = "RENDER_ENDED";
        public const string StateRenderStarted = "RENDER_STARTED";
        private static int _renderId;
        private static int _iterations;
        private static TimeSpan _renderTime;
        private static Color[] _transformationColors;
        private static double[] _transformationGradientValues;
        private static GradientModel _gradModel;

        private static CancellationTokenSource _sourceRender;
        private static CancellationTokenSource _sourceParallel;
        private static CancellationTokenSource _sourceDraw;
        private static readonly object LockObj=new object();
        public static bool HasRender => Display != null;
        public static bool IsRendering { get; set; }

        public static LogDisplayModel Display { get; private set; }

        public static void RenderStop()
        {
            _sourceRender?.Cancel(true);
            _sourceParallel?.Cancel(true);
            _sourceDraw?.Cancel(true);
            _renderId++;
            // Debug.WriteLine($"\n#######\t[RenderStop] [{_sourceRender}] [{_sourceParallel}] [{_sourceDraw}]");
        }

        public static async void Render(RenderPackModel renderPack, RenderActionsModel renderActionsPack,
            bool draftMode = true, bool @continue = false)
        {
            var renderId = ++_renderId;
            _sourceParallel?.Cancel(true);
            _sourceDraw?.Cancel(true);

            if (_sourceRender != null)
            {
                _sourceRender.Cancel(true);
                await WaitForRenderEnd(TimeSpan.FromSeconds(10));
                _sourceRender = new CancellationTokenSource();
            }
            else
            {
                _sourceRender = new CancellationTokenSource();
            }

            if (renderId != _renderId) return;
            if (draftMode) DestroyDisplay();

            try
            {
                await Task.Run(() => RenderTask(renderPack, renderActionsPack, renderId, draftMode, @continue),
                    _sourceRender.Token);
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine($"# cancel of {renderId}");
            }

            Debug.WriteLine($"# end of #{renderId}");
            _sourceRender?.Dispose();
            _sourceRender = null;
        }

        private static void InitVariations(IEnumerable<VariationModel> variations)
        {
            foreach (var variation in variations) variation.Init();
        }

        public static void SaveImage(string folder, string name, FlameColorMode colorMode)
        {
            if (Display == null) return;
            var now = DateTime.Now;

            var rcMode = "";
            switch (Display.RenderColorMode)
            {
                case RenderColorModeModel.RenderColorMode.Hsb:
                    rcMode = "hsb";
                    break;
                case RenderColorModeModel.RenderColorMode.Lab:
                    rcMode = "lab";
                    break;
                case RenderColorModeModel.RenderColorMode.LogGamma:
                    rcMode = "log";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var path =
                $"{folder}\\{name}-{now.Year}{now.Month:00}{now.Day:00}-{(int) now.TimeOfDay.TotalSeconds}-{_iterations}-{(int) _renderTime.TotalSeconds}_{rcMode}.png";
            BitmapSource img;
            switch (colorMode)
            {
                case FlameColorMode.Color:
                    img = Display.GetBitmapForRender(_transformationColors);
                    break;
                case FlameColorMode.Gradient:
                    img = Display.GetBitmapForRender(_transformationGradientValues, _gradModel);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            SaveImage(path, img);
        }

        private static void SaveImage(string filename, BitmapSource bitmapSource)
        {
            if (string.IsNullOrEmpty(filename)) return;
            var imageType = Path.GetExtension(filename).Trim('.');
            BitmapEncoder encoder;
            switch (imageType.ToLower())
            {
                case "png":
                    encoder = new PngBitmapEncoder();
                    break;
                case "jpg":
                case "jpeg":
                    encoder = new JpegBitmapEncoder();
                    break;
                case "bmp":
                    encoder = new BmpBitmapEncoder();
                    break;
                case "tiff":
                    encoder = new TiffBitmapEncoder();
                    break;
                case "gif":
                    encoder = new GifBitmapEncoder();
                    break;
                case "wmb":
                    encoder = new WmpBitmapEncoder();
                    break;
                default:
                    return;
            }

            using (var fileStream = new FileStream(filename, FileMode.Create))
            {
                encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                encoder.Save(fileStream);
            }
        }

        public static void DestroyDisplay()
        {
            if (Display != null) Display = null;
        }


        private static void RenderTask(RenderPackModel renderPack, RenderActionsModel renderActionsPack, int renderId,
            bool draftMode = true, bool @continue = false)
        {
            #region prepare variables

            var isDrawingIntermediate = false;

            var cores = Environment.ProcessorCount;
            var symmetry = renderPack.ViewSettings.Symmetry;

            var totalPointsPerIteration = renderPack.RenderSettings.ShotsPerIteration;
            var totalIterations = renderPack.RenderSettings.Iterations;
            var totalPointsPerCore = totalPointsPerIteration / cores / symmetry;
            var renderColorMode = renderPack.RenderSettings.RenderColorMode;

            var imageSize = new Size(renderPack.ViewSettings.ImageWidth, renderPack.ViewSettings.ImageHeight);

            var colorCount = renderPack.Transformations.Length;


            LogDisplayModel display;
            if (@continue && HasRender)
                display = Display;
            else
                display = new LogDisplayModel((int) imageSize.Width, (int) imageSize.Height, colorCount,
                    renderPack.ViewSettings.BackColor);

            display.RenderColorMode = renderColorMode;

            var translationMatrix = Matrix.FromViewSettings(renderPack.ViewSettings);
            var translationArray = translationMatrix.Array;

            var iterators = new IteratorModel[cores];
            for (var i = 0; i < cores; i++) iterators[i] = new IteratorModel(renderPack);

            var sector = Math.PI * 2.0 / symmetry;
            var sectorCos = Math.Cos(sector);
            var sectorSin = Math.Sin(sector);
            var centerVector = new Vector(renderPack.ViewSettings.HalfWidth, renderPack.ViewSettings.HalfHeight);
            var cx = centerVector.X;
            var cy = centerVector.Y;

            _transformationColors = new Color[colorCount];
            _transformationGradientValues = new double[colorCount];
            for (var i = 0; i < colorCount; i++)
                _transformationColors[i] = renderPack.Transformations[i].Color;
            for (var i = 0; i < colorCount; i++)
                _transformationGradientValues[i] = renderPack.Transformations[i].ColorPosition;

            _gradModel = renderPack.GradModelCopy;

            IsRendering = true;
            if (!draftMode) renderActionsPack.ActionRenderState?.Invoke(StateRenderStarted, true);

            #endregion

            #region render sequence

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            for (var iteration = 0; iteration < totalIterations; iteration++)
            {
                IsRendering = true;

                _sourceParallel?.Cancel(true);
                _sourceParallel?.Dispose();
                lock (LockObj)
                {
                    _sourceParallel = new CancellationTokenSource();
                }

                var parallelOptions = new ParallelOptions
                    {CancellationToken = _sourceParallel.Token, MaxDegreeOfParallelism = cores};

                try
                {
                    Parallel.For(0, cores, parallelOptions, core =>
                    {
                        var iterator = iterators[core];

                        iterator.Iterate(totalPointsPerCore);

                        for (var i = 0; i < totalPointsPerCore; i++)
                        {
                            var p = i * 2;
                            var pX = iterator.Points[p];
                            var pY = iterator.Points[p + 1];

                            var colorId = iterator.ColorIds[i];

                            for (var s = 0; s < symmetry; s++)
                            {
                                Trigonometry.TranslatePoint(pX, pY, translationArray, cx, cy, out var tpX, out var tpY);
                                if (Trigonometry.InRange(tpX, tpY, imageSize))
                                    display.Shot((uint) tpX, (uint) tpY, colorId);
                                if (s < symmetry - 1)
                                    Trigonometry.RotatePoint(ref pX, ref pY, sectorCos, sectorSin);
                            }
                        }
                    });
                }
                catch (OperationCanceledException)
                {
                    EndRender(display, totalIterations, stopwatch.Elapsed, renderActionsPack,
                        $"render interrupted (OperationCanceledException) {renderId}", draftMode, renderId);
                    return;
                }
                finally
                {
                    _sourceParallel?.Dispose();
                    _sourceParallel = null;
                }

                _sourceParallel?.Dispose();
                _sourceParallel = null;

                if (!RenderValidity(renderId))
                {
                    EndRender(display, totalIterations, stopwatch.Elapsed, renderActionsPack,
                        $"render interrupted (RenderValidity) {renderId}", draftMode, renderId);
                    return;
                }

                #region draw intermediate image

                if (!isDrawingIntermediate && iteration % renderPack.RenderSettings.RenderPerIterations == 0)
                {
                    isDrawingIntermediate = true;
                    
                    var tmpDisplay = display.Copy();
                    
                    _sourceDraw?.Cancel(true);
                    _sourceDraw?.Dispose();
                    _sourceDraw = new CancellationTokenSource();

                    try
                    {
                        var iterationLocal = iteration;
                        Task.Run(() =>
                        {
                            var fast = iterationLocal == 0;
                            // Debug.WriteLine(
                            //     $"\t@ {typeof(RenderMachine).Name}.{MethodBase.GetCurrentMethod().Name} getting img");
                            var sw = new Stopwatch();
                            sw.Start();

                            // Density.Estimation(tmpDisplay, 0d, 9.0d, .8, out var display);

                            BitmapSource img;
                            switch (renderPack.ColorMode)
                            {
                                case FlameColorMode.Color:
                                    img = tmpDisplay.GetBitmapForRender(_transformationColors, fast);
                                    break;
                                case FlameColorMode.Gradient:
                                    img = tmpDisplay.GetBitmapForRender(_transformationGradientValues, _gradModel,
                                        fast);
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }

                            if (IsRendering)
                            {
                                sw.Stop();
                                renderActionsPack.ActionDraw.Invoke(img);
                                Debug.WriteLine("DrawIntermediate:  renderActionsPack.ActionDraw.Invoke(img);");
                            }
                            else
                            {
                                Debug.WriteLine("DrawIntermediate: IsRendering false");
                            }

                            isDrawingIntermediate = false;
                        }, _sourceDraw.Token);
                    }
                    catch (OperationCanceledException)
                    {
                        Debug.WriteLine($"render interrupted (DrawingIntermediate) #{renderId}");
                        EndRender(display, totalIterations, stopwatch.Elapsed, renderActionsPack,
                            $"render interrupted (DrawingIntermediate) {renderId}", draftMode, renderId);
                        return;
                    }
                    finally
                    {
                        _sourceDraw?.Dispose();
                        _sourceDraw = null;
                    }
                }

                #endregion


                // var iterationReportString = "";
                // var elapsedSeconds = stopwatch.Elapsed.TotalSeconds;
                // if (iteration > 0 && elapsedSeconds > 1e-2)
                // {
                //     var iterationsPerSecond = iteration / elapsedSeconds;
                //     var iterationsLeft = totalIterations - iteration;
                //     var secondsLeft = iterationsLeft / iterationsPerSecond;
                //     var timeLeft = TimeSpan.FromSeconds(secondsLeft);
                //     var timeString = TimeStringModel.TimeSpanToSting(timeLeft);
                //     iterationReportString =
                //         $"left: {iterationsLeft:00000000}, I/s {iterationsPerSecond:0.00}, {timeString} left";
                // }

                // StaticAction.Invoke("REPORT", $"ITERATION #{iteration:00000000} {iterationReportString}");
                // if (token.IsCancellationRequested) throw new TaskCanceledException();

                #region print message

                if (renderActionsPack.ActionMessage == null) continue;
                var elapsedSeconds = stopwatch.Elapsed.TotalSeconds;
                if (iteration <= 0 || !(elapsedSeconds > 0.1)) continue;

                var ips = iteration / elapsedSeconds;
                var left = (double) (totalIterations - iteration);
                var timeIpsString = ips.ToString("0.000");
                var timeElapsedString = stopwatch.Elapsed.ToString(@"hh\:mm\:ss");
                var timeLeftString = TimeSpan.FromSeconds(left / ips).ToString(@"hh\:mm\:ss");
                var message =
                    $"[RenderMachine ({renderId})] iteration: {iteration}; left: {left}; i/s: {timeIpsString}; elapsed: {timeElapsedString}; time left: {timeLeftString}";
                if (isDrawingIntermediate) message += " @";
                renderActionsPack.ActionMessage.Invoke(message);

                #endregion
            }

            stopwatch.Stop();

            #endregion

            // #region report
            // var renderParameters =
            //     $"{{{imageSize.Width:0000}x{imageSize.Height:0000}}}, shift: {{{flameModel.ViewShiftX:0.0000}}}, {{{flameModel.ViewShiftY:0.0000}}}, zoom: {flameModel.ViewZoom:0.0000}";
            // var time = $"Time: {stopwatch.ElapsedMilliseconds * .001:0.000}sec";
            // var filename = $"{Path.GetFileNameWithoutExtension(_flameModelPath)}";
            // var now = DateTime.Now;
            // var _pngName =
            // $"{filename}-{now.DayOfYear}-{(int) now.TimeOfDay.TotalSeconds}-{(int) stopwatch.Elapsed.TotalSeconds}.png";
            // return gradientMode
            //     ? display.GetBitmapForRender(transformationGradientValues, gradModel)
            //     : display.GetBitmapForRender(flameModel.FunctionColors.ToArray());
            // #endregion

            #region draw final image

            stopwatch.Stop();
            EndRender(display, totalIterations, stopwatch.Elapsed, renderActionsPack,
                $"Done after {stopwatch.Elapsed}.", draftMode, renderId);

            #endregion
        }


        private static Task WaitForRenderEnd(TimeSpan wait)
        {
            return Task.Run(() =>
            {
                var sw = new Stopwatch();
                sw.Start();
                while (IsRendering && sw.Elapsed < wait) Thread.Sleep(5);
            });
        }

        private static bool RenderValidity(int id)
        {
            if (_renderId == id) return true;
            IsRendering = false;
            return false;
        }

        private static void EndRender(LogDisplayModel logDisplay, int maxIterations, TimeSpan elapsed,
            RenderActionsModel renderActions,
            string message, bool draftMode, int renderId)
        {
            Debug.WriteLine($"EndRender [{renderId}]");
            IsRendering = false;
            if (draftMode)
            {
                renderActions.ActionMessage?.Invoke($"draft render ends in {elapsed:hh\\:mm\\:ss}");
                return;
            }

            Display = logDisplay;
            _iterations = maxIterations;
            _renderTime = elapsed;
            renderActions.ActionRenderState?.Invoke(StateRenderEnded, true);
            renderActions.ActionMessage?.Invoke(message);
        }

        public static void LoadDisplay(uint[,,] display, Color backColor)
        {
            Display = new LogDisplayModel(display, backColor);
        }

        public static BitmapSource GetImage(Color[] colors, double[] gradientValues, GradientModel gradientModel)
        {
            return gradientModel == null
                ? Display.GetBitmapForRender(colors)
                : Display.GetBitmapForRender(gradientValues, gradientModel);
        }
    }
}