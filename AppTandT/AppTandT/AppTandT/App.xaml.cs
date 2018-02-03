using FFImageLoading.Forms;
using AppTandT.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamvvm;
using AppTandT.BLL;
using AppTandT.Pages.UserPages;
using AppTandT.Pages.Menu;
//using TandT.XBLL;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace TandT_App
{
    public partial class App : Application
    {
        static public double ScreenWidth;
        static public double ScreenHeight;
        static public double NavigationBarHeight;
        static public double StatusBarHeight;
        static public double Density;
        public static bool isUWP;

        public App()
        {
            InitializeComponent();
            App.Current.Resources = new ResourceDictionary()
           {
              { "CustomCacheKeyFactory", new CustomCacheKeyFactory() }
           };

            CachedImage.FixedOnMeasureBehavior = true;
            CachedImage.FixedAndroidMotionEventHandler = true;

            //init Horizontal List View
          
            // Xamvvm init
            var factory = new XamvvmFormsFactory(this);
            if (Dekanat.isLogined())
                factory.RegisterNavigationPage<MainNavigationPageModel>(() => this.GetPageFromCache<MainMasterDetailPageModel>());
            else
                factory.RegisterNavigationPage<MainNavigationPageModel>(() => this.GetPageFromCache<LoginPageModel>());

            XamvvmCore.SetCurrentFactory(factory);
            MainPage = this.GetPageFromCache<MainNavigationPageModel>() as NavigationPage;
        }

        protected override void OnStart()
        {
            // Handle when your app starts  
        }
        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }
        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        public static bool isConnected()
        {
            ///!!!
            return true;
        }
    }

    class CustomCacheKeyFactory : ICacheKeyFactory
    {
        public string GetKey(ImageSource imageSource, object bindingContext)
        {
            return "StreamTestCustomKey";
        }
    }

}

