using System;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Util;
using Android.Widget;

namespace ActionBarDemo.Widget
{
    public class ScrollingTextView : TextView
    {
        public ScrollingTextView(IntPtr doNotUse, JniHandleOwnership transfer) : base(doNotUse, transfer)
        {
        }

        public ScrollingTextView(Context context)
            : base(context)
        {
        }

        public ScrollingTextView(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
        }

        public ScrollingTextView(Context context, IAttributeSet attrs, int defStyle)
            : base(context, attrs, defStyle)
        {
        }

        protected override void OnFocusChanged(bool gainFocus, Android.Views.FocusSearchDirection direction, Rect previouslyFocusedRect)
        {
            if (gainFocus)
            {
                base.OnFocusChanged(true, direction, previouslyFocusedRect);
            }
        }

        public override void OnWindowFocusChanged(bool focused)
        {
            if (focused)
            {
                base.OnWindowFocusChanged(true);
            }
        }

        public override bool HasFocus
        {
            get
            {
                return true;
            }
        }
    }
}