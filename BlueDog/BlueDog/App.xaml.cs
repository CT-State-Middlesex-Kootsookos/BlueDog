using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using BlueDog.Services;
using BlueDog.Views;
using Plugin.BLE;
using System.Collections.Generic;

namespace BlueDog
{
    public partial class App : Application
    {

        public App ()
        {
            InitializeComponent();

            DependencyService.Register<RobotDataStore>();
            DependencyService.Register<Bluetooth>();

            MainPage = new AppShell();
        }

        protected override void OnStart ()
        {
        }

        protected override void OnSleep ()
        {
        }

        protected override void OnResume ()
        {
        }
    }
}

