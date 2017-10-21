using Android.App;
using Android.Widget;
using Android.OS;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using GymProgUI;
using GymProgFramework.LocalStorage;

namespace GymProg
{
    [Activity(Label = "GymProg", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            // SetContentView (Resource.Layout.Main);

            FileReader.Instance = new AndroidFileHandler();

            Forms.Init(this, bundle);
            LoadApplication(new App());
        }

    }
}

