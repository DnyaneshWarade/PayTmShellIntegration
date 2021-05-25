using AllInOneSDK;
using PaytmShellSample.Models;
using PaytmShellSample.PaymentGateway;
using PaytmShellSample.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PaytmShellSample.Views
{
	public partial class NewItemPage : ContentPage, PaymentCallback
	{
		public Item Item { get; set; }

		public NewItemPage()
		{
			InitializeComponent();
			BindingContext = new NewItemViewModel();
		}

        void Button_Clicked(System.Object sender, System.EventArgs e)
        {
			string amount = "1.0";
			Random _random = new Random();
			string OrderId = _random.Next().ToString();
			var payTMTuple = PayTmPayment.GenerateTransactionToken(amount, "1111111111", OrderId, "WEBSTAGING", "Payment");

			if (payTMTuple.Item1 == PaytmErrors.SUCCESS)
			{
				if (!payTMTuple.Item2.Equals(""))
				{
					//go ahead with the transaction

					var TransactionId = payTMTuple.Item2; //storing the transcationId.

					AllInOnePlugin.startTransaction(OrderId, "qFKjOk51773158003561", payTMTuple.Item2, amount, "https://securegw-stage.paytm.in/theia/paytmCallback?ORDER_ID=" + OrderId, true, true, this);
				}
			}
		}

		public void success(Dictionary<string, object> dictionary)
		{
			AllInOnePlugin.DestroyInstance();
		}

		public void error(string errorMessage)
		{
			AllInOnePlugin.DestroyInstance();
		}
	}
}