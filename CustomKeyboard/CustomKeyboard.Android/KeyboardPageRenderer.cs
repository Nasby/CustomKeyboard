using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.App;
using Android.Hardware;
using Android.Views;
using Android.Graphics;
using Android.InputMethodServices;
using Android.Widget;
using Android.Views.Animations;
using Java.Lang;
using CustomKeyboard;
using CustomKeyboard.Droid;
using Android.Runtime;
using System.ComponentModel;
using System.Threading.Tasks;
using Android.Views.InputMethods;

[assembly: ExportRenderer(typeof(KeyboardPage), typeof(KeyboardPageRenderer))]
namespace CustomKeyboard.Droid
{
    class KeyboardPageRenderer : PageRenderer
    {

        public CustomKeyboardView mKeyboardView;
        public EditText mTargetView;
        public Android.InputMethodServices.Keyboard mKeyboard;
        Activity activity;
        global::Android.Views.View view;

        protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || Element == null)
            {
                return;
            }

            try
            {
                SetupUserInterface();
                SetupEventHandlers();
                this.AddView(view);
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(@"			ERROR: ", ex.Message);
            }
        }

        void SetupUserInterface()
        {
            activity = this.Context as Activity;
            view = activity.LayoutInflater.Inflate(Resource.Layout.Main, this, false);
            
            mKeyboard = new Android.InputMethodServices.Keyboard(Context, Resource.Xml.keyboard);
            mTargetView = view.FindViewById<EditText>(Resource.Id.target);
            
            mKeyboardView = view.FindViewById<CustomKeyboardView>(Resource.Id.keyboard_view);
            mKeyboardView.Keyboard = mKeyboard;
        }
        
        void SetupEventHandlers()
        {
            mTargetView.Touch += (sender, e) =>
            {
                ShowKeyboardWithAnimation();
                e.Handled = false;
                mTargetView.ShowSoftInputOnFocus = false;
            };

            mTargetView.FocusChange += (sender, e) =>
            {
                var idk = FindFocus();
                if (!mTargetView.IsFocused)
                {
                    mKeyboardView.Visibility = ViewStates.Gone;
                }

            };

            mKeyboardView.Key += (sender, e) =>
            {
                long eventTime = JavaSystem.CurrentTimeMillis();
                KeyEvent ev = new KeyEvent(eventTime, eventTime, KeyEventActions.Down, e.PrimaryCode, 0, 0, 0, 0, KeyEventFlags.SoftKeyboard | KeyEventFlags.KeepTouchMode);

                DispatchKeyEvent(ev);
            };
        }
        

        public void ShowKeyboardWithAnimation()
        {
            if (mKeyboardView.Visibility == ViewStates.Gone)
            {
                mKeyboardView.Visibility = ViewStates.Visible;
                Android.Views.Animations.Animation animation = AnimationUtils.LoadAnimation(
                    Context,
                    Resource.Animation.slide_up_bottom
                );
                mKeyboardView.ShowWithAnimation(animation);
            }
        }

        protected override void OnLayout (bool changed, int l, int t, int r, int b)
		{
			base.OnLayout (changed, l, t, r, b);

			var msw = MeasureSpec.MakeMeasureSpec (r - l, MeasureSpecMode.Exactly);
			var msh = MeasureSpec.MakeMeasureSpec (b - t, MeasureSpecMode.Exactly);

			view.Measure (msw, msh);
			view.Layout (0, 0, r - l, b - t);
		}
    }

}