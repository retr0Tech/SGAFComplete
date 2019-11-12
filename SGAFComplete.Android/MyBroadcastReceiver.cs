using System;
using Com.Cipherlab.Rfidapi;
using Android.App;
using Android.Runtime;
using Android.OS;
using Android.Content;
using Com.Cipherlab.Rfid;
using Android.Widget;
using Xamarin.Forms.Internals;
using Xamarin.Forms;

namespace SGAFComplete.Droid
{
	public class MyBroadcastReceiver : BroadcastReceiver
	{
		public override void OnReceive(Context context, Intent intent)
		{
			if (intent.Action.Equals(GeneralString.IntentRFIDSERVICECONNECTED))
			{
				string PackageName = intent.GetStringExtra("PackageName");

				//Revisa que la app esta conextada con el RFID
				string ver = "";
				ver = MainActivity.mRfidManager.APIVersion;

				Toast.MakeText(context, "Gun Service Connected", ToastLength.Short).Show();
			}
			else if (intent.Action.Equals(GeneralString.IntentRFIDSERVICETAGDATA))
			{
				/* 
				 * type : 0=Normal scan (Press Trigger Key to receive the data) ; 1=Inventory EPC ; 2=Inventory ECP TID ; 3=Reader tag ; 5=Write tag ; 6=Lock tag ; 7=Kill tag ; 8=Authenticate tag ; 9=Untraceable tag
				 * response : 0=RESPONSE_OPERATION_SUCCESS ; 1=RESPONSE_OPERATION_FINISH ; 2=RESPONSE_OPERATION_TIMEOUT_FAIL ; 6=RESPONSE_PASSWORD_FAIL ; 7=RESPONSE_OPERATION_FAIL ;251=DEVICE_BUSY
				 * */
				int type = intent.GetIntExtra(GeneralString.ExtraDataType, -1);
				int response = intent.GetIntExtra(GeneralString.ExtraResponse, -1);
				double data_rssi = intent.GetDoubleExtra(GeneralString.ExtraDataRssi, 0);

				String PC = intent.GetStringExtra(GeneralString.ExtraPc);
				String EPC = intent.GetStringExtra(GeneralString.ExtraEpc);
				String TID = intent.GetStringExtra(GeneralString.ExtraTid);
				String ReadData = intent.GetStringExtra(GeneralString.EXTRAReadData);
				int EPC_length = intent.GetIntExtra(GeneralString.ExtraEpcLength, 0);
				int TID_length = intent.GetIntExtra(GeneralString.ExtraTidLength, 0);
				int ReadData_length = intent.GetIntExtra(GeneralString.EXTRAReadDataLENGTH, 0);

				string Data = "response = " + response + " , EPC = " + EPC + "\r TID = " + TID;
				MessagingCenter.Send<MyBroadcastReceiver, string>(this, "Code", EPC);
			}
			else if (intent.Action.Equals(GeneralString.IntentGUNAttached))
			{
				Toast.MakeText(context, "Pistola Rfid Conectada", ToastLength.Short).Show();
			}
			else if (intent.Action.Equals(GeneralString.IntentGUNUnattached))
			{
				Toast.MakeText(context, "Pistola Rfid Desconectada", ToastLength.Short).Show();
			}
		}
	}
}
