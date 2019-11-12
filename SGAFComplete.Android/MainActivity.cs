using System;
using Com.Cipherlab.Rfidapi;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Com.Cipherlab.Rfid;
using Android.Content;

namespace SGAFComplete.Droid
{
    [Activity(Label = "SGAFComplete", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        IntentFilter filter;
        public static RfidManager mRfidManager;

        MyBroadcastReceiver MyDataReceiver;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            mRfidManager = RfidManager.InitInstance(this);
            //RFID Code
            MyDataReceiver = new MyBroadcastReceiver();
            filter = new IntentFilter();
            filter.AddAction(GeneralString.IntentRFIDSERVICECONNECTED);
            filter.AddAction(GeneralString.IntentRFIDSERVICETAGDATA);
            //filter.AddAction(GeneralString.IntentRFIDSERVICEEVENT);
            //filter.AddAction(GeneralString.IntentFWUpdateErrorMessage);
            //filter.AddAction(GeneralString.IntentFWUpdatePercent);
            //filter.AddAction(GeneralString.IntentFWUpdateFinish);
            filter.AddAction(GeneralString.IntentGUNAttached);
            filter.AddAction(GeneralString.IntentGUNAttached);
            //filter.AddAction(GeneralString.IntentGUNPower);

            //RegisterReceiver(MyDataReceiver, filter);
            //End RFID Code
            //mRfidManager.Release();
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            Context context = Android.App.Application.Context;
            LoadApplication(new App(context));
        }
        protected override void OnResume()
        {
            base.OnResume();
            RegisterReceiver(MyDataReceiver, filter);
        }
        protected override void OnPause()
        {
            base.OnPause();
            UnregisterReceiver(MyDataReceiver);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}