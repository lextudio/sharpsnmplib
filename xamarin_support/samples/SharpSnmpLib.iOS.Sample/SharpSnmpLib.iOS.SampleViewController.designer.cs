// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.CodeDom.Compiler;

namespace SharpSnmpLib.iOS.Sample
{
	[Register ("SharpSnmpLib_iOS_SampleViewController")]
	partial class SharpSnmpLib_iOS_SampleViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton myButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField tfInput { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextView tvResult { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (myButton != null) {
				myButton.Dispose ();
				myButton = null;
			}
			if (tfInput != null) {
				tfInput.Dispose ();
				tfInput = null;
			}
			if (tvResult != null) {
				tvResult.Dispose ();
				tvResult = null;
			}
		}
	}
}
