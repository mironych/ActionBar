namespace ActionBarDemo.Widget.ActionBar
{
    public abstract class BaseAction : IAction
    {
        protected BaseAction(int drawable, int drawableDisabled = -1)
        {
            Drawable = drawable;
            DrawableDisabled = drawableDisabled;
        }

        public int Drawable { get; private set; }
        public int DrawableDisabled { get; private set; }
        public abstract void PerformAction();
    }
}