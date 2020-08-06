using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FlameBase.Models
{
    public class VariationFactoryModel
    {
        public static VariationFactoryModel StaticVariationFactory = new VariationFactoryModel();


        private readonly Dictionary<int, int> _ids = new Dictionary<int, int>();
        private readonly Dictionary<string, int> _names = new Dictionary<string, int>();
        private readonly Dictionary<string, double[]> _parameters = new Dictionary<string, double[]>();
        private readonly Dictionary<string, string[]> _parametersNames = new Dictionary<string, string[]>();
        private readonly Type[] _types;

        public VariationFactoryModel()
        {
            //TODO: assemblyFile
            var assemblyFile = $"{AppDomain.CurrentDomain.BaseDirectory}\\VariationsLibrary.dll";
            var assembly = Assembly.LoadFrom(assemblyFile);
            _types = assembly.GetExportedTypes().Where(t => t.IsClass).ToArray();
            Array.Sort(_types, (a, b) => string.CompareOrdinal(a.Name, b.Name));
            for (var i = 0; i < _types.Length; i++)
            {
                var instance = (VariationModel) Activator.CreateInstance(_types[i]);
                var name = instance.Name;
                _names.Add(name, i);
                _ids.Add(instance.Id, i);
                _parametersNames.Add(name, GetParameterNames(instance));
                _parameters.Add(name, GetParameters(instance));
            }

            VariationNames = _names.Keys.ToArray();
        }

        public string[] VariationNames { get; }

        private static string[] GetParameterNames(VariationModel variationModel)
        {
            var names = new string[variationModel.HasParameters];
            for (var i = 0; i < names.Length; i++)
                switch (i)
                {
                    case 0:
                        names[i] = variationModel.P1Name;
                        break;
                    case 1:
                        names[i] = variationModel.P2Name;
                        break;
                    case 2:
                        names[i] = variationModel.P3Name;
                        break;
                    case 3:
                        names[i] = variationModel.P4Name;
                        break;
                }

            return names;
        }

        private static double[] GetParameters(VariationModel variationModel)
        {
            var parameters = new double[variationModel.HasParameters];
            for (var i = 0; i < parameters.Length; i++)
                switch (i)
                {
                    case 0:
                        parameters[i] = variationModel.P1;
                        break;
                    case 1:
                        parameters[i] = variationModel.P2;
                        break;
                    case 2:
                        parameters[i] = variationModel.P3;
                        break;
                    case 3:
                        parameters[i] = variationModel.P4;
                        break;
                }

            return parameters;
        }

        public bool TryGet(int id, out VariationModel variation)
        {
            variation = null;
            if (!TryGetType(id, out var type)) return false;
            variation = (VariationModel) Activator.CreateInstance(type);
            return true;
        }

        public bool TryGet(string name, out VariationModel variation)
        {
            variation = null;
            if (!TryGetType(name, out var type)) return false;
            variation = (VariationModel) Activator.CreateInstance(type);
            return true;
        }

        public bool TryGetParameters(string name, out string[] parameterNames, out double[] parameters)
        {
            parameterNames = null;
            parameters = null;
            if (!_parametersNames.ContainsKey(name)) return false;
            parameterNames = _parametersNames[name].ToArray();
            parameters = _parameters[name].ToArray();
            return true;
        }

        private bool TryGetType(int id, out Type type)
        {
            type = null;
            if (!_ids.ContainsKey(id)) return false;
            type = _types[_ids[id]];
            return true;
        }

        private bool TryGetType(string name, out Type type)
        {
            type = null;
            if (!_names.ContainsKey(name)) return false;
            type = _types[_names[name]];
            return true;
        }
    }
}