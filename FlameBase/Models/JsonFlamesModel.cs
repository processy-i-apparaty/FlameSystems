using System.Collections.Generic;
using System.Windows.Media;
using Newtonsoft.Json;

namespace FlameBase.Models
{
    public static class JsonFlamesModel
    {
        private static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        };

        public static FlameModel GetFlameModel(string jsonString)
        {
            return JsonConvert.DeserializeObject<FlameModel>(jsonString, JsonSerializerSettings);
        }


        public static string GetFractalString(TransformModel[] transformations, VariationModel[] variations,
            ViewSettingsModel viewSettings, GradientModel gradModel)
        {
            var coefficients = new List<double[]>();
            var variationIds = new List<int>();
            var parameters = new List<double[]>();
            var colors = new List<Color>();
            var colorPositions = new List<double>();
            var weights = new List<double>();

            for (var i = 0; i < transformations.Length; i++)
            {
                coefficients.Add(new[]
                {
                    transformations[i].A,
                    transformations[i].B,
                    transformations[i].C,
                    transformations[i].D,
                    transformations[i].E,
                    transformations[i].F,
                    transformations[i].Probability
                });
                variationIds.Add(variations[i].Id);
                parameters.Add(new double[variations[i].HasParameters]);
                for (var j = 0; j < variations[i].HasParameters; j++)
                    switch (j)
                    {
                        case 0:
                            parameters[i][j] = variations[i].P1;
                            break;
                        case 1:
                            parameters[i][j] = variations[i].P2;
                            break;
                        case 2:
                            parameters[i][j] = variations[i].P3;
                            break;
                        case 3:
                            parameters[i][j] = variations[i].P4;
                            break;
                    }

                colorPositions.Add(transformations[i].ColorPosition);
                colors.Add(transformations[i].Color);
                weights.Add(variations[i].W);
            }

            var model = new FlameModel
            {
                ImageWidth = viewSettings.ImageWidth,
                ImageHeight = viewSettings.ImageHeight,
                Rotation = viewSettings.Rotation,
                ViewShiftX = viewSettings.ShiftX,
                ViewShiftY = viewSettings.ShiftY,
                Symmetry = viewSettings.Symmetry,
                ViewZoom = viewSettings.Zoom,
                Coefficients = coefficients,
                Weights = weights,
                Parameters = parameters,
                VariationIds = variationIds,
                FunctionColors = colors
            };
            if (gradModel != null)
            {
                model.GradModelPack = gradModel.Pack();
                model.FunctionColorPositions = colorPositions;
            }

            return JsonConvert.SerializeObject(model, Formatting.Indented, JsonSerializerSettings);
        }
    }
}