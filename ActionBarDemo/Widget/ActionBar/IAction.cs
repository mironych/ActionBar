namespace ActionBarDemo.Widget.ActionBar
{
    public interface IAction
    {
        int Drawable { get; }
        int DrawableDisabled { get; }
        void PerformAction();
    }
}