//using Acr.UserDialogs;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Provider;
using FFImageLoading;
using FFImageLoading.Forms.Droid;
using Java.IO;
using Java.Lang;
using Plugin.Permissions;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Plugin.Permissions.Abstractions;

namespace AppTandT.Droid
{
    [Activity(Label = "AppTandT", Icon = "@drawable/logo", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            Xamarin.Forms.Forms.SetFlags("FastRenderers_Experimental");
            CachedImageRenderer.Init(true);

            var config = new FFImageLoading.Config.Configuration()
            {
                VerboseLogging = false,
                VerbosePerformanceLogging = false,
                VerboseMemoryCacheLogging = false,
                VerboseLoadingCancelledLogging = false,
                Logger = new CustomLogger(),
            };

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());
        }

        public event EventHandler<ActivityResultEventArgs> ActivityResult;

        public class CustomLogger : FFImageLoading.Helpers.IMiniLogger
        {
            public void Debug(string message)
            {
                System.Console.WriteLine(message);
            }

            public void Error(string errorMessage)
            {
                System.Console.WriteLine(errorMessage);
            }

            public void Error(string errorMessage, System.Exception ex)
            {
                Error(errorMessage + System.Environment.NewLine + ex.ToString());
            }
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if (resultCode == Result.Ok)
            {
                if (ActivityResult != null)
                    ActivityResult(this, new ActivityResultEventArgs { Intent = data });

            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }

    public class ActivityResultEventArgs : EventArgs
    {

        public Intent Intent
        {
            get;
            set;
        }
    }
}

