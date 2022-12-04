using Android.App;
using Android.Content.PM;
using Android.OS;
using Xamarin.Forms;
using EpdApp.Services.XmlsService;
using EpdApp.Services;

namespace EpdApp.Droid
{
    [Activity(Label = "EpdApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        internal static MainActivity Instance { get; private set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //UserDialogs.Init(this);
            var service = new XmlService();
            DependencyService.RegisterSingleton<IXmlService>(service);
            DependencyService.RegisterSingleton<IBluetoothService>(new Bluetooth.BluetoothService());

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            Instance = this;
            LoadApplication(new App());
        }
    }
}