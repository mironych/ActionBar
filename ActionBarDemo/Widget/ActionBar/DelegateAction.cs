using System;

namespace ActionBarDemo.Widget.ActionBar
{
    public class DeleagetAction : BaseAction
    {
        private readonly Action _action;

        public DeleagetAction(Action action, int drawable)
            : base(drawable)
        {
            _action = action;
        }

        public DeleagetAction(Action action, int drawable, int drawableDisabled = -1)
            : base(drawable, drawableDisabled)
        {
            _action = action;
        }

        public override void PerformAction()
        {
            if (_action != null)
                _action();
        }
    }
}