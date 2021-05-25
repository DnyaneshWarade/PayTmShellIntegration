using PaytmShellSample.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using AllInOneSDK;
using PaytmShellSample.PaymentGateway;

namespace PaytmShellSample.ViewModels
{
	public class NewItemViewModel : BaseViewModel, PaymentCallback
	{
		private string text;
		private string description;

		public NewItemViewModel()
		{
			SaveCommand = new Command(OnSave, ValidateSave);
			CancelCommand = new Command(OnCancel);
			PayCommand = new Command(OnPay);
			this.PropertyChanged +=
				(_, __) => SaveCommand.ChangeCanExecute();
		}

		private bool ValidateSave()
		{
			return !String.IsNullOrWhiteSpace(text)
				&& !String.IsNullOrWhiteSpace(description);
		}

		public string Text
		{
			get => text;
			set => SetProperty(ref text, value);
		}

		public string Description
		{
			get => description;
			set => SetProperty(ref description, value);
		}

		public Command SaveCommand { get; }
		public Command CancelCommand { get; }
		public Command PayCommand { get; }
		private async void OnCancel()
		{
			// This will pop the current page off the navigation stack
			await Shell.Current.GoToAsync("..");
		}

		private async void OnPay()
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

		private async void OnSave()
		{
			Item newItem = new Item()
			{
				Id = Guid.NewGuid().ToString(),
				Text = Text,
				Description = Description
			};

			await DataStore.AddItemAsync(newItem);

			// This will pop the current page off the navigation stack
			await Shell.Current.GoToAsync("..");
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
