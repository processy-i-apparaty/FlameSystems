using System.Collections.Generic;
using System.Collections.ObjectModel;
using FlameBase.Models;
using FlameSystems.Infrastructure;

namespace FlameSystems.Connectors.Base
{
    public class PanelModel
    {
        public List<TransformModel> _transforms;

        public PanelModel(ObservableCollection<PanelTransform> transforms, PanelTransform final)
        {
            _transforms = new List<TransformModel>();
            foreach (var panelTransform in transforms)
            {
                GetTransform(panelTransform);
            }
        }

        public PanelModel()
        {
        }

        private void GetTransform(PanelTransform panelTransform)
        {
            var dc = (PanelTransformViewModel) panelTransform.DataContext;

            var tm = new TransformModel
            {
                
            };

        }

        public int Id { get; set; }
    }
}