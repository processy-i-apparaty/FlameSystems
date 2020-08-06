using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace FlameBase.Models
{
    public class RenderActionsModel
    {
        public RenderActionsModel(Action<BitmapSource> actionDraw, Action<string> actionMessage,
            Action<string> actionRenderState)
        {
            ActionDraw = actionDraw;
            ActionMessage = actionMessage;
            ActionRenderState = actionRenderState;
        }

        public Action<BitmapSource> ActionDraw { get; }
        public Action<string> ActionMessage { get; }
        public Action<string> ActionRenderState { get; }
    }
}
