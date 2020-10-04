using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using FlameBase.Models;
using FlameSystems.Infrastructure;
using FlameSystems.Infrastructure.ActionFire;
using FlameSystems.Infrastructure.ValueBind;

namespace FlameSystems.Connectors.Base
{
    public class PanelTransformViewModel : Notifier
    {
        public PanelTransformViewModel()
        {
            Command = new RelayCommand(HandlerCommand);
            Id = Guid.NewGuid().GetHashCode();
            BindStorage.SetActionAll(HandlerItemChanged);
        }

        private void HandlerItemChanged(string arg1, object arg2)
        {
            Debug.WriteLine($"HandlerItemChanged {Id} {arg1} {arg2}");
        }

        public ICommand Command { get; }


        public int Id { get; }


        private void HandlerCommand(object obj)
        {
            var command = (string) obj;
            switch (command)
            {
                case "remove":
                    ActionFire.Invoke("panel-transform-remove", Id);
                    break;
                case "randomize":
                    ActionFire.Invoke("panel-transform-randomize", Id);
                    break;
                case "selectColor":
                    ActionFire.Invoke("panel-transform-select-color", Id);
                    break;
            }
        }

        #region binding

        [ValueBind]
        public double ShiftX
        {
            get
            {
                var g = Get();
                return g;
            }
            set => Set(value);
        }

        [ValueBind]
        public double ShiftY
        {
            get
            {
                var g = Get();
                return g;
            }
            set => Set(value);
        }

        [ValueBind]
        public double Angle
        {
            get
            {
                var g = Get();
                return g;
            }
            set => Set(value);
        }


        [ValueBind]
        public double ScaleX
        {
            get
            {
                var g = Get();
                return g;
            }
            set => Set(value);
        }

        [ValueBind]
        public double ScaleY
        {
            get
            {
                var g = Get();
                return g;
            }
            set => Set(value);
        }

        [ValueBind]
        public double ShearX
        {
            get
            {
                var g = Get();
                return g;
            }
            set => Set(value);
        }

        [ValueBind]
        public double ShearY
        {
            get
            {
                var g = Get();
                return g;
            }
            set => Set(value);
        }

        [ValueBind]
        public double Probability
        {
            get
            {
                var g = Get();
                return g;
            }
            set => Set(value);
        }


        [ValueBind]
        public double Parameter1
        {
            get
            {
                var g = Get();
                return g;
            }
            set => Set(value);
        }


        [ValueBind]
        public double Parameter2
        {
            get
            {
                var g = Get();
                return g;
            }
            set => Set(value);
        }


        [ValueBind]
        public double Parameter3
        {
            get
            {
                var g = Get();
                return g;
            }
            set => Set(value);
        }


        [ValueBind]
        public double Parameter4
        {
            get
            {
                var g = Get();
                return g;
            }
            set => Set(value);
        }


        [ValueBind]
        public SolidColorBrush ColorBrush
        {
            get
            {
                var g = Get();
                return g;
            }
            set => Set(value);
        }

        [ValueBind]
        public Visibility Parameter1Visibility
        {
            get
            {
                var g = Get();
                return g;
            }
            set => Set(value);
        }


        [ValueBind]
        public List<VariationModel> Variations
        {
            get
            {
                var g = Get();
                return g;
            }
            set => Set(value);
        }

        [ValueBind]
        public VariationModel VariationSelected
        {
            get
            {
                var g = Get();
                return g;
            }
            set => Set(value);
        }

        [ValueBind]
        public bool IsVariationSelectEnabled
        {
            get
            {
                var g = Get();
                return g;
            }
            set => Set(value);
        }

        [ValueBind]
        public Visibility WeightVisibility
        {
            get
            {
                var g = Get();
                return g;
            }
            set => Set(value);
        }

        [ValueBind]
        public string Weight
        {
            get
            {
                var g = Get();
                return g;
            }
            set => Set(value);
        }

        [ValueBind]
        public string Coefficients
        {
            get
            {
                var g = Get();
                return g;
            }
            set => Set(value);
        }

        [ValueBind]
        public Visibility Parameter2Visibility
        {
            get
            {
                var g = Get();
                return g;
            }
            set => Set(value);
        }

        [ValueBind]
        public Visibility Parameter3Visibility
        {
            get
            {
                var g = Get();
                return g;
            }
            set => Set(value);
        }

        [ValueBind]
        public Visibility Parameter4Visibility
        {
            get
            {
                var g = Get();
                return g;
            }
            set => Set(value);
        }

        [ValueBind]
        public string Parameter1Name
        {
            get
            {
                var g = Get();
                return g;
            }
            set => Set(value);
        }

        [ValueBind]
        public string Parameter2Name
        {
            get
            {
                var g = Get();
                return g;
            }
            set => Set(value);
        }

        [ValueBind]
        public string Parameter3Name
        {
            get
            {
                var g = Get();
                return g;
            }
            set => Set(value);
        }

        [ValueBind]
        public string Parameter4Name
        {
            get
            {
                var g = Get();
                return g;
            }
            set => Set(value);
        }

        #endregion
    }
}