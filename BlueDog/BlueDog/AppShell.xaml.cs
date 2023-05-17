using System;
using System.Collections.Generic;
using BlueDog.ViewModels;
using BlueDog.Views;
using Xamarin.Forms;

namespace BlueDog
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

