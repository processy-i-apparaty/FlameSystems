using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using FlameBase.Enums;
using FlameBase.Models;
using FlameSystems.Infrastructure;
using FlameSystems.Infrastructure.ActionFire;
using FlameSystems.Infrastructure.ValueBind;

namespace FlameSystems.Controls.ViewModels
{
    internal class GradientPickerViewModel : Notifier
    {
        private const double EllipseWidth = 18.0;
        private readonly Dictionary<int, Point> _ellipseCoordinates = new Dictionary<int, Point>();
        private readonly Dictionary<int, Ellipse> _ellipses = new Dictionary<int, Ellipse>();
        private readonly double _gap;
        private readonly double _halfWidth;
        private readonly Stopwatch _stopwatch = new Stopwatch();

        private Canvas _canvasMiddle;
        private Canvas _canvasMiddleScanner;
        private int _clickOn = -1;
        private int _colorMode;
        private int _dragId = -1;
        private double _newPosition;
        private int _selectedColorId;

        #region actions

        private void ActionCallback(Color color)
        {
            if (_selectedColorId >= 0)
            {
                var e = _ellipses[_selectedColorId];
                e.Fill = new SolidColorBrush(color);
                switch (_colorMode)
                {
                    case 2:
                        GradientModel.Add(_selectedColorId, color, _newPosition);
                        break;
                    case 1:
                        GradientModel.ChangeColor(_selectedColorId, color);
                        break;
                }
            }
            else
            {
                switch (_selectedColorId)
                {
                    case -11:
                        ColorLeft = new SolidColorBrush(color);
                        break;
                    case -22:
                        ColorRight = new SolidColorBrush(color);
                        break;
                }
            }

            RedrawGradient();
        }

        #endregion

        #region public

        public GradientModel GradientModel { get; }
        public GradientMode GradientMode { get; } = GradientMode.Edit;
        public int Id { get; }

        public double GetColorPosition()
        {
            return _ellipseCoordinates.ElementAt(0).Value.X / _canvasMiddleScanner.ActualWidth;
        }

        #endregion

        #region constructors

        public GradientPickerViewModel()
        {
            Id = GiveIdModel.Get;
            _stopwatch.Start();
            _halfWidth = EllipseWidth * .5;
            _gap = (20.0 - EllipseWidth) * .5;
            GradientModel = new GradientModel(Colors.Gray, Colors.Gray);

            InitCommands();

            ColorLeft = new SolidColorBrush(Colors.CadetBlue);
            ColorRight = new SolidColorBrush(Colors.Blue);
            RedrawGradient();
        }

        public GradientPickerViewModel(GradientModel gradientModel, double position)
        {
            _stopwatch.Start();
            _halfWidth = EllipseWidth * .5;
            _gap = (20.0 - EllipseWidth) * .5;
            GradientModel = gradientModel;
            GradientMode = GradientMode.Select;
            InitCommands();
            InitGradientSelect(position);
            RedrawGradient();
        }

        public GradientPickerViewModel(GradientModel gradientModel)
        {
            _stopwatch.Start();
            _halfWidth = EllipseWidth * .5;
            _gap = (20.0 - EllipseWidth) * .5;
            GradientModel = gradientModel;
            GradientMode = GradientMode.Edit;
            InitCommands();

            InitGradientEdit();
            ThicknessSquares = new Thickness(1);
            RedrawGradient();
        }

        #endregion

        #region private

        private async void InitGradientEdit()
        {
            await Task.Run(() =>
            {
                while (_canvasMiddle == null || _canvasMiddleScanner == null) Thread.Sleep(10);
            });
            for (var i = 0; i < GradientModel.Length; i++)
                CreateEllipse(GradientModel.Ids[i], GradientModel.Colors[i], GradientModel.Values[i]);
        }

        private async void InitGradientSelect(double position)
        {
            await Task.Run(() =>
            {
                while (_canvasMiddle == null || _canvasMiddleScanner == null) Thread.Sleep(10);
            });
            CreateEllipse(GiveIdModel.Get, GradientModel.GetFromPosition(position), position);
        }

        private void InitCommands()
        {
            CommandCanvasLoaded = new RelayCommand(CanvasLoadedHandler);
            CommandCanvasLeftMd = new RelayCommand(CanvasLeftMd);
            CommandCanvasMidMd = new RelayCommand(CanvasMidMd);
            CommandCanvasMidMm = new RelayCommand(CanvasMidMm);
            CommandCanvasRightMd = new RelayCommand(CanvasRightMd);
            CommandCanvasMidMrd = new RelayCommand(CanvasMidMrd);
            CommandCanvasMidMu = new RelayCommand(CanvasMidMu);
            CommandSizeChanged = new RelayCommand(SizeChanged);
            Command = new RelayCommand(ButtonsHandler);
            ActionFire.AddOrReplace("GRADIENT_PICKER_VIEWMODEL-CALLBACK", new Action<Color>(ActionCallback), GetType());
        }

