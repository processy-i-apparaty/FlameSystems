using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ColorMine.ColorSpaces;
using ConsoleWpfApp.Infrastructure;
using ConsoleWpfApp.Infrastructure.ValueBind;
using ConsoleWpfApp.Pickers.Models;
using FlameBase.Enums;

namespace ConsoleWpfApp.Pickers.ViewModels
{
    internal class ColorPickViewModel : Notifier
    {
        private readonly Dictionary<string, ColorPickerMode> _elementModes = new Dictionary<string, ColorPickerMode>
        {
            {"RadioH", ColorPickerMode.H},
            {"RadioS", ColorPickerMode.S},
            {"RadioV", ColorPickerMode.V},
            {"RadioR", ColorPickerMode.R},
            {"RadioG", ColorPickerMode.G},
            {"RadioB", ColorPickerMode.B}
        };

        private readonly Color _initialColor;
        private readonly Action<bool, Color> _callback;

        private readonly Dictionary<string, int> _textCodes = new Dictionary<string, int>
        {
            {"TextH", 0},
            {"TextS", 1},
            {"TextV", 2},
            {"TextR", 3},
            {"TextG", 4},
            {"TextB", 5},
            {"TextHex", 6}
        };

        private Canvas _canvasColumn;
        private Canvas _canvasCube;
        private ColorPickerMode _colorMode;
        private Canvas _columnArrows;
        private bool _columnDrag;
        private double _columnY;
        private bool _cubeDrag;
        private Ellipse _cubeEllipse;
        private Point _cubeXy;
        private int _textFromTextField = -1;

        public ColorPickViewModel(Color initialColor, Action<bool, Color> callback)
        {
            _initialColor = initialColor;
            _callback = callback;

            Init();
        }

        public bool Initiated { get; set; }

        private void Init()
        {
            CommandCanvasLoaded = new RelayCommand(CommandCanvasLoadedHandler);
            CommandCanvasMouseDown = new RelayCommand(CommandCanvasMouseDownHandler);
            CommandCanvasMouseUp = new RelayCommand(CommandCanvasMouseUpHandler);
            CommandCanvasMouseMove = new RelayCommand(CommandCanvasMouseMoveHandler);
            Command = new RelayCommand(CommandHandler);

            _cubeEllipse = new Ellipse
            {
                Width = 16.0,
                Height = 16.0,
                Stroke = Brushes.WhiteSmoke,
                StrokeThickness = 1
            };
            _columnArrows = Application.Current.TryFindResource("ArrowsBox") as Canvas;
            if (_columnArrows != null) _columnArrows.Height = 8.0;

            ColorCurrent = new SolidColorBrush(_initialColor);
            RadioH = true;
            BindStorage.SetActionFor(ActRadio, "RadioH", "RadioS", "RadioV", "RadioR", "RadioG", "RadioB");
            BindStorage.SetActionFor(ActText, "TextH", "TextS", "TextV", "TextR", "TextG", "TextB", "TextHex");
        }

        #region buttons

        private void CommandHandler(object obj)
        {
            switch ((string) obj)
            {
                case "ok":
                    _canvasColumn.Children.Remove(_columnArrows);
                    _callback.Invoke(true, ColorNew.Color);
                    break;
                case "cancel":
                    _canvasColumn.Children.Remove(_columnArrows);
                    _callback.Invoke(false, ColorCurrent.Color);
                    break;
                case "current":
                    ColorNew = new SolidColorBrush(ColorCurrent.Color);
                    ColorPickerCoordinatesModel.Get(ColorNew.Color, _colorMode, out _cubeXy, out _columnY);
                    ColorCube = ColorPickerCubeModel.Get(_colorMode, _columnY, (int) _canvasCube.ActualWidth);
                    ColorColumn = ColorPickerColumnModel.Get(_cubeXy, _colorMode, (int) _canvasColumn.ActualWidth,
                        (int) _canvasColumn.ActualHeight);
                    SetIndicators();
                    break;
            }
        }

        #endregion

        #region acts

        private void ActRadio(string name, object state)
        {
            if (!(bool) state) return;
            _colorMode = _elementModes[name];
            ColorPickerCoordinatesModel.Get(ColorNew.Color, _colorMode, out _cubeXy, out _columnY);
            SetIndicators();
            ColorCube = ColorPickerCubeModel.Get(_colorMode, _columnY, (int) _canvasCube.ActualWidth);
            ColorColumn = ColorPickerColumnModel.Get(_cubeXy, _colorMode, (int) _canvasColumn.ActualWidth,
                (int) _canvasColumn.ActualHeight);
            SetText(ColorNew.Color);
        }

        private void ActText(string name, object num)
        {
            var code = _textCodes[name];
            SetText(code, num);
        }

