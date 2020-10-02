using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using ConsoleWpfApp.Infrastructure;
using ConsoleWpfApp.Infrastructure.ValueBind;

namespace ConsoleWpfApp.Pickers.ViewModels
{
    internal class CurvePickerViewModel : Notifier
    {
        private const int LinesCount = 25;
        private const int LineFatCount = 5;
        private Canvas _canvasLayer1;
        private Canvas _canvasLayer2;
        private Line[] _gridLinesHorizontal;
        private Line[] _gridLinesVertical;
        private bool _isGridInitialized;

        public CurvePickerViewModel()
        {
            CommandLoaded = new RelayCommand(HandlerLoaded);
            CommandGridSizeChanged = new RelayCommand(HandlerGridSizeChanged);
            BindStorage.SetActionFor("GridSize", ActionGridSize);
            Init();
        }

        private async void Init()
        {
            await Task.Run(() =>
            {
                while (!IsInitialized) Thread.Sleep(5);
            });
            RedrawGrid();
        }

        private void ActionGridSize(string arg1, object arg2)
        {
            RedrawGrid();
        }

        #region bindings

        public bool IsInitialized => _canvasLayer1 != null && _canvasLayer2 != null && _isGridInitialized;

        [ValueBind]
        public Size GridSize
        {
            get => Get();
            set => Set(value);
        }

        public ICommand CommandLoaded { get; }
        public ICommand CommandGridSizeChanged { get; }

        #endregion

        #region handlers

        private void HandlerGridSizeChanged(object obj)
        {
            var grid = (Grid) obj;
            GridSize = new Size(grid.ActualWidth, grid.ActualHeight);
            RedrawGrid();
        }

        private void HandlerLoaded(object obj)
        {
            if (obj is Grid grid)
            {
                HandlerGridSizeChanged(grid);
                _isGridInitialized = true;
                return;
            }

            var canvas = (Canvas) obj;
            switch (canvas.Name)
            {
                case "CanvasLayer1":
                    _canvasLayer1 = canvas;
                    break;
                case "CanvasLayer2":
                    _canvasLayer2 = canvas;
                    break;
            }
        }

        #endregion

        #region grid

        private void RedrawGrid()
        {
            if (_canvasLayer1 == null) return;
            if (_gridLinesVertical == null)
                CreateGrid();

            UpdateGrid();
        }

        private void UpdateGrid()
        {
            var stepX = GridSize.Width / LinesCount;
            var stepY = GridSize.Height / LinesCount;
            var dx = stepX;
            var dy = stepY;
            for (var i = 0; i < LinesCount - 1; i++)
            {
                var lineV = _gridLinesVertical[i];
                var lineH = _gridLinesHorizontal[i];

                lineV.X1 = dx;
                lineV.X2 = dx;
                lineV.Y1 = 0.0;
                lineV.Y2 = GridSize.Height;

                lineH.X1 = 0.0;
                lineH.X2 = GridSize.Width;
                lineH.Y1 = dy;
                lineH.Y2 = dy;
                dx += stepX;
                dy += stepY;
            }
        }

        private void CreateGrid()
        {
            _gridLinesVertical = new Line[LinesCount - 1];
            _gridLinesHorizontal = new Line[LinesCount - 1];

            var brushFat = new SolidColorBrush(Color.FromArgb(30, 255, 255, 255));
            var brushThin = new SolidColorBrush(Color.FromArgb(10, 255, 255, 255));

            for (var i = 1; i < LinesCount; i++)
            {
                var stroke = i % LineFatCount == 0 ? brushFat : brushThin;

                var lineV = new Line
                {
                    Stroke = stroke,
                    StrokeThickness = 1.0
                };
                var lineH = new Line
                {
                    Stroke = stroke,
                    StrokeThickness = 1.0
                };
                _gridLinesVertical[i - 1] = lineV;
                _gridLinesHorizontal[i - 1] = lineH;
                _canvasLayer1.Children.Add(lineV);
                _canvasLayer1.Children.Add(lineH);
            }
        }

        #endregion
    }
}