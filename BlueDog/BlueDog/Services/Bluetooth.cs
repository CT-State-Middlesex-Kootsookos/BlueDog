using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BlueDog.Models;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Xamarin.Forms;

namespace BlueDog.Services
{
	public class Bluetooth
	{
        List<IDevice> deviceList = new List<IDevice>();

        public IDataStore<RobotInfo> DataStore = DependencyService.Get<IDataStore<RobotInfo>>();

        public Bluetooth()
		{
        }

        async public Task ScanForDevices()
        {
            deviceList.Clear();

            IAdapter adapter = CrossBluetoothLE.Current.Adapter;

            adapter.DeviceDiscovered += (s, a) =>
            {                
               deviceList.Add(a.Device);
            };

            await adapter.StartScanningForDevicesAsync();

            foreach (var device in deviceList)
            {
                if (RobotInfo.InterestingName(device))
                {
                    await DataStore.AddItemAsync(new RobotInfo(device));
                }
            }
        }

        public async Task CallRobot(string UUID)
        {
            IAdapter adapter = CrossBluetoothLE.Current.Adapter;
            IDevice robot = await adapter.ConnectToKnownDeviceAsync(new Guid(UUID));            

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

