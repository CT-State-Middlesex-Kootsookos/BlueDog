using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlueDog.Models;
using Newtonsoft.Json;
using Xamarin.Essentials;

namespace BlueDog.Services
{
    public class PreferencesDataStore : IDataStore<RobotInfo>
    {
        const String MY_BLUETOOTH_DEVICES = "my_bluetooth_devices_mk5";
        const String NO_LIST = "no_list";

        readonly List<RobotInfo> items;

        public PreferencesDataStore()
        {

            if (Preferences.ContainsKey(MY_BLUETOOTH_DEVICES))
            {
                var serializedList = Preferences.Get(MY_BLUETOOTH_DEVICES, NO_LIST);
                if (serializedList != NO_LIST)
                {
                    items = JsonConvert.DeserializeObject<List<RobotInfo>>(serializedList);
                }
            }
            else
            {
                items = new List<RobotInfo>()
                {
                };
            }
        }

        private bool ContainsUUID(string UUID)
        {
            var itemsThatMatch = items.Where(robot => robot.UUID == UUID).ToList();
            return itemsThatMatch.Count > 0;
        }

        public async Task<bool> AddItemAsync(RobotInfo item)
        {
            if (!ContainsUUID(item.UUID))
            {
                items.Add(item);
                Preferences.Set(MY_BLUETOOTH_DEVICES, JsonConvert.SerializeObject(items));
            }

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(RobotInfo item)
        {
            var oldRobotInfo = items.Where((RobotInfo arg) => arg.UUID == item.UUID).FirstOrDefault();
            items.Remove(oldRobotInfo);
            items.Add(item);

            Preferences.Set(MY_BLUETOOTH_DEVICES, JsonConvert.SerializeObject(items));

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldRobotInfo = items.Where((RobotInfo arg) => arg.UUID == id).FirstOrDefault();
            items.Remove(oldRobotInfo);

            Preferences.Set(MY_BLUETOOTH_DEVICES, JsonConvert.SerializeObject(items));

            return await Task.FromResult(true);
        }

        public async Task<RobotInfo> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.UUID == id));
        }

        public async Task<IEnumerable<RobotInfo>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }
    }
}
