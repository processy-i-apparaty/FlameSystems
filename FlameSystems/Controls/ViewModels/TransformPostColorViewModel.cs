using System.Windows.Input;
using System.Windows.Media;
using FlameBase.Models;
using FlameSystems.Infrastructure;
using FlameSystems.Infrastructure.ValueBind;

namespace FlameSystems.Controls.ViewModels
{
    internal class TransformPostColorViewModel : Notifier
    {
        // private GradientModel _gradientModel;

        private Color _transformColor;
        // private double _transformGradientPosition;

        public TransformPostColorViewModel()
        {
            CommandSelectColor = new RelayCommand(SelectColorHandler);
        }

        [ValueBind]
        public string Text
        {
            get => Get();
            set => Set(value);
        }

        [ValueBind]
        public Brush ColorBrush
        {
            get => Get();
            set => Set(value);
        }

        public ICommand CommandSelectColor { get; }
        public int Id { get; set; }

        // public double TransformGradientPosition
        // {
        //     get => _transformGradientPosition;
        //     set
        //     {
        //         _transformGradientPosition = value;
        //         ColorBrush = new SolidColorBrush(_gradientModel.GetFromPosition(value));
        //     }
        // }

        public Color TransformColor
        {
            get => _transformColor;
            set
            {
                _transformColor = value;
                ColorBrush = new SolidColorBrush(value);
            }
        }


        public void Init(double[] gradientPositions, GradientModel gradientModel)
        {

        }
        private void SelectColorHandler(object obj)
        {
        }
    }
}