using PaytmShellSample.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace PaytmShellSample.Views
{
	public partial class ItemDetailPage : ContentPage
	{
		public ItemDetailPage()
		{
			InitializeComponent();
			BindingContext = new ItemDetailViewModel();
		}
	}
}