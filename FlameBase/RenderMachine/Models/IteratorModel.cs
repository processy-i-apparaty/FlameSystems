using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using FlameBase.Models;

namespace FlameBase.RenderMachine.Models
{
    internal class IteratorModel
    {
        private readonly double[] _currentPoint = new double[2];
        private readonly RandomTransformModel _randomT = new RandomTransformModel();
        private TransformModel[] _transforms;
        private VariationModel[] _variations;

        public IteratorModel(RenderPackModel renderPack)
        {
            InitModel(renderPack.Transformations, renderPack.Variations);
        }

        public IteratorModel(IReadOnlyList<TransformModel> transforms,
            IReadOnlyList<VariationModel> variations)
        {
            InitModel(transforms, variations);
        }

        public double[] Points { get; private set; }

        public byte[] ColorIds { get; private set; }

        public int Length
        {
            get
            {
                if (ColorIds == null) return -1;
                return ColorIds.Length;
            }
        }


        private void InitModel(IReadOnlyList<TransformModel> transformations,
            IReadOnlyList<VariationModel> variations)
        {
            _transforms = CopyTransformations(transformations);
            _variations = CopyVariations(variations);
            var random = new Random(Guid.NewGuid().GetHashCode());


            _currentPoint[0] = random.NextDouble() * 2.0 - 1.0;
            _currentPoint[1] = random.NextDouble() * 2.0 - 1.0;


            PreIterate();
        }

        public void Iterate(int iterations)
        {
            _randomT.InitProbabilities(_transforms);

            Points = new double[iterations * 2];
            ColorIds = _randomT.GetRandomT(iterations);

            SetVariations();
            var length = Length;
            if (length <= 0) return;

            var final =  _transforms.SingleOrDefault(transform => transform.IsFinal);
            var index = _transforms.ToList().IndexOf(final);

            for (var i = 0; i < length; i++)
            {
                var pointX = i * 2;
                var pointY = pointX + 1;

                var r = ColorIds[i];
                var f = _transforms[r];
                var v = _variations[r];

                var x = _currentPoint[0];
                var y = _currentPoint[1];


                var dx = f.A * x + f.B * y + f.E;
                var dy = f.C * x + f.D * y + f.F;
                var p2 = v.Fun(new Point(dx, dy));

                _currentPoint[0] = p2.X;
                _currentPoint[1] = p2.Y;

                
                if (final != null)
                {
                    dx = final.A * p2.X + final.B * p2.Y + final.E;
                    dy = final.C * p2.X + final.D * p2.Y + final.F;
                    p2 = _variations[index].Fun(new Point(dx, dy));
                    _currentPoint[0] = p2.X;
                    _currentPoint[1] = p2.Y;
                }

                Points[pointX] = p2.X;
                Points[pointY] = p2.Y;
            }
        }

        private void PreIterate()
        {
            Iterate(20);
            Points = null;
            ColorIds = null;
        }


        private static VariationModel[] CopyVariations(IReadOnlyList<VariationModel> variationModels)
        {
            var variations = new VariationModel[variationModels.Count];
            for (var i = 0; i < variations.Length; i++)
            {
                variations[i] = variationModels[i].Copy();
                variations[i].Init();
            }

            return variations;
        }

        private static TransformModel[] CopyTransformations(IReadOnlyList<TransformModel> transformationModels)
        {
            var transformations = new TransformModel[transformationModels.Count];
            for (var i = 0; i < transformations.Length; i++) transformations[i] = transformationModels[i].Copy();
            return transformations;
        }

        private void SetVariations()
        {
            for (var i = 0; i < _variations.Length; i++)
                if (_variations[i].IsDependent)
                    _variations[i].SetAffineCoefficients(new[]
                    {
                        _transforms[i].A, _transforms[i].B, _transforms[i].C, _transforms[i].D,
                        _transforms[i].E, _transforms[i].F
                    });
        }
    }
}