        private static void ButtonsHandler(object obj)
        {
            var button = (string) obj;
            switch (button)
            {
                case "ok":
                    ActionFire.Invoke("CREATE_FLAME_VIEWMODEL-TRANSFORM_PICK_GRADIENT_CALLBACK", true);
                    break;
                case "cancel":
                    ActionFire.Invoke("CREATE_FLAME_VIEWMODEL-TRANSFORM_PICK_GRADIENT_CALLBACK", false);
                    break;
            }
        }

        #endregion

        #region properties

        public SolidColorBrush ColorLeft
        {
            get
            {
                switch (GradientMode)
                {
                    case GradientMode.Edit:
                        return new SolidColorBrush(GradientModel.StartColor);
                    case GradientMode.Select:
                        return null;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            set
            {
                switch (GradientMode)
                {
                    case GradientMode.Edit:
                        GradientModel.StartColor = value.Color;
                        break;
                    case GradientMode.Select:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                Notify();
            }
        }

        public SolidColorBrush ColorRight
        {
            get
            {
                switch (GradientMode)
                {
                    case GradientMode.Edit:
                        return new SolidColorBrush(GradientModel.EndColor);
                    case GradientMode.Select:
                        return null;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            set
            {
                switch (GradientMode)
                {
                    case GradientMode.Edit:
                        GradientModel.EndColor = value.Color;
                        break;
                    case GradientMode.Select:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                Notify();
            }
        }

        [ValueBind]
        public GradientBrush Gradient
        {
            get => Get();
            set => Set(value);
        }

        [ValueBind]
        public Thickness ThicknessSquares
        {
            get => Get();
            set => Set(value);
        }


        public ICommand CommandCanvasLoaded { get; private set; }
        public ICommand CommandCanvasLeftMd { get; private set; }
        public ICommand CommandCanvasRightMd { get; private set; }
        public ICommand CommandCanvasMidMd { get; private set; }
        public ICommand CommandCanvasMidMrd { get; private set; }
        public ICommand CommandCanvasMidMu { get; private set; }
        public ICommand CommandCanvasMidMm { get; private set; }
        public ICommand CommandSizeChanged { get; private set; }

        public ICommand Command { get; set; }

        #endregion

        #region ui interactions

        #region loaded

        private void CanvasLoadedHandler(object obj)
        {
            var canvas = (Canvas) obj;
            switch (canvas.Name)
            {
                case "ColorMiddle":
                    _canvasMiddleScanner = canvas;
                    break;
                case "CanvasMiddle":
                    _canvasMiddle = canvas;
                    break;
            }
        }

        #endregion

        #region size changed

        private void SizeChanged(object obj)
        {
        }

        #endregion

        #region mouse mid

        private void CanvasMidMu(object obj)
        {
            _dragId = -1;
            Mouse.Capture(null);
            RedrawGradient();
        }

        private void CanvasMidMrd(object obj)
        {
            if (GradientMode == GradientMode.Edit)
                if (GetEllipse(Mouse.GetPosition(_canvasMiddleScanner), out var id))
                    RemoveEllipse(id);
        }

        private void CanvasMidMm(object obj)
        {
            if (_dragId == -1) return;
            var e = _ellipses[_dragId];
            var p = Mouse.GetPosition(_canvasMiddleScanner);

            if (p.X < 0) p.X = 0;
            if (p.X > _canvasMiddleScanner.ActualWidth) p.X = _canvasMiddleScanner.ActualWidth;
            _ellipseCoordinates[_dragId] = p;
            SetPos(e, p);
            var gradPos = p.X / _canvasMiddleScanner.ActualWidth;
            if (gradPos >= 1.0) gradPos = 1.0 - 1E-10;
            if (gradPos <= 0) gradPos = 1E-10;
            if (GradientMode == GradientMode.Edit)
            {
                GradientModel.Move(_dragId, gradPos);
                RedrawGradient();
            }

            if (GradientMode == GradientMode.Select)
                e.Fill = new SolidColorBrush(GradientModel.GetFromPosition(gradPos));
        }

        private void CanvasMidMd(object obj)
        {
            var pos = Mouse.GetPosition(_canvasMiddleScanner);

            if (_stopwatch.ElapsedMilliseconds < 250 && _clickOn == 1)
            {
                if (GradientMode == GradientMode.Edit)
                {
                    //double click
                    if (GetEllipse(pos, out var id))
                        //set color
                        SelectColor(id, 1);
                    else
                        CreateEllipse(pos);
                }
            }
            else
            {
                if (GradientMode == GradientMode.Edit)
                {
                    if (GetEllipse(pos, out var id))
                    {
                        _dragId = id;
                        ZIndex(id);
                        Mouse.Capture(_canvasMiddleScanner);
                    }
                    else
                    {
                        _dragId = -1;
                    }
                }

                if (GradientMode == GradientMode.Select)
                    if (GetEllipse(pos, out var id))
                    {
                        _dragId = id;
                        Mouse.Capture(_canvasMiddleScanner);
                    }
            }

            _stopwatch.Restart();
            _clickOn = 1;
        }

        #endregion

        #region mouse down

        private void CanvasLeftMd(object obj)
        {
            //double click check
            if (_stopwatch.ElapsedMilliseconds < 250 && _clickOn == 0) SelectColor(-11);
            _stopwatch.Restart();
            _clickOn = 0;
        }

        private void CanvasRightMd(object obj)
        {
            //double click check
            if (_stopwatch.ElapsedMilliseconds < 250 && _clickOn == 2) SelectColor(-22);
            _stopwatch.Restart();
            _clickOn = 2;
        }

        #endregion

        #region methods

        private void RedrawGradient()
        {
            Gradient = new LinearGradientBrush(GradientModel.GetGradientStopCollection(), 0.0);
        }

        private void ZIndex(int id)
        {
            var max = (from pair in _ellipses where pair.Key != id select Panel.GetZIndex(pair.Value))
                .Concat(new[] {int.MinValue}).Max();
            Panel.SetZIndex(_ellipses[id], max + 1);
        }

        private void CreateEllipse(Point pos)
        {
            var id = GiveIdModel.Get;
            var ellipse = new Ellipse
            {
                Width = EllipseWidth,
                Height = EllipseWidth,
                Fill = new SolidColorBrush(Colors.Yellow),
                Stroke = new SolidColorBrush(Colors.DimGray)
            };
            SetPos(ellipse, pos);
            _canvasMiddle.Children.Add(ellipse);
            _ellipses.Add(id, ellipse);
            _ellipseCoordinates.Add(id, pos);
            _newPosition = pos.X / _canvasMiddleScanner.ActualWidth;
            SelectColor(id, 2);
        }

        private void CreateEllipse(int id, Color color, double value)
        {
            var pos = new Point(value * _canvasMiddleScanner.ActualWidth, 0);
            var ellipse = new Ellipse
            {
                Width = EllipseWidth,
                Height = EllipseWidth,
                Fill = new SolidColorBrush(color),
                Stroke = new SolidColorBrush(Colors.DimGray)
            };
            SetPos(ellipse, pos);
            _canvasMiddle.Children.Add(ellipse);
            _ellipses.Add(id, ellipse);
            _ellipseCoordinates.Add(id, pos);
//            _gradModel.Add(id, color, value);
        }

        private void SetPos(UIElement ellipse, Point pos)
        {
            Canvas.SetLeft(ellipse, pos.X - _halfWidth);
            Canvas.SetTop(ellipse, _gap);
        }

        private bool GetEllipse(Point pos, out int i)
        {
            var min = double.MaxValue;
            var minId = -1;
            foreach (var pair in _ellipseCoordinates)
            {
                var distance = Math.Abs(pos.X - pair.Value.X); // Distance(pos, pair.Value);
                if (distance >= _halfWidth) continue;
                if (distance >= min) continue;
                minId = pair.Key;
                min = distance;
            }

            if (minId >= 0)
            {
                i = minId;
                return true;
            }

            i = -1;
            return false;
        }

        private void RemoveEllipse(int id)
        {
            var e = _ellipses[id];
            _canvasMiddle.Children.Remove(e);
            _ellipses.Remove(id);
            _ellipseCoordinates.Remove(id);
            GradientModel.Delete(id);
            RedrawGradient();
        }

        private void SelectColor(int id, int mode = 0)
        {
            _colorMode = mode;
            _selectedColorId = id;

            Color color;
            if (id == -22)
            {
                color = ColorRight.Color;
            }
            else
            {
                if (_ellipses.ContainsKey(id))
                {
                    var value = _ellipseCoordinates[id].X / _canvasMiddleScanner.ActualWidth;
                    color = GradientModel.GetFromPosition(value);
                }
                else
                {
                    var value = _newPosition / _canvasMiddleScanner.ActualWidth;
                    color = GradientModel.GetFromPosition(value);
                }
            }

            ActionFire.Invoke("CREATE_FLAME_VIEWMODEL-PICK_GRADIENT", color);
        }

        #endregion

        #endregion
    }
}