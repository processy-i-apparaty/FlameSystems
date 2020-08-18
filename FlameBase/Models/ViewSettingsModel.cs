using System.Windows.Media;
using FlameBase.FlameMath;

namespace FlameBase.Models
{
    public class ViewSettingsModel
    {
        private int _imageHeight = 1;
        private int _imageWidth = 1;
        private double _rotation;
        private int _symmetry = 1;

        public ViewSettingsModel(int imageWidth, int imageHeight)
        {
            ImageWidth = imageWidth;
            ImageHeight = imageHeight;
        }

        public ViewSettingsModel(int imageWidth, int imageHeight, double shiftX, double shiftY, double zoom,
            double rotation,
            int symmetry, Color backColor)
        {
            Rotation = rotation;
            ImageWidth = imageWidth;
            ImageHeight = imageHeight;


            ShiftX = shiftX;
            ShiftY = shiftY;
            Zoom = zoom;
            Symmetry = symmetry;
            BackColor = backColor;
        }

        public Color BackColor { get; set; }

        public double HalfWidth { get; private set; }
        public double HalfHeight { get; private set; }

        public int ImageWidth
        {
            get => _imageWidth;
            set
            {
                _imageWidth = value;
                if (_imageWidth <= 0) _imageWidth = 1;
                ImageAspect = 1.0 * _imageHeight / _imageWidth;
                HalfWidth = ImageWidth / 2.0;
            }
        }

        public int ImageHeight
        {
            get => _imageHeight;
            set
            {
                _imageHeight = value;
                if (_imageHeight <= 0) _imageHeight = 1;
                ImageAspect = 1.0 * _imageHeight / _imageWidth;

                HalfHeight = ImageHeight / 2.0;
            }
        }

        public double ImageAspect { get; private set; }
        public double ShiftX { get; set; }
        public double ShiftY { get; set; }
        public double Zoom { get; set; } = 1.0;

        public double Rotation
        {
            get => _rotation;
            set
            {
                _rotation = value;
                while (_rotation > Trigonometry.TwoPi) _rotation -= Trigonometry.TwoPi;
                while (_rotation < 0) _rotation += Trigonometry.TwoPi;
            }
        }

        public double RotationDegrees
        {
            get => Rotation * Trigonometry.ToDegrees;
            set => Rotation = value * Trigonometry.ToRadians;
        }

        public int Symmetry
        {
            get => _symmetry;
            set => _symmetry = value < 1 ? 1 : value;
        }

        public void Reset()
        {
            Rotation = 0.0;
            ShiftX = 0.0;
            ShiftY = 0.0;
            Symmetry = 1;
            Zoom = 1.0;
        }

        public ViewSettingsModel Clone()
        {
            var s = new ViewSettingsModel(ImageWidth, ImageHeight)
            {
                ShiftX = ShiftX,
                ShiftY = ShiftY,
                Zoom = Zoom,
                Rotation = Rotation,
                Symmetry = Symmetry
            };
            return s;
        }
    }
}