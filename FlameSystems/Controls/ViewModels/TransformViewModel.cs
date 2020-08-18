using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using FlameBase.Enums;
using FlameBase.Models;
using FlameSystems.Infrastructure;
using FlameSystems.Infrastructure.ActionFire;
using FlameSystems.Infrastructure.ValueBind;

namespace FlameSystems.Controls.ViewModels
{
    internal class TransformViewModel : Notifier
    {
        private readonly string[] _propParameters =
            {"Weight", "Parameter1", "Parameter2", "Parameter3", "Parameter4"};

        private readonly string[] _propValues =
            {"ShiftX", "ShiftY", "Angle", "ScaleX", "ScaleY", "ShearX", "ShearY"};

        private readonly string[] _propValues2 = {"Probability", "ColorBrush"};

        private readonly string[] _propVariation = {"VariationSelected"};
        private readonly TransformModel _transformModel;

        private FlameColorMode _flameColorMode;

        private bool _isFrozen;

        public TransformViewModel()
        {
            _transformModel = new TransformModel();
            _flameColorMode = FlameColorMode.Color;

            Command = new RelayCommand(CommandHandler);
            GradientModel = new GradientModel(Colors.Gray, Colors.Gray);
            ColorPosition = .5;


            FColor = Colors.Gray;
            Parameter1Visibility = Visibility.Collapsed;
            Parameter2Visibility = Visibility.Collapsed;
            Parameter3Visibility = Visibility.Collapsed;
            Parameter4Visibility = Visibility.Collapsed;

            Variations = VariationFactoryModel.StaticVariationFactory.VariationNames;
            VariationSelected = "Linear";

            BindStorage.SetActionFor(ActionValueChanged, _propParameters);
            BindStorage.SetActionFor(ActionValueChanged, _propValues2);
            BindStorage.SetActionFor(ActionTransformValueChanged, _propValues);
            BindStorage.SetActionFor(ActionVariationChanged, _propVariation);

            SetCoefficients();
        }

        #region public

        public VariationModel GetVariation
        {
            get
            {
                if (!VariationFactoryModel.StaticVariationFactory.TryGet(VariationSelected, out var variation))
                    return null;

                variation.P1 = Parameter1;
                variation.P2 = Parameter2;
                variation.P3 = Parameter3;
                variation.P4 = Parameter4;
                variation.W = Weight;
                return variation;
            }
        }

        public TransformModel GetTransform
        {
            get
            {
                SetCoefficients();
                return _transformModel.Copy();
            }
        }


        public void Freeze(bool state)
        {
            _isFrozen = state;
            BindStorage.FreezeFor(state, _propParameters);
            BindStorage.FreezeFor(state, _propValues);
            BindStorage.FreezeFor(state, _propValues2);
            BindStorage.Set("IsVariationSelectEnabled", !state);
        }

        #endregion

        #region bindings

        public int Id { get; set; }
        public GradientModel GradientModel { get; set; }

        public FlameColorMode FlameColorMode
        {
            get => _flameColorMode;
            set
            {
                _flameColorMode = value;
                switch (value)
                {
                    case FlameColorMode.Color:
                        ColorBrush = new SolidColorBrush(FColor);
                        break;
                    case FlameColorMode.Gradient:
                        ColorBrush =
                            new SolidColorBrush(GradientModel.GetFromPosition(ColorPosition));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(value), value, null);
                }
            }
        }

        public double ColorPosition
        {
            get => _transformModel.ColorPosition;
            set
            {
                _transformModel.ColorPosition = value;
                ColorBrush = new SolidColorBrush(GradientModel.GetFromPosition(value));
            }
        }

        public Color FColor
        {
            get => _transformModel.Color;
            set
            {
                _transformModel.Color = value;
                ColorBrush = new SolidColorBrush(value);
            }
        }

        public string[] Variations { get; }

        [ValueBind(0.0, -2.0, 2.0)]
        public double ShiftX
        {
            get => Get();
            set => Set(value);
        }

        [ValueBind(0.0, -2.0, 2.0)]
        public double ShiftY
        {
            get => Get();
            set => Set(value);
        }

        [ValueBind(0.0, 0.0, 360.0, true)]
        public double Angle
        {
            get => Get();
            set => Set(value);
        }

        [ValueBind(1.0, -2.0, 2.0)]
        public double ScaleX
        {
            get => Get();
            set => Set(value);
        }

        [ValueBind(1.0, -2.0, 2.0)]
        public double ScaleY
        {
            get => Get();
            set => Set(value);
        }

        [ValueBind(0.0, -2.0, 2.0)]
        public double ShearX
        {
            get => Get();
            set => Set(value);
        }

        [ValueBind(0.0, -2.0, 2.0)]
        public double ShearY
        {
            get => Get();
            set => Set(value);
        }

