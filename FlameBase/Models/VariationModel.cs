using System.Windows;

namespace FlameBase.Models
{
    public abstract class VariationModel
    {
        private readonly string[] _parameterNames = new string[4];

        private readonly double[] _parameters = new double[4];

        protected double[] AffineCoefficients = new double[6];

        protected VariationModel()
        {
            Name = GetType().Name;
        }

        public abstract int Id { get; }
        public abstract int HasParameters { get; }
        public abstract bool IsDependent { get; }
        public string Name { get; set; }
        protected VariationHelperModel VariationHelper { get; } = new VariationHelperModel();

        protected double A => AffineCoefficients[0];
        protected double B => AffineCoefficients[1];
        protected double C => AffineCoefficients[2];
        protected double D => AffineCoefficients[3];
        protected double E => AffineCoefficients[4];
        protected double F => AffineCoefficients[5];

        public double W { get; set; } = 1.0;


        public double P1
        {
            get => _parameters[0];
            set => _parameters[0] = value;
        }

        public double P2
        {
            get => _parameters[1];
            set => _parameters[1] = value;
        }

        public double P3
        {
            get => _parameters[2];
            set => _parameters[2] = value;
        }

        public double P4
        {
            get => _parameters[3];
            set => _parameters[3] = value;
        }

        public string P1Name
        {
            get => _parameterNames[0];
            set => _parameterNames[0] = value;
        }

        public string P2Name
        {
            get => _parameterNames[1];
            set => _parameterNames[1] = value;
        }

        public string P3Name
        {
            get => _parameterNames[2];
            set => _parameterNames[2] = value;
        }

        public string P4Name
        {
            get => _parameterNames[3];
            set => _parameterNames[3] = value;
        }

        public abstract Point Fun(Point p);

        public abstract void Init();

        public void SetAffineCoefficients(double[] a)
        {
            for (var i = 0; i < AffineCoefficients.Length; i++) AffineCoefficients[i] = a[i];
        }

        public void SetParameters(double[] parameters, string[] parameterNames)
        {
            for (var i = 0; i < HasParameters; i++)
            {
                _parameters[i] = parameters[i];
                _parameterNames[i] = parameterNames[i];
            }
        }

        public void SetParameters(double[] parameters)
        {
            if (parameters == null) return;
            for (var i = 0; i < HasParameters; i++) _parameters[i] = parameters[i];
        }


        public override string ToString()
        {
            return VariationHelper.GetString(this);
        }

        public VariationModel Copy()
        {
            VariationFactoryModel.StaticVariationFactory.TryGet(Name, out var model);
            for (var i = 0; i < HasParameters; i++) model._parameters[i] = _parameters[i];
            model.W = W;
            return model;
        }
    }
}