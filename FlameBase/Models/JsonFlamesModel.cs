using System.Collections.Generic;
using System.Windows.Media;
using FlameBase.RenderMachine.Models;
using Newtonsoft.Json;

namespace FlameBase.Models
{
    public static class JsonFlamesModel
    {
        private static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        };

        public static FlameModel GetFlameModelFromString(string jsonString)
        {
            return JsonConvert.DeserializeObject<FlameModel>(jsonString, JsonSerializerSettings);
        }

        public static string GetLogDisplayString(LogDisplayModel logDisplay)
        {
            var array = logDisplay.GetArrayCopy();
            return JsonConvert.SerializeObject(array, Formatting.Indented, JsonSerializerSettings);
        }

        public static string GetFlameModelJson(TransformModel[] transformations, VariationModel[] variations,
            ViewSettingsModel viewSettings, GradientModel gradModel)
        {
            var flameModel = FlameModelFromObjects(transformations, variations, viewSettings, gradModel);
            return JsonConvert.SerializeObject(flameModel, Formatting.Indented, JsonSerializerSettings);
        }

        private static FlameModel FlameModelFromObjects(IReadOnlyList<TransformModel> transformations,
            IReadOnlyList<VariationModel> variations,
            ViewSettingsModel viewSettings, GradientModel gradModel)
        {
            var coefficients = new List<double[]>();
            var variationIds = new List<int>();
            var parameters = new List<double[]>();
            var colors = new List<Color>();
            var colorPositions = new List<double>();
            var weights = new List<double>();
            var isFinal = new List<bool>();

            for (var i = 0; i < transformations.Count; i++)
            {
                //todo isFinal
                isFinal.Add(transformations[i].IsFinal);
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

            var flameModel = new FlameModel
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
                FunctionColors = colors,
                BackColor = viewSettings.BackColor,
                IsFinal = isFinal
            };
            if (gradModel != null)
            {
                flameModel.GradientPack = gradModel.Pack();
                flameModel.FunctionColorPositions = colorPositions;
            }

            return flameModel;
        }
    }
}