        [ValueBind(0.5, 0.0, 1.0)]
        public double Probability
        {
            get => Get();
            set => Set(value);
        }

        [ValueBind(1.0, -10.0, 10.0)]
        public double Weight
        {
            get => Get();
            set => Set(value);
        }

        [ValueBind]
        public SolidColorBrush ColorBrush
        {
            get => Get();
            set => Set(value);
        }

        [ValueBind]
        public string Coefficients
        {
            get => Get();
            set => Set(value);
        }


        [ValueBind]
        public Visibility Parameter1Visibility
        {
            get => Get();
            set => Set(value);
        }

        [ValueBind]
        public Visibility Parameter2Visibility
        {
            get => Get();
            set => Set(value);
        }

        [ValueBind]
        public Visibility Parameter3Visibility
        {
            get => Get();
            set => Set(value);
        }

        [ValueBind]
        public Visibility Parameter4Visibility
        {
            get => Get();
            set => Set(value);
        }


        [ValueBind]
        public string Parameter1Name
        {
            get => Get();
            set => Set(value);
        }

        [ValueBind]
        public string Parameter2Name
        {
            get => Get();
            set => Set(value);
        }

        [ValueBind]
        public string Parameter3Name
        {
            get => Get();
            set => Set(value);
        }

        [ValueBind]
        public string Parameter4Name
        {
            get => Get();
            set => Set(value);
        }

        [ValueBind(1.0, -10.0, 10.0)]
        public double Parameter1
        {
            get => Get();
            set => Set(value);
        }

        [ValueBind(1.0, -10.0, 10.0)]
        public double Parameter2
        {
            get => Get();
            set => Set(value);
        }

        [ValueBind(1.0, -10.0, 10.0)]
        public double Parameter3
        {
            get => Get();
            set => Set(value);
        }

        [ValueBind(1.0, -10.0, 10.0)]
        public double Parameter4
        {
            get => Get();
            set => Set(value);
        }


        [ValueBind]
        public string VariationSelected
        {
            get => Get();
            set => Set(value);
        }

        [ValueBind(true)]
        public bool IsVariationSelectEnabled
        {
            get => Get();
            set => Set(value);
        }

        #endregion

        #region actions

        private void ActionTransformValueChanged(string name, object obj)
        {
            if (_isFrozen) return;
            SetCoefficients();
            ActionFire.Invoke("CREATE_FLAME_VIEWMODEL-CALL_RENDER", "draft");
        }

        private void ActionValueChanged(string name, object obj)
        {
            if (_isFrozen) return;
            ActionFire.Invoke("CREATE_FLAME_VIEWMODEL-CALL_RENDER", "draft");
        }

        private void ActionVariationChanged(string name, object obj)
        {
            if (_isFrozen) return;

            if (VariationFactoryModel.StaticVariationFactory.TryGetParameters(VariationSelected, out var parameterNames,
                out var parameters))
                ShowParameters(parameterNames, parameters);

            ActionFire.Invoke("CREATE_FLAME_VIEWMODEL-CALL_RENDER", "draft");
        }

        #endregion

        #region commands

        public ICommand Command { get; }

        private void CommandHandler(object obj)
        {
            switch ((string) obj)
            {
                case "remove":
                    if (_isFrozen) return;
                    ActionFire.Invoke("CREATE_FLAME_VIEWMODEL-TRANSFORM_REMOVE", Id);
                    break;
                case "randomize":
                    Randomize();
                    break;
                case "selectColor":
                    SelectColor();
                    break;
            }
        }

        #endregion

        #region private



