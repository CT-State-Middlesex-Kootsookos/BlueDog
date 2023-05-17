using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using BlueDog.Models;
using BlueDog.ViewModels;

namespace BlueDog.Views
{
    public partial class NewItemPage : ContentPage
    {
        public RobotInfo Item { get; set; }

        public NewItemPage()
        {
            InitializeComponent();
            BindingContext = new NewItemViewModel();
        }
    }
}
