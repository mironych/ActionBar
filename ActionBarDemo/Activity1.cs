using ActionBarDemo.Widget.ActionBar;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace ActionBarDemo
{
    [Activity(Label = "ActionBar", MainLauncher = true, Icon = "@drawable/icon")]
    public class Activity1 : ActionBarActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.Main);
            InitializeActionBar();
            var mailIntent = new Intent(Intent.ActionSend);
            mailIntent.PutExtra(Intent.ExtraEmail, "test@mail.com");
            mailIntent.PutExtra(Intent.ExtraSubject, "Subject");
            mailIntent.PutExtra(Intent.ExtraText, "Body");
            mailIntent.SetType("text/plain");
            var intent = Intent.CreateChooser(mailIntent, "Send mail");
            ActionBar.AddAction(new IntentAction(this, intent, Resource.Drawable.ic_action_message)).AddStubAction()
                .AddAction(new DeleagetAction(OpenCameraClick, Android.Resource.Drawable.IcMenuCamera))
                .AddLeftAction(new DeleagetAction(Finish, Resource.Drawable.ic_action_back))
                .SetTitle("ActionBarTest");
        }

        private void OpenCameraClick()
        {
            Toast.MakeText(this, "Camera button clicked", ToastLength.Short).Show();
        }
    }
}

