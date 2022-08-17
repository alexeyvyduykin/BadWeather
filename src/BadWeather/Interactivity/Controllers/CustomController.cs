using BadWeather.Manipulators;
using InteractiveGeometry;
using InteractiveGeometry.UI;
using InteractiveGeometry.UI.Input;
using InteractiveGeometry.UI.Input.Core;

namespace BadWeather.Controllers
{
    public class CustomController : ControllerBase, IController
    {
        public CustomController(IInteractive interactive)
        {
            var behavior = new InteractiveBehavior(interactive);

            var click = new DelegateMapCommand<MouseDownEventArgs>(
                (view, controller, args) => controller.AddMouseManipulator(view, new ClickManipulator(view, behavior), args));

            var hover = new DelegateMapCommand<MouseEventArgs>(
                (view, controller, args) => controller.AddHoverManipulator(view, new PointeroverManipulator(view, behavior), args));

            this.BindMouseDown(MouseButton.Left, click);
            this.BindMouseEnter(hover);
        }
    }
}
