using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BlueDog.Models;
using BlueDog.Services;
using Newtonsoft.Json;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.Exceptions;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BlueDog.Views
{
    public partial class AboutPage : ContentPage
    {

        public AboutPage()
        {
            InitializeComponent();
        }

        async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            if (sender is Button)
            {
                Button sent = (Button)sender;
                sent.IsVisible = false;
            }

            Bluetooth scanner = DependencyService.Get<Bluetooth>();

            await scanner.ScanForDevices();

            if (sender is Button)
            {
                Button sent = (Button)sender;
                sent.IsVisible = true;
            }

        }

    }
}
