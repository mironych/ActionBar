using Android.Content;
using Android.Widget;

namespace ActionBarDemo.Widget.ActionBar
{
    public class IntentAction : BaseAction
    {
        private readonly Context _context;
        private readonly Intent _intent;

        public IntentAction(Context context, Intent intent, int drawable)
            : base(drawable)
        {
            _context = context;
            _intent = intent;
        }

        public IntentAction(Context context, Intent intent, int drawable, int drawableDisabled)
            : base(drawable, drawableDisabled)
        {
            _context = context;
            _intent = intent;
        }

        public override void PerformAction()
        {
            try
            {
                _context.StartActivity(_intent);
            }
            catch (ActivityNotFoundException)
            {
                Toast.MakeText(_context,
                        _context.GetText(Resource.String.actionbar_activity_not_found),
                        ToastLength.Short).Show();
            }
        }
    }
}