        #endregion acts

        #region reactions

        private void ReactMouseCube(Point p)
        {
            var moved = false;

            if (p.X < 0) p.X = 0;
            if (p.X > _canvasCube.ActualWidth) p.X = _canvasCube.ActualWidth;
            if (Math.Abs(p.X - _cubeXy.X) > 1E-10)
            {
                moved = true;
                _cubeXy.X = p.X / _canvasCube.ActualWidth;
            }

            if (p.Y < 0) p.Y = 0;
            if (p.Y > _canvasCube.ActualHeight) p.Y = _canvasCube.ActualHeight;
            if (Math.Abs(p.Y - _cubeXy.Y) > 1E-10)
            {
                moved = true;
                _cubeXy.Y = p.Y / _canvasCube.ActualHeight;
            }

            if (!moved) return;

            SetIndicatorEllipse(p);
            if (_colorMode != 0)
                ColorColumn = ColorPickerColumnModel.Get(_cubeXy, _colorMode, (int) _canvasColumn.ActualWidth,
                    (int) _canvasColumn.ActualHeight);
            ColorNew.Color = ColorPickerColorsModel.GetColor(_cubeXy, _columnY, _colorMode);
            SetText(ColorNew.Color);
        }

        private void ReactMouseColumn(double y)
        {
            if (y < 0) y = 0;
            if (y > _canvasColumn.ActualHeight) y = _canvasColumn.ActualHeight;

            var columnY = y / _canvasColumn.ActualHeight;
            if (Math.Abs(_columnY - columnY) < 1E-10) return;

            _columnY = y / _canvasColumn.ActualHeight;
            SetIndicatorArrows();
            ColorCube = ColorPickerCubeModel.Get(_colorMode, _columnY, (int) _canvasCube.ActualWidth);
            ColorNew.Color = ColorPickerColorsModel.GetColor(_cubeXy, _columnY, _colorMode);
            SetText(ColorNew.Color);
        }

        #endregion reactions

        #region indicators

        private void SetIndicators()
        {
            SetIndicatorArrows();
            SetIndicatorEllipse();
        }

        private void SetIndicatorArrows()
        {
            Canvas.SetTop(_columnArrows, _columnY * _canvasColumn.ActualHeight - 4.0);
        }

        private void SetIndicatorEllipse(Point p)
        {
            if (p.X >= 0 && p.X < _canvasCube.ActualWidth) Canvas.SetLeft(_cubeEllipse, p.X - 8.0);
            if (p.Y >= 0 && p.Y < _canvasCube.ActualHeight) Canvas.SetTop(_cubeEllipse, p.Y - 8.0);
        }

        private void SetIndicatorEllipse()
        {
            Canvas.SetLeft(_cubeEllipse, _cubeXy.X * _canvasCube.ActualWidth - 8.0);
            Canvas.SetTop(_cubeEllipse, _cubeXy.Y * _canvasCube.ActualHeight - 8.0);
        }

        #endregion indicators

        #region bind

        [ValueBind]
        public WriteableBitmap ColorCube
        {
            get => Get();
            set => Set(value);
        }

        [ValueBind]
        public WriteableBitmap ColorColumn
        {
            get => Get();
            set => Set(value);
        }

        [ValueBind]
        public SolidColorBrush ColorCurrent
        {
            get => Get();
            set => Set(value);
        }

        [ValueBind]
        public SolidColorBrush ColorNew
        {
            get => Get();
            set => Set(value);
        }

        [ValueBind]
        public bool RadioH
        {
            get => Get();
            set => Set(value);
        }

        [ValueBind]
        public bool RadioS
        {
            get => Get();
            set => Set(value);
        }

        [ValueBind]
        public bool RadioV
        {
            get => Get();
            set => Set(value);
        }

        [ValueBind]
        public bool RadioR
        {
            get => Get();
            set => Set(value);
        }

        [ValueBind]
        public bool RadioG
        {
            get => Get();
            set => Set(value);
        }

        [ValueBind]
        public bool RadioB
        {
            get => Get();
            set => Set(value);
        }

        [ValueBind]
        public int TextH
        {
            get => Get();
            set => Set(value);
        }

        [ValueBind]
        public int TextS
        {
            get => Get();
            set => Set(value);
        }

        [ValueBind]
        public int TextV
        {
            get => Get();
            set => Set(value);
        }

        [ValueBind]
        public int TextR
        {
            get => Get();
            set => Set(value);
        }

        [ValueBind]
        public int TextG
        {
            get => Get();
            set => Set(value);
        }

        [ValueBind]
        public int TextB
        {
            get => Get();
            set => Set(value);
        }

        [ValueBind]
        public string TextHex
        {
            get => Get();
            set => Set(value);
        }


