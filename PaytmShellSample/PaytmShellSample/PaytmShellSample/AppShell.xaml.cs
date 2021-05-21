using PaytmShellSample.ViewModels;
using PaytmShellSample.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace PaytmShellSample
{
	public partial class AppShell : Xamarin.Forms.Shell
	{
		public AppShell()
		{
			InitializeComponent();
			Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
			Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
		}

	}
}
