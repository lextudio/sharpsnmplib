using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Net;
using Lextm.SharpSnmpLib;
using System.Collections.Generic;

namespace SharpSnmpLib.iOS.Sample
{
	public partial class SharpSnmpLib_iOS_SampleViewController : UIViewController
	{
		public SharpSnmpLib_iOS_SampleViewController (IntPtr handle) : base (handle)
		{
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		#region View lifecycle

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			// Perform any additional setup after loading the view, typically from a nib.
			myButton.TouchUpInside += (o, e) => {
				var resultBox = tvResult;
				IPAddress address;
				if (IPAddress.TryParse(tfInput.Text, out address))
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

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
		}

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
		}

		public override void ViewDidDisappear (bool animated)
		{
			base.ViewDidDisappear (animated);
		}

		#endregion
	}
}

