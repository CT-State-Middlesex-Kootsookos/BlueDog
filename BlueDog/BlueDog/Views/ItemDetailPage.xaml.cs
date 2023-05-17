using System.ComponentModel;
using Xamarin.Forms;
using BlueDog.ViewModels;

namespace BlueDog.Views
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
