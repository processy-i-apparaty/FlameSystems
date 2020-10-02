using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
    //todo: render machine break
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
        private static CancellationTokenSource _sourceDrawIntermediate;
        private static readonly object LockObj = new object();
        public static bool HasRender => Display != null;
        public static bool IsRendering { get; set; }

        public static LogDisplayModel Display { get; private set; }

        public static bool RenderStopTotal { get; private set; }

        public static void MainRenderStop(bool total = false)
        {
            RenderStopTotal = total;
            _sourceRender?.Token.ThrowIfCancellationRequested();
            _sourceDrawIntermediate?.Token.ThrowIfCancellationRequested();
            _sourceParallel?.Token.ThrowIfCancellationRequested();
            _renderId++;
            RenderStopTotal = false;
        }

        public static async void Render(RenderPackModel renderPack, RenderActionsModel renderActionsPack,
            bool draftMode = true, bool @continue = false)
        {
            var renderId = ++_renderId;

            _sourceParallel?.Token.ThrowIfCancellationRequested();
            _sourceDrawIntermediate?.Token.ThrowIfCancellationRequested();


            if (_sourceRender != null)
            {
                // _sourceRender.Cancel(true);
                _sourceRender.Token.ThrowIfCancellationRequested();
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
            finally
            {
                Debug.WriteLine($"# end of #{renderId}");
                _sourceRender?.Dispose();
                _sourceRender = null;
            }
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

            //TODO: Density estimation point
            // Density.Estimation(Display, 0, 9, .4, out var di);


            var path =
                $"{folder}\\{name}-{now.Year}{now.Month:00}{now.Day:00}-{(int) now.TimeOfDay.TotalSeconds}-{_iterations}-{(int) _renderTime.TotalSeconds}_{rcMode}.png";
            BitmapSource img;
            switch (colorMode)
            {
                case FlameColorMode.Color:
                    img = Display.GetBitmapForRender(_transformationColors);
                    // img = di.GetBitmapForRender(_transformationColors);
                    break;
                case FlameColorMode.Gradient:
                    img = Display.GetBitmapForRender(_transformationGradientValues, _gradModel);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            ImageHelper.SaveImage(path, img);
        }


        public static void DestroyDisplay()
        {
            if (Display != null) Display = null;
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


        #region ren

        private static RenderSequenceModel PrepareRsm(RenderPackModel renderPack, RenderActionsModel renderActionsPack,
            int renderId, bool draftMode = true, bool continueRender = false)
        {
            var cores = Environment.ProcessorCount;
            var symmetry = renderPack.ViewSettings.Symmetry;
            var totalPointsPerIteration = renderPack.RenderSettings.ShotsPerIteration;
            var imageSize = new Size(renderPack.ViewSettings.ImageWidth, renderPack.ViewSettings.ImageHeight);
            var renderColorMode = renderPack.RenderSettings.RenderColorMode;
            var translationMatrix = Matrix.FromViewSettings(renderPack.ViewSettings);
            var colorCount = renderPack.Transformations.Length;
            var iterators = new IteratorModel[cores];
            for (var i = 0; i < cores; i++) iterators[i] = new IteratorModel(renderPack);
            var sector = Math.PI * 2.0 / symmetry;

            LogDisplayModel display;
            if (continueRender && HasRender)
                display = Display;
            else
                display = new LogDisplayModel((int) imageSize.Width, (int) imageSize.Height,
                    renderPack.Transformations.Length, renderPack.ViewSettings.BackColor);
            display.RenderColorMode = renderColorMode;


            _transformationColors = new Color[colorCount];
            _transformationGradientValues = new double[colorCount];
            for (var i = 0; i < colorCount; i++)
                _transformationColors[i] = renderPack.Transformations[i].Color;
            for (var i = 0; i < colorCount; i++)
                _transformationGradientValues[i] = renderPack.Transformations[i].ColorPosition;

            _gradModel = renderPack.GradModelCopy;

            var rsm = new RenderSequenceModel
            {
                TotalIterations = renderPack.RenderSettings.Iterations,
                Cores = cores,
                Iterators = iterators,
                TotalPointsPerCore = totalPointsPerIteration / cores / symmetry,
                Symmetry = symmetry,
                TranslationArray = translationMatrix.Array,
                CenterPoint = new Point(renderPack.ViewSettings.HalfWidth, renderPack.ViewSettings.HalfHeight),
                ImageSize = imageSize,
                Display = display,
                RenderActionsPack = renderActionsPack,
                RenderId = renderId,
                DraftMode = draftMode,
                RenderPack = renderPack,
                IsDrawingIntermediate = false,
                SectorCos = Math.Cos(sector),
                SectorSin = Math.Sin(sector)
            };
            return rsm;
        }


        private static void RenderTask(RenderPackModel renderPack, RenderActionsModel renderActionsPack, int renderId,
            bool draftMode = true, bool continueRender = false)
        {
            var rsm = PrepareRsm(renderPack, renderActionsPack, renderId, draftMode, continueRender);

            if (!draftMode) renderActionsPack.ActionRenderState?.Invoke(StateRenderStarted, true);
            IsRendering = true;

            if (rsm.RenderPack.RenderSettings.RenderByQuality) RenderSequenceQuality(rsm);
            else RenderSequence(rsm);
        }


        private static bool Iteration(RenderSequenceModel rsm, Stopwatch stopwatch, bool countShots, out int shots)
        {
            shots = 0;
            var shotsParallel = new int[0];

            if (countShots) shotsParallel = new int[rsm.Cores];

            #region render parallel

            var cx = rsm.CenterPoint.X;
            var cy = rsm.CenterPoint.Y;

            IsRendering = true;

            _sourceParallel?.Token.ThrowIfCancellationRequested();

            lock (LockObj)
            {
                _sourceParallel = new CancellationTokenSource();
            }

            var parallelOptions = new ParallelOptions
                {CancellationToken = _sourceParallel.Token, MaxDegreeOfParallelism = rsm.Cores};

            try
            {
                Parallel.For(0, rsm.Cores, parallelOptions, core =>
                {
                    var iterator = rsm.Iterators[core];

                    iterator.Iterate(rsm.TotalPointsPerCore);

                    for (var i = 0; i < rsm.TotalPointsPerCore; i++)
                    {
                        var p = i * 2;
                        var pX = iterator.Points[p];
                        var pY = iterator.Points[p + 1];

                        var colorId = iterator.ColorIds[i];

                        for (var s = 0; s < rsm.Symmetry; s++)
                        {
                            Trigonometry.TranslatePoint(pX, pY, rsm.TranslationArray, cx, cy, out var tpX,
                                out var tpY);
                            if (Trigonometry.InRange(tpX, tpY, rsm.ImageSize))
                            {
                                rsm.Display.Shot((uint) tpX, (uint) tpY, colorId);
                                if (countShots)
                                    shotsParallel[core]++;
                            }

                            if (s < rsm.Symmetry - 1)
                                Trigonometry.RotatePoint(ref pX, ref pY, rsm.SectorCos, rsm.SectorSin);
                        }
                    }
                });
            }
            catch (OperationCanceledException)
            {
                EndRender(rsm.Display, rsm.TotalIterations, stopwatch.Elapsed, rsm.RenderActionsPack,
                    $"render interrupted (OperationCanceledException) {rsm.RenderId}", rsm.DraftMode, rsm.RenderId);
                return false;
            }
            finally
            {
                _sourceParallel?.Dispose();
                _sourceParallel = null;
            }

            _sourceParallel?.Dispose();
            _sourceParallel = null;

            if (!RenderValidity(rsm.RenderId))
            {
                EndRender(rsm.Display, rsm.TotalIterations, stopwatch.Elapsed, rsm.RenderActionsPack,
                    $"render interrupted (RenderValidity) {rsm.RenderId}", rsm.DraftMode, rsm.RenderId);
                return false;
            }

            if (countShots) shots = shotsParallel.Sum();
            // shots = shotSum;

            return true;

            #endregion
        }

        private static bool DrawIntermediate(RenderSequenceModel rsm, bool fast)
        {
            #region draw intermediate image

            rsm.IsDrawingIntermediate = true;

            var tmpDisplay = rsm.Display.Copy();

            _sourceDrawIntermediate?.Token.ThrowIfCancellationRequested();
            _sourceDrawIntermediate = new CancellationTokenSource();

            try
            {
                Task.Run(() =>
                {
                    var sw = new Stopwatch();
                    sw.Start();

                    // Density.Estimation(tmpDisplay, 0d, 9.0d, .8, out var display);

                    BitmapSource img;
                    switch (rsm.RenderPack.ColorMode)
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
                        rsm.RenderActionsPack.ActionDraw.Invoke(img);
                        Debug.WriteLine("DrawIntermediate:  renderActionsPack.ActionDraw.Invoke(img);");
                    }
                    else
                    {
                        Debug.WriteLine("DrawIntermediate: IsRendering false");
                    }

                    rsm.IsDrawingIntermediate = false;
                }, _sourceDrawIntermediate.Token);
            }
            catch (OperationCanceledException)
            {
                // if (!RenderStopTotal)
                // {
                //     Debug.WriteLine($"render interrupted (DrawingIntermediate) #{rsm.RenderId}");
                //     EndRender(rsm.Display, rsm.TotalIterations, stopwatch.Elapsed, rsm.RenderActionsPack,
                //         $"render interrupted (DrawingIntermediate) {rsm.RenderId}", rsm.DraftMode,
                //         rsm.RenderId);
                // }
                // else
                // {
                rsm.Display.CancelParallel();
                // }

                return false;
            }
            finally
            {
                _sourceDrawIntermediate?.Dispose();
                _sourceDrawIntermediate = null;
            }

            return true;

            #endregion
        }


        private static void PrintMessage(RenderSequenceModel rsm, int iteration, Stopwatch stopwatch)
        {
            #region print message

            var elapsedSeconds = stopwatch.Elapsed.TotalSeconds;
            var ips = iteration / elapsedSeconds;
            var left = (double) (rsm.TotalIterations - iteration);
            var timeIpsString = ips.ToString("0.000");
            var timeElapsedString = stopwatch.Elapsed.ToString(@"hh\:mm\:ss");
            var timeLeftString = TimeSpan.FromSeconds(left / ips).ToString(@"hh\:mm\:ss");
            var message =
                $"[RenderMachine ({rsm.RenderId})] iteration: {iteration}; left: {left}; i/s: {timeIpsString}; elapsed: {timeElapsedString}; time left: {timeLeftString}";
            if (rsm.IsDrawingIntermediate) message += " @";
            rsm.RenderActionsPack.ActionMessage.Invoke(message);

            #endregion
        }

        private static void RenderSequence(RenderSequenceModel rsm)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            for (var iteration = 0; iteration < rsm.TotalIterations; iteration++)
            {
                if (!Iteration(rsm, stopwatch, false, out var shots)) return;
                if (!rsm.IsDrawingIntermediate && iteration % rsm.RenderPack.RenderSettings.RenderPerIterations == 0)
                {
                    var fast = iteration == 0;
                    if (!DrawIntermediate(rsm, fast)) return;
                }

                if (rsm.RenderActionsPack.ActionMessage == null) continue;
                if (iteration > 0 && stopwatch.Elapsed.TotalSeconds > 0.1)
                    PrintMessage(rsm, iteration, stopwatch);
            }

            stopwatch.Stop();
            EndRender(rsm.Display, rsm.TotalIterations, stopwatch.Elapsed, rsm.RenderActionsPack,
                $"Done after {stopwatch.Elapsed}.", rsm.DraftMode, rsm.RenderId);
        }

        private static void RenderSequenceQuality(RenderSequenceModel rsm)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var quality = 0.0;
            var totalShots = 0.0;
            var totalPixels = rsm.ImageSize.Width * rsm.ImageSize.Height;

            while (quality < rsm.RenderPack.RenderSettings.Quality)
            {
                if (!Iteration(rsm, stopwatch, true, out var shots)) return;

                totalShots += shots;
                quality = totalShots / totalPixels;

                if (!rsm.IsDrawingIntermediate &&
                    rsm.Iteration % rsm.RenderPack.RenderSettings.RenderPerIterations == 0)
                {
                    // Debug.WriteLine($"QUALITY: {quality}, total shots: {totalShots}, total pixels: {totalPixels}");
                    var fast = rsm.Iteration == 0;
                    if (!DrawIntermediate(rsm, fast)) return;
                }

                if (rsm.RenderActionsPack.ActionMessage == null) continue;

                if (rsm.Iteration > 0 && stopwatch.Elapsed.TotalSeconds > 0.1 && quality > 0.0)
                    PrintMessageQuality(rsm, quality, stopwatch);

                rsm.Iteration++;
            }

            stopwatch.Stop();
            EndRender(rsm.Display, rsm.Iteration, stopwatch.Elapsed, rsm.RenderActionsPack,
                $"Done after {stopwatch.Elapsed}.", rsm.DraftMode, rsm.RenderId);
        }

        private static void PrintMessageQuality(RenderSequenceModel rsm, double currentQuality, Stopwatch stopwatch)
        {
            var elapsedSeconds = stopwatch.Elapsed.TotalSeconds;
            var totalQuality = rsm.RenderPack.RenderSettings.Quality;
            var totalTime = totalQuality * elapsedSeconds / currentQuality;
            var ips = rsm.Iteration / elapsedSeconds;
            var timeIpsString = ips.ToString("0.000");
            var timeElapsedString = stopwatch.Elapsed.ToString(@"hh\:mm\:ss");
            var timeLeftString = TimeSpan.FromSeconds(totalTime-elapsedSeconds).ToString(@"hh\:mm\:ss");
            var message =
                $"[RenderMachine ({rsm.RenderId}) iteration: {rsm.Iteration}; i/s {timeIpsString}; quality: {currentQuality:0.00}/{totalQuality:0.00} elapsed: {timeElapsedString}; time left: {timeLeftString}";
            if (rsm.IsDrawingIntermediate) message += " @";
            rsm.RenderActionsPack.ActionMessage.Invoke(message);
        }

        #endregion


        #region backup

        // private static void RenderSequence(RenderSequenceModel rsm)
        // {
        //     var cx = rsm.CenterPoint.X;
        //     var cy = rsm.CenterPoint.Y;
        //
        //     var stopwatch = new Stopwatch();
        //     stopwatch.Start();
        //
        //
        //     for (var iteration = 0; iteration < rsm.TotalIterations; iteration++)
        //     {
        //         #region render parallel
        //
        //         IsRendering = true;
        //
        //         // _sourceParallel?.Cancel(true);
        //         // _sourceParallel?.Dispose();
        //         _sourceParallel?.Token.ThrowIfCancellationRequested();
        //
        //         lock (LockObj)
        //         {
        //             _sourceParallel = new CancellationTokenSource();
        //         }
        //
        //         var parallelOptions = new ParallelOptions
        //         { CancellationToken = _sourceParallel.Token, MaxDegreeOfParallelism = rsm.Cores };
        //
        //         try
        //         {
        //             Parallel.For(0, rsm.Cores, parallelOptions, core =>
        //             {
        //                 var iterator = rsm.Iterators[core];
        //
        //                 iterator.Iterate(rsm.TotalPointsPerCore);
        //
        //                 for (var i = 0; i < rsm.TotalPointsPerCore; i++)
        //                 {
        //                     var p = i * 2;
        //                     var pX = iterator.Points[p];
        //                     var pY = iterator.Points[p + 1];
        //
        //                     var colorId = iterator.ColorIds[i];
        //
        //                     for (var s = 0; s < rsm.Symmetry; s++)
        //                     {
        //                         Trigonometry.TranslatePoint(pX, pY, rsm.TranslationArray, cx, cy, out var tpX,
        //                             out var tpY);
        //                         if (Trigonometry.InRange(tpX, tpY, rsm.ImageSize))
        //                             rsm.Display.Shot((uint)tpX, (uint)tpY, colorId);
        //                         if (s < rsm.Symmetry - 1)
        //                             Trigonometry.RotatePoint(ref pX, ref pY, rsm.SectorCos, rsm.SectorSin);
        //                     }
        //                 }
        //             });
        //         }
        //         catch (OperationCanceledException)
        //         {
        //             EndRender(rsm.Display, rsm.TotalIterations, stopwatch.Elapsed, rsm.RenderActionsPack,
        //                 $"render interrupted (OperationCanceledException) {rsm.RenderId}", rsm.DraftMode, rsm.RenderId);
        //             return;
        //         }
        //         finally
        //         {
        //             _sourceParallel?.Dispose();
        //             _sourceParallel = null;
        //         }
        //
        //         _sourceParallel?.Dispose();
        //         _sourceParallel = null;
        //
        //         if (!RenderValidity(rsm.RenderId))
        //         {
        //             EndRender(rsm.Display, rsm.TotalIterations, stopwatch.Elapsed, rsm.RenderActionsPack,
        //                 $"render interrupted (RenderValidity) {rsm.RenderId}", rsm.DraftMode, rsm.RenderId);
        //             return;
        //         }
        //
        //         #endregion
        //
        //         #region draw intermediate image
        //
        //         if (!rsm.IsDrawingIntermediate && iteration % rsm.RenderPack.RenderSettings.RenderPerIterations == 0)
        //         {
        //             rsm.IsDrawingIntermediate = true;
        //
        //             var tmpDisplay = rsm.Display.Copy();
        //
        //             _sourceDrawIntermediate?.Token.ThrowIfCancellationRequested();
        //             // _sourceDraw?.Cancel(true);
        //             // _sourceDraw?.Dispose();
        //             _sourceDrawIntermediate = new CancellationTokenSource();
        //
        //             try
        //             {
        //                 var iterationLocal = iteration;
        //                 Task.Run(() =>
        //                 {
        //                     var fast = iterationLocal == 0;
        //                     // Debug.WriteLine(
        //                     //     $"\t@ {typeof(RenderMachine).Name}.{MethodBase.GetCurrentMethod().Name} getting img");
        //                     var sw = new Stopwatch();
        //                     sw.Start();
        //
        //                     // Density.Estimation(tmpDisplay, 0d, 9.0d, .8, out var display);
        //
        //                     BitmapSource img;
        //                     switch (rsm.RenderPack.ColorMode)
        //                     {
        //                         case FlameColorMode.Color:
        //                             img = tmpDisplay.GetBitmapForRender(_transformationColors, fast);
        //                             break;
        //                         case FlameColorMode.Gradient:
        //                             img = tmpDisplay.GetBitmapForRender(_transformationGradientValues, _gradModel,
        //                                 fast);
        //                             break;
        //                         default:
        //                             throw new ArgumentOutOfRangeException();
        //                     }
        //
        //                     if (IsRendering)
        //                     {
        //                         sw.Stop();
        //                         rsm.RenderActionsPack.ActionDraw.Invoke(img);
        //                         Debug.WriteLine("DrawIntermediate:  renderActionsPack.ActionDraw.Invoke(img);");
        //                     }
        //                     else
        //                     {
        //                         Debug.WriteLine("DrawIntermediate: IsRendering false");
        //                     }
        //
        //                     rsm.IsDrawingIntermediate = false;
        //                 }, _sourceDrawIntermediate.Token);
        //             }
        //             catch (OperationCanceledException)
        //             {
        //                 if (!RenderStopTotal)
        //                 {
        //                     Debug.WriteLine($"render interrupted (DrawingIntermediate) #{rsm.RenderId}");
        //                     EndRender(rsm.Display, rsm.TotalIterations, stopwatch.Elapsed, rsm.RenderActionsPack,
        //                         $"render interrupted (DrawingIntermediate) {rsm.RenderId}", rsm.DraftMode,
        //                         rsm.RenderId);
        //                 }
        //                 else
        //                 {
        //                     rsm.Display.CancelParallel();
        //                 }
        //
        //                 return;
        //             }
        //             finally
        //             {
        //                 _sourceDrawIntermediate?.Dispose();
        //                 _sourceDrawIntermediate = null;
        //             }
        //         }
        //
        //         #endregion
        //
        //         #region print message
        //
        //         if (rsm.RenderActionsPack.ActionMessage == null) continue;
        //         var elapsedSeconds = stopwatch.Elapsed.TotalSeconds;
        //         if (iteration <= 0 || !(elapsedSeconds > 0.1)) continue;
        //
        //         var ips = iteration / elapsedSeconds;
        //         var left = (double)(rsm.TotalIterations - iteration);
        //         var timeIpsString = ips.ToString("0.000");
        //         var timeElapsedString = stopwatch.Elapsed.ToString(@"hh\:mm\:ss");
        //         var timeLeftString = TimeSpan.FromSeconds(left / ips).ToString(@"hh\:mm\:ss");
        //         var message =
        //             $"[RenderMachine ({rsm.RenderId})] iteration: {iteration}; left: {left}; i/s: {timeIpsString}; elapsed: {timeElapsedString}; time left: {timeLeftString}";
        //         if (rsm.IsDrawingIntermediate) message += " @";
        //         rsm.RenderActionsPack.ActionMessage.Invoke(message);
        //
        //         #endregion
        //     }
        //
        //     stopwatch.Stop();
        //
        //
        //     #region draw final image
        //
        //     stopwatch.Stop();
        //     EndRender(rsm.Display, rsm.TotalIterations, stopwatch.Elapsed, rsm.RenderActionsPack,
        //         $"Done after {stopwatch.Elapsed}.", rsm.DraftMode, rsm.RenderId);
        //
        //     #endregion
        // }

        #endregion
    }
}