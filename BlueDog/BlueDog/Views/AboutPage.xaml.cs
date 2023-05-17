using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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
        const String MY_BLUETOOTH_DEVICES = "my_bluetooth_devices";
        const String NO_LIST = "no_list";

        List<String> knownRobots = new List<String>();

        List<IDevice> deviceList = new List<IDevice>();

        List<IDevice> myRobots = new List<IDevice>();

        IAdapter adapter;

        public AboutPage()
        {
            InitializeComponent();

            if (Preferences.ContainsKey(MY_BLUETOOTH_DEVICES))
            {
                var serializedList = Preferences.Get(MY_BLUETOOTH_DEVICES, NO_LIST);
                if (serializedList != NO_LIST)
                {
                    knownRobots = JsonConvert.DeserializeObject<List<String>>(serializedList);
                }
            }

            var ble = CrossBluetoothLE.Current;
            adapter = CrossBluetoothLE.Current.Adapter;            

            adapter.DeviceDiscovered += (s, a) => deviceList.Add(a.Device);            
        }

        async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            if (sender is Button)
            {
                Button sent = (Button)sender;
                sent.IsVisible = false;
            }

            myRobots.Clear();

            await adapter.StartScanningForDevicesAsync();

            foreach (IDevice device in deviceList)
            {
                if (InterestingName(device))
                {
                    myRobots.Add(device);
                    if (!knownRobots.Contains(device.Name))
                    {
                        knownRobots.Add(device.Name);
                    }
                }
            }
            
            await DisplayAlert("Devices", myRobots.Count.ToString() + " devices found.", "OK");

            if (myRobots.Count > 0)
            {
                try
                {
                    foreach (var robot in myRobots)
                    {
                        await CallRobot(robot);
                    }
                }
                catch (DeviceConnectionException exception)
                {
                    Console.WriteLine(exception.ToString());
                }
            }

            if (sender is Button)
            {
                Button sent = (Button)sender;
                sent.IsVisible = true;
            }

            Preferences.Set(MY_BLUETOOTH_DEVICES, JsonConvert.SerializeObject(knownRobots));

        }

        // Add Bittle, or Petoi named devices to the list.
        private bool InterestingName(IDevice device)
        {
            return device.Name != null
                && (device.Name.Contains("Bittle")
                || device.Name.Contains("Petoi"));
        }

        private async Task CallRobot(IDevice robot)
        {            
            await adapter.ConnectToDeviceAsync(robot);

            var services = await robot.GetServicesAsync();

            foreach (var service in services)
            {
                Console.WriteLine("Service:" + service.Name);
                
                var characteristics = await service.GetCharacteristicsAsync();
                foreach (var characteristic in characteristics)
                {
                    Console.WriteLine("Characteristic: " + characteristic.Name + ":" + characteristic.Id.ToString());

                    if (characteristic.Name.Contains("Unknown"))
                    {
                        await characteristic.WriteAsync(Encoding.ASCII.GetBytes("kbalance"));
                        await Task.Delay(10000);
                        await characteristic.WriteAsync(Encoding.ASCII.GetBytes("ksit"));
                        await Task.Delay(10000);
                        await characteristic.WriteAsync(Encoding.ASCII.GetBytes("kstr"));
                        await Task.Delay(10000);
                        await characteristic.WriteAsync(Encoding.ASCII.GetBytes("kzero"));
                        await Task.Delay(10000);
                    }

                    var descriptors = await characteristic.GetDescriptorsAsync();

                    foreach (var descriptor in descriptors)
                    {
                        Console.WriteLine("Descriptor:" + descriptor.Name);
                        var bytes = await descriptor.ReadAsync();
                        foreach (var bite in bytes)
                        {
                            Console.WriteLine("Bytes:" + bite.ToString());
                        }
                    }
                }
            }
        }
    }
}
