using InteractiveGeometry;
using InteractiveGeometry.UI;
using InteractiveGeometry.UI.Input;
using InteractiveGeometry.UI.Input.Core;

namespace BadWeather.Manipulators
{
    internal class ClickManipulator : MouseManipulator
    {
        private IInteractiveBehavior _behavior;

        public ClickManipulator(IView view, IInteractiveBehavior behavior) : base(view)
        {
            _behavior = behavior;
        }

        public override void Completed(MouseEventArgs e)
        {
            base.Completed(e);

            if (e.Handled == false)
            {
                var mapInfo = e.MapInfo;

                if (mapInfo != null &&
                    mapInfo.Layer != null &&
                    mapInfo.Feature != null)
                {
                    _behavior.OnCompleted(mapInfo);
                }
            }

            e.Handled = true;
        }
    }
}