        public ICommand CommandCanvasLoaded { get; set; }

        public ICommand CommandCanvasMouseDown { get; set; }

        public ICommand CommandCanvasMouseUp { get; set; }

        public ICommand CommandCanvasMouseMove { get; set; }

        public ICommand Command { get; set; }

        #endregion bind

        #region initiate

        private void Initiate()
        {
            //            Debug.WriteLine($"\t@ {GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
            _colorMode = ColorPickerMode.H;


            ColorPickerCoordinatesModel.Get(ColorCurrent.Color, _colorMode, out _cubeXy, out _columnY);
            ColorCube = ColorPickerCubeModel.Get(_colorMode, _columnY, (int) _canvasCube.ActualWidth);
            ColorColumn = ColorPickerColumnModel.Get(_cubeXy, _colorMode, (int) _canvasColumn.ActualWidth,
                (int) _canvasColumn.ActualHeight);
            SetIndicators();
            ColorNew = new SolidColorBrush(ColorCurrent.Color);
            SetText(ColorNew.Color);
            Initiated = true;
        }

        private void CommandCanvasLoadedHandler(object obj)
        {
            var canvas = (Canvas) obj;

            //            Debug.WriteLine($"\t@ {GetType().Name}.{MethodBase.GetCurrentMethod().Name}: {canvas.Name}");
            switch (canvas.Name)
            {
                case "CanvasCube":
                    _canvasCube = canvas;
                    _canvasCube.Children.Add(_cubeEllipse);
                    if (_canvasColumn != null) Initiate();
                    break;
                case "CanvasColumn":
                    _canvasColumn = canvas;
                    _canvasColumn.Children.Add(_columnArrows);
                    if (_canvasCube != null) Initiate();
                    break;
            }
        }

        #endregion initiate

        #region mouse

        private void CommandCanvasMouseUpHandler(object obj)
        {
            var who = (string) obj;
            switch (who)
            {
                case "cube":
                    _cubeDrag = false;
                    Mouse.Capture(null);
                    break;
                case "column":
                    _columnDrag = false;
                    Mouse.Capture(null);
                    break;
            }
        }

        private void CommandCanvasMouseDownHandler(object obj)
        {
            var who = (string) obj;
            switch (who)
            {
                case "cube":
                    _cubeDrag = true;
                    Mouse.Capture(_canvasCube);
                    ReactMouseCube(Mouse.GetPosition(_canvasCube));
                    return;
                case "column":
                    _columnDrag = true;
                    Mouse.Capture(_canvasColumn);
                    ReactMouseColumn(Mouse.GetPosition(_canvasColumn).Y);
                    return;
            }
        }

        private void CommandCanvasMouseMoveHandler(object obj)
        {
            var who = (string) obj;
            switch (who)
            {
                case "cube":
                    if (!_cubeDrag) return;
                    ReactMouseCube(Mouse.GetPosition(_canvasCube));
                    return;
                case "column":
                    if (!_columnDrag) return;
                    ReactMouseColumn(Mouse.GetPosition(_canvasColumn).Y);
                    return;
            }
        }

        #endregion mouse

        #region text

        private void SetText(Color c)
        {
            var rgb = new Rgb
            {
                R = c.R,
                G = c.G,
                B = c.B
            };
            var hsb = rgb.To<Hsb>();

            var texts = _textCodes.Keys.ToArray();
            BindStorage.TurnActionFor(false, texts);

            if (_textFromTextField != 0)
                TextH = (int) Math.Round(hsb.H);
            if (_textFromTextField != 1)
                TextS = (int) Math.Round(hsb.S * 100.0);
            if (_textFromTextField != 2)
                TextV = (int) Math.Round(hsb.B * 100.0);
            if (_textFromTextField != 3)
                TextR = (int) Math.Round(rgb.R);
            if (_textFromTextField != 4)
                TextG = (int) Math.Round(rgb.G);
            if (_textFromTextField != 5)
                TextB = (int) Math.Round(rgb.B);
            if (_textFromTextField != 6)
                TextHex = c.ToString().Substring(3, 6).ToLower();

            BindStorage.TurnActionFor(true, texts);
            _textFromTextField = -1;
        }

