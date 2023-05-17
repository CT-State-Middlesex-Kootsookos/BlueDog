using System;
using Plugin.BLE.Abstractions.Contracts;

namespace BlueDog.Models
{
	public class RobotInfo
	{
		public string Name { get; set; }
        public string HardwareName { get; set; }
        public string UUID { get; set; }

        public RobotInfo()
		{
		}

        public RobotInfo(IDevice device)
        {
            Name = device.Name;
            HardwareName = device.Name;
            UUID = device.Id.ToString();
        }

        // Add Bittle, or Petoi named devices to the list.
        public static bool InterestingName(IDevice device)
        {
            return device.Name != null
                && (device.Name.Contains("Bittle")
                || device.Name.Contains("Petoi"));
        }
    }
}

