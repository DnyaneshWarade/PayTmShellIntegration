﻿using PaytmShellSample.Models;
using PaytmShellSample.ViewModels;
using PaytmShellSample.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PaytmShellSample.Views
{
	public partial class ItemsPage : ContentPage
	{
		ItemsViewModel _viewModel;

		public ItemsPage()
		{
			InitializeComponent();

			BindingContext = _viewModel = new ItemsViewModel();
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			_viewModel.OnAppearing();
		}

        void ToolbarItem_Clicked(System.Object sender, System.EventArgs e)
        {
			Navigation.PushModalAsync(new NavigationPage(new NewItemPage()));
        }
    }
}