        private void SetText(int code, object num)
        {
            var intNum = -1;
            var isNums = true;
            var isHsb = false;

            if (num is int n) intNum = n;
            else isNums = false;

            var nums = new[] {TextH, TextS, TextV, TextR, TextG, TextB};
            var color = new Color();
            if (code >= 0 && code < 6) nums[code] = intNum;
            var isOk = false;
            if (code >= 0 && code <= 2) isHsb = true;

            switch (code)
            {
                case 0:
                    if (intNum >= 0 && intNum <= 360) isOk = true;
                    break;
                case 1:
                    if (intNum >= 0 && intNum <= 100) isOk = true;
                    break;
                case 2:
                    if (intNum >= 0 && intNum <= 100) isOk = true;
                    break;
                case 3:
                    if (intNum >= 0 && intNum <= 255) isOk = true;
                    break;
                case 4:
                    if (intNum >= 0 && intNum <= 255) isOk = true;
                    break;
                case 5:
                    if (intNum >= 0 && intNum <= 255) isOk = true;
                    break;
                case 6:
                    if (ColorPickerColorsModel.TryParseHexColor((string) num, out color))
                        isOk = true;
                    break;
            }

            if (isNums && isOk)
            {
                var texts = _textCodes.Keys.ToArray();
                BindStorage.TurnActionFor(false, texts);

                if (isHsb)
                {
                    var hsb = new Hsb
                    {
                        H = nums[0],
                        S = nums[1] * 0.01,
                        B = nums[2] * 0.01
                    };
                    var rgb = hsb.To<Rgb>();
                    if (code != 0) TextH = nums[0];
                    if (code != 1) TextS = nums[1];
                    if (code != 2) TextV = nums[2];
                    TextR = (int) Math.Round(rgb.R);
                    TextG = (int) Math.Round(rgb.G);
                    TextB = (int) Math.Round(rgb.B);
                    var tColor = Color.FromRgb((byte) TextR, (byte) TextG, (byte) TextB);
                    TextHex = tColor.ToString().Substring(3, 6)
                        .ToLower();

                    ColorPickerCoordinatesModel.Get(tColor, _colorMode, out _cubeXy, out _columnY);
                    ColorCube = ColorPickerCubeModel.Get(_colorMode, _columnY, (int) _canvasCube.ActualWidth);
                    ColorColumn = ColorPickerColumnModel.Get(_cubeXy, _colorMode, (int) _canvasColumn.ActualWidth,
                        (int) _canvasColumn.ActualHeight);
                    SetIndicators();
                    ColorNew = new SolidColorBrush(tColor);
                }
                else
                {
                    var rgb = new Rgb
                    {
                        R = nums[3],
                        G = nums[4],
                        B = nums[5]
                    };
                    var hsb = rgb.To<Hsb>();
                    TextH = (int) Math.Round(hsb.H);
                    TextS = (int) Math.Round(hsb.S * 100.0);
                    TextV = (int) Math.Round(hsb.B * 100.0);
                    if (code != 3) TextR = nums[3];
                    if (code != 4) TextG = nums[4];
                    if (code != 5) TextB = nums[5];
                    TextHex = Color.FromRgb((byte) TextR, (byte) TextG, (byte) TextB).ToString().Substring(3, 6)
                        .ToLower();

                    var tColor = Color.FromRgb((byte) nums[3], (byte) nums[4], (byte) nums[5]);
                    ColorPickerCoordinatesModel.Get(tColor, _colorMode, out _cubeXy, out _columnY);
                    ColorCube = ColorPickerCubeModel.Get(_colorMode, _columnY, (int) _canvasCube.ActualWidth);
                    ColorColumn = ColorPickerColumnModel.Get(_cubeXy, _colorMode, (int) _canvasColumn.ActualWidth,
                        (int) _canvasColumn.ActualHeight);
                    SetIndicators();
                    ColorNew = new SolidColorBrush(tColor);
                }

                BindStorage.TurnActionFor(true, texts);
            }
            else if (!isNums && isOk)
            {
                var texts = _textCodes.Keys.ToArray();
                BindStorage.TurnActionFor(false, texts);

                var rgb = new Rgb
                {
                    R = color.R,
                    G = color.G,
                    B = color.B
                };
                var hsb = rgb.To<Hsb>();
                TextH = (int) Math.Round(hsb.H);
                TextS = (int) Math.Round(hsb.S * 100.0);
                TextV = (int) Math.Round(hsb.B * 100.0);
                TextR = (int) Math.Round(rgb.R);
                TextG = (int) Math.Round(rgb.G);
                TextB = (int) Math.Round(rgb.B);

                ColorPickerCoordinatesModel.Get(color, _colorMode, out _cubeXy, out _columnY);
                ColorCube = ColorPickerCubeModel.Get(_colorMode, _columnY, (int) _canvasCube.ActualWidth);
                ColorColumn = ColorPickerColumnModel.Get(_cubeXy, _colorMode, (int) _canvasColumn.ActualWidth,
                    (int) _canvasColumn.ActualHeight);
                SetIndicators();
                ColorNew = new SolidColorBrush(color);

                BindStorage.TurnActionFor(true, texts);
            }
        }

        #endregion text
    }
}