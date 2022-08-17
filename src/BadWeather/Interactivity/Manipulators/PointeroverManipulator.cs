﻿using InteractiveGeometry;
using InteractiveGeometry.UI;
using InteractiveGeometry.UI.Input;
using InteractiveGeometry.UI.Input.Core;

namespace BadWeather.Manipulators
{
    internal class PointeroverManipulator : MouseManipulator
    {
        private bool _isChecker = false;
        private IInteractiveBehavior _behavior;

        public PointeroverManipulator(IView view, IInteractiveBehavior behavior) : base(view)
        {
            _behavior = behavior;
        }

        public override void Delta(MouseEventArgs e)
        {
            base.Delta(e);

            if (e.Handled == false)
            {
                var mapInfo = e.MapInfo;

                if (mapInfo != null && mapInfo.Layer != null)
                {
                    if (_isChecker == true)
                    {
                        View.SetCursor(CursorType.Hand);

                        _behavior.OnHoverStart(mapInfo);

                        _isChecker = false;
                    }

                    e.Handled = false;// true;
                }
                else
                {
                    if (_isChecker == false)
                    {
                        View.SetCursor(CursorType.Default);

                        _behavior.OnHoverStop(mapInfo);

                        _isChecker = true;
                    }
                }
            }
        }
    }
}