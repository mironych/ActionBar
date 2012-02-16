using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace ActionBarDemo.Widget.ActionBar
{
    public class ActionBar : RelativeLayout, View.IOnClickListener
    {
        private LayoutInflater _mInflater;
        private RelativeLayout _mBarView;
        private ImageView _mLogoView;
        private View _mBackIndicator;
        private TextView _mTitleView;
        private LinearLayout _mActionsView;
        private LinearLayout _mLeftActionsView;
        private ImageButton _mHomeBtn;
        private RelativeLayout _mHomeLayout;
        private ProgressBar _mProgress;
        private readonly IDictionary<Guid, IAction> _actionsList = new Dictionary<Guid, IAction>();

        public int ActionCount
        {
            get { return _mActionsView.ChildCount; }
        }

        public bool ProgressBarIsVisible { get; private set; }

        public ActionBar(IntPtr doNotUse, JniHandleOwnership transfer)
            : base(doNotUse, transfer)
        {
        }

        public ActionBar(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
            InternalCtor(context, attrs);
        }

        public ActionBar(Context context, IAttributeSet attrs, int defStyle)
            : base(context, attrs, defStyle)
        {
            InternalCtor(context, attrs);
        }

        ~ActionBar()
        {
            Dispose(false);
        }

        private void InternalCtor(Context context, IAttributeSet attrs)
        {
            _mInflater = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService);
            _mBarView = (RelativeLayout)_mInflater.Inflate(Resource.Layout.actionbar, null);
            AddView(_mBarView);
            _mLogoView = (ImageView)_mBarView.FindViewById(Resource.Id.actionbar_home_logo);
            _mHomeLayout = (RelativeLayout)_mBarView.FindViewById(Resource.Id.actionbar_home_bg);
            _mHomeBtn = (ImageButton)_mBarView.FindViewById(Resource.Id.actionbar_home_btn);
            _mBackIndicator = _mBarView.FindViewById(Resource.Id.actionbar_home_is_back);
            _mTitleView = (TextView)_mBarView.FindViewById(Resource.Id.actionbar_title);
            _mTitleView.TextSize = 18;
            _mActionsView = (LinearLayout)_mBarView.FindViewById(Resource.Id.actionbar_actions);
            _mLeftActionsView = (LinearLayout)_mBarView.FindViewById(Resource.Id.actionbar_left_actions);
            _mProgress = (ProgressBar)_mBarView.FindViewById(Resource.Id.actionbar_progress);
            var a = context.ObtainStyledAttributes(attrs, Resource.Styleable.ActionBar);
            var title = a.GetString(Resource.Styleable.ActionBar_title);
            if (!string.IsNullOrEmpty(title))
            {
                SetTitle(title);
            }
            a.Recycle();
        }


        public ActionBar SetHomeAction(IAction action)
        {
            _mHomeBtn.SetOnClickListener(this);
            _mHomeBtn.Tag = (Java.Lang.Object)action;
            _mHomeBtn.SetImageResource(action.Drawable);
            _mHomeLayout.Visibility = ViewStates.Visible;
            return this;
        }

        public void ClearHomeAction()
        {
            _mHomeLayout.Visibility = ViewStates.Gone;
        }

        public ActionBar SetHomeLogo(int resId)
        {
            _mLogoView.SetImageResource(resId);
            _mLogoView.Visibility = ViewStates.Visible;
            _mHomeLayout.Visibility = ViewStates.Gone;
            return this;
        }

        public ActionBar SetHomeLogo(IAction action)
        {
            if (action == null)
                throw new ArgumentNullException("action");
            _mLogoView.SetImageResource(action.Drawable);
            _mLogoView.Visibility = ViewStates.Visible;
            _mLogoView.Click += delegate
                                  {
                                      action.PerformAction();
                                  };
            _mHomeLayout.Visibility = ViewStates.Gone;
            return this;
        }

        public ActionBar SetDisplayHomeAsUpEnabled(bool show)
        {
            _mBackIndicator.Visibility = show ? ViewStates.Visible : ViewStates.Gone;
            return this;
        }

        public ActionBar SetTitle(string title)
        {
            _mTitleView.Text = title;
            return this;
        }

        public ActionBar SetTitle(int resid)
        {
            _mTitleView.SetText(resid);
            return this;
        }

        public void ShowProgressBar()
        {
            _mProgress.Visibility = ViewStates.Visible;
            ProgressBarIsVisible = true;
        }

        public void HideProgressBar()
        {
            _mProgress.Visibility = ViewStates.Gone;
            ProgressBarIsVisible = false;
        }

        public ActionBar SetOnTitleClickListener(IOnClickListener listener)
        {
            _mTitleView.SetOnClickListener(listener);
            return this;
        }

        public ActionBar AddActions(List<IAction> actionList)
        {
            foreach (var action in actionList)
            {
                AddAction(action);
            }
            return this;
        }

        public ActionBar AddAction(IAction action, bool enabled = true)
        {
            var index = _mActionsView.ChildCount;
            return AddAction(action, index, enabled);
        }

        public ActionBar AddAction(IAction action, int index, bool enabled)
        {
            _mActionsView.AddView(InflateAction(action, enabled, _mActionsView), index);
            return this;
        }

        public ActionBar AddStubAction()
        {
            var view = _mInflater.Inflate(Resource.Layout.actionbar_item, _mActionsView, false);
            view.Enabled = false;
            _mActionsView.AddView(view);
            return this;
        }

        public ActionBar AddLeftAction(IAction action, bool enabled = true)
        {
            var index = _mLeftActionsView.ChildCount;
            return AddLeftAction(action, index, enabled);
        }

        public ActionBar AddLeftAction(IAction action, int index, bool enabled)
        {
            _mLeftActionsView.AddView(InflateAction(action, enabled, _mLeftActionsView), index);
            return this;
        }

        public ActionBar DisableAction(IAction action)
        {
            SetActionEnabled(action, false);
            return this;
        }

        public ActionBar EnableAction(IAction action)
        {
            SetActionEnabled(action, true);
            return this;
        }

        private void SetActionEnabled(IAction action, bool enabled)
        {
            var actionID = _actionsList.Where(k => k.Value.Equals(action)).Select(k => k.Key).SingleOrDefault();
            if (actionID == Guid.Empty) return;

            var childCount = _mActionsView.ChildCount;
            for (var i = 0; i < childCount; i++)
            {
                var view = _mActionsView.GetChildAt(i);
                if (view == null) continue;
                var strTag = view.Tag == null ? string.Empty : view.Tag.ToString();
                Guid tag;
                if (!Guid.TryParse(strTag, out tag)) continue;
                if (tag != actionID) continue;
                view.Enabled = enabled;
                var labelView = view.FindViewById<ImageButton>(Resource.Id.actionbar_item);
                labelView.SetImageResource(enabled ? action.Drawable : action.DrawableDisabled == -1 ? action.Drawable : action.DrawableDisabled);
                return;
            }
        }

        public void RemoveAllActions()
        {
            _mActionsView.RemoveAllViews();
            _actionsList.Clear();
        }

        public void RemoveActionAt(int index)
        {
            var view = _mActionsView.GetChildAt(index);
            if (view == null) return;
            var strTag = view.Tag == null ? string.Empty : view.Tag.ToString();
            Guid tag;
            if (!Guid.TryParse(strTag, out tag)) return;
            if (!_actionsList.ContainsKey(tag)) return;

            _actionsList.Remove(tag);
            _mActionsView.RemoveViewAt(index);
            return;
        }

        public void RemoveAction(IAction action)
        {
            var actionID = _actionsList.Where(k => k.Value.Equals(action)).Select(k => k.Key).SingleOrDefault();
            if (actionID == Guid.Empty) return;

            var childCount = _mActionsView.ChildCount;
            for (var i = 0; i < childCount; i++)
            {
                var view = _mActionsView.GetChildAt(i);
                if (view == null) continue;
                var strTag = view.Tag == null ? string.Empty : view.Tag.ToString();
                Guid tag;
                if (!Guid.TryParse(strTag, out tag)) continue;
                if (tag != actionID) continue;
                _mActionsView.RemoveViewAt(i);
                _actionsList.Remove(tag);
                return;
            }
        }

        private View InflateAction(IAction action, bool enabled, ViewGroup viewGroup)
        {
            var view = _mInflater.Inflate(Resource.Layout.actionbar_item, viewGroup, false);
            var labelView = view.FindViewById<ImageButton>(Resource.Id.actionbar_item);
            labelView.SetImageResource(enabled ? action.Drawable : action.DrawableDisabled == -1 ? action.Drawable : action.DrawableDisabled);
            var key = Guid.NewGuid();
            view.Tag = key.ToString();
            view.Enabled = enabled;
            view.SetOnClickListener(this);
            _actionsList.Add(key, action);
            return view;
        }

        public void OnClick(View view)
        {
            var strTag = view.Tag == null ? string.Empty : view.Tag.ToString();
            Guid tag;
            if (!Guid.TryParse(strTag, out tag)) return;

            if (_actionsList.ContainsKey(tag))
            {
                _actionsList[tag].PerformAction();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (_mActionsView != null)
                _mActionsView.Dispose();
            if (_mBackIndicator != null)
                _mBackIndicator.Dispose();
            if (_mBarView != null)
                _mBarView.Dispose();
            if (_mHomeBtn != null)
                _mHomeBtn.Dispose();
            if (_mHomeLayout != null)
                _mHomeLayout.Dispose();
            if (_mInflater != null)
                _mInflater.Dispose();
            if (_mLogoView != null)
                _mLogoView.Dispose();
            if (_mProgress != null)
                _mProgress.Dispose();
            if (_mTitleView != null)
                _mTitleView.Dispose();
            if (disposing)
                GC.SuppressFinalize(this);
            base.Dispose(disposing);
        }
    }
}