        private void SelectColor()
        {
            if (_isFrozen) return;

            switch (_flameColorMode)
            {
                //TODO: set names for actions
                case FlameColorMode.Color:
                    ActionFire.Invoke("CREATE_FLAME_VIEWMODEL-TRANSFORM_PICK_COLOR", this);
                    break;
                case FlameColorMode.Gradient:
                    ActionFire.Invoke("CREATE_FLAME_VIEWMODEL-TRANSFORM_PICK_GRADIENT_COLOR", this);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void SetTransformation(TransformModel model)
        {
            var props = _propValues.Concat(_propValues2).ToArray();
            BindStorage.TurnActionFor(false, props);

            ShiftX = model.TranslateX;
            ShiftY = model.TranslateY;
            ScaleX = model.ScaleX;
            ScaleY = model.ScaleY;
            ShearX = model.ShearX;
            ShearY = model.ShearY;
            Angle = model.Angle;
            Probability = model.Probability;

            FColor = model.Color;


            if (GradientModel != null) ColorPosition = model.ColorPosition;
            BindStorage.TurnActionFor(true, props);
            SetCoefficients();
        }

        private void Randomize()
        {
            if (_isFrozen) return;
            var props = _propValues.Concat(_propValues2).ToArray();
            BindStorage.TurnActionFor(false, props);
            BindStorage.RandomizeFor(props);
            BindStorage.TurnActionFor(true, props);
            SetCoefficients();
            ActionFire.Invoke("CREATE_FLAME_VIEWMODEL-CALL_RENDER", "draft");
        }


        public void SetVariation(int variationId, double[] parameters, double weight = 1.0)
        {
            var props = _propParameters.Concat(_propVariation).ToArray();
            if (!VariationFactoryModel.StaticVariationFactory.TryGet(variationId, out var variation)) return;
            BindStorage.TurnActionFor(false, props);
            VariationSelected = variation.Name;
            VariationFactoryModel.StaticVariationFactory.TryGetParameters(VariationSelected, out var parameterNames,
                out var paramts);
            ShowParameters(parameterNames, paramts);
            Weight = weight;
            if (parameters != null)
                for (var i = 0; i < variation.HasParameters; i++)
                    switch (i)
                    {
                        case 0:
                            Parameter1 = parameters[0];
                            break;
                        case 1:
                            Parameter2 = parameters[1];
                            break;

                        case 2:
                            Parameter3 = parameters[2];
                            break;

                        case 3:
                            Parameter4 = parameters[3];
                            break;
                    }

            BindStorage.TurnActionFor(true, props);
        }
        
        private void SetCoefficients()
        {
            _transformModel.SetFromValues(new[] {ShiftX, ShiftY, ScaleX, ScaleY, ShearX, ShearY, Angle},
                Probability, FColor, ColorPosition);
            Coefficients =
                $"a {$"{_transformModel.A:0.00}",-5} b {$"{_transformModel.B:0.00}",-5} e {$"{_transformModel.E:0.00}",-5}\nc {$"{_transformModel.C:0.00}",-5} d {$"{_transformModel.D:0.00}",-5} f {$"{_transformModel.F:0.00}",-5}";
        }



        private void ShowParameters(IReadOnlyList<string> parametersNames, IReadOnlyList<double> parameters)
        {
            switch (parametersNames.Count)
            {
                case 0:
                    Parameter1Visibility = Visibility.Collapsed;
                    Parameter2Visibility = Visibility.Collapsed;
                    Parameter3Visibility = Visibility.Collapsed;
                    Parameter4Visibility = Visibility.Collapsed;
                    Parameter1Name = string.Empty;
                    Parameter2Name = string.Empty;
                    Parameter3Name = string.Empty;
                    Parameter4Name = string.Empty;
                    break;
                case 1:
                    Parameter1Visibility = Visibility.Visible;
                    Parameter2Visibility = Visibility.Collapsed;
                    Parameter3Visibility = Visibility.Collapsed;
                    Parameter4Visibility = Visibility.Collapsed;
                    Parameter1Name = parametersNames[0];
                    Parameter2Name = string.Empty;
                    Parameter3Name = string.Empty;
                    Parameter4Name = string.Empty;
                    Parameter1 = parameters[0];
                    break;
                case 2:
                    Parameter1Visibility = Visibility.Visible;
                    Parameter2Visibility = Visibility.Visible;
                    Parameter3Visibility = Visibility.Collapsed;
                    Parameter4Visibility = Visibility.Collapsed;
                    Parameter1Name = parametersNames[0];
                    Parameter2Name = parametersNames[1];
                    Parameter3Name = string.Empty;
                    Parameter4Name = string.Empty;
                    Parameter1 = parameters[0];
                    Parameter2 = parameters[1];
                    break;
                case 3:
                    Parameter1Visibility = Visibility.Visible;
                    Parameter2Visibility = Visibility.Visible;
                    Parameter3Visibility = Visibility.Visible;
                    Parameter4Visibility = Visibility.Collapsed;
                    Parameter1Name = parametersNames[0];
                    Parameter2Name = parametersNames[1];
                    Parameter3Name = parametersNames[2];
                    Parameter4Name = string.Empty;
                    Parameter1 = parameters[0];
                    Parameter2 = parameters[1];
                    Parameter3 = parameters[2];

                    break;
                case 4:
                    Parameter1Visibility = Visibility.Visible;
                    Parameter2Visibility = Visibility.Visible;
                    Parameter3Visibility = Visibility.Visible;
                    Parameter4Visibility = Visibility.Visible;
                    Parameter1Name = parametersNames[0];
                    Parameter2Name = parametersNames[1];
                    Parameter3Name = parametersNames[2];
                    Parameter4Name = parametersNames[3];
                    Parameter1 = parameters[0];
                    Parameter2 = parameters[1];
                    Parameter3 = parameters[2];
                    Parameter4 = parameters[3];
                    break;
            }
        }

        #endregion
    }
}