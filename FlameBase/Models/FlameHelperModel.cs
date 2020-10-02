using System;

namespace FlameBase.Models
{
    public static class FlameHelperModel
    {
        public static VariationModel[] GetVariationsFromFlameModel(FlameModel model)
        {
            var length = model.VariationIds.Count;
            var variations = new VariationModel[length];
            var hasParameters = model.Parameters != null;
            var hasWeights = model.Weights != null;
            for (var i = 0; i < length; i++)
            {
                if (!VariationFactoryModel.StaticVariationFactory.TryGet(model.VariationIds[i], out var v))
                    throw new ArgumentOutOfRangeException(nameof(model), @"UNKNOWN VARIATION ID");
                if (hasParameters) v.SetParameters(model.Parameters[i]);
                if (hasWeights) v.W = model.Weights[i];
                variations[i] = v;
            }

            return variations;
        }

        public static TransformModel[] GetTransformationsFromFlameModel(FlameModel model)
        {
            var length = model.Coefficients.Count;
            var transformations = new TransformModel[length];
            var hasGradient = model.GradientPack != null;
            for (var i = 0; i < length; i++)
            {
                var t = new TransformModel();
                var colorPosition = .5;
                if (hasGradient) colorPosition = model.FunctionColorPositions[i];
                var mif = model.IsFinal;
                t.SetFromCoefficients(model.Coefficients[i], model.Coefficients[i][6], model.FunctionColors[i],
                    mif.Count != 0 && model.IsFinal[i], colorPosition);
                transformations[i] = t;
            }

            return transformations;
        }
    }
}