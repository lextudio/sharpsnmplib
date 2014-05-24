using System;
using Android.App;
using Android.Widget;
using Android.OS;
using Lextm.SharpSnmpLib;
using System.Collections.Generic;
using System.Net;

namespace SharpSnmpLib.Android.Sample
{
    [Activity (Label = "SharpSnmpLib.Android.Sample", MainLauncher = true)]
	public class MainActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
			Button button = FindViewById<Button> (Resource.Id.myButton);
			
			button.Click += delegate {
                var resultBox = FindViewById<TextView>(Resource.Id.textView2);
                IPAddress address;
                if (IPAddress.TryParse(FindViewById<TextView>(Resource.Id.textView1).Text, out address))
                {
                    try
                    {
                        var result = Lextm.SharpSnmpLib.Messaging.Messenger.Get(VersionCode.V1,
                            new IPEndPoint(address, 161),
                            new OctetString("public"),
                            new List<Variable> { new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.1.0")) },
							10000);
                        resultBox.Text = result[0].Data.ToString();
                    }
                    catch (Exception ex)
                    {
                        resultBox.Text = ex.ToString();
                    }
                }
                else
                {
                    resultBox.Text = "Please provide a valid IP address";
                }
            };
		}
	}
}


