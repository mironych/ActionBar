using ActionBarDemo.Widget.ActionBar;
using Android.App;

namespace ActionBarDemo
{
    public class ActionBarActivity : Activity
    {
        protected ActionBar ActionBar { get; private set; }

        protected void InitializeActionBar()
        {
            ActionBar = FindViewById<ActionBar>(Resource.Id.actionbar);
        }
    }
}