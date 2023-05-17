using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using BlueDog.Models;
using BlueDog.Views;
using System.Linq;
using BlueDog.Services;

namespace BlueDog.ViewModels
{
    public class RobotInfosViewModel : BaseViewModel
    {
        private RobotInfo _selectedRobotInfo;

        public ObservableCollection<RobotInfo> Items { get; }
        public Command LoadRobotInfosCommand { get; }
        public Command RunDogs { get;  }
        public Command<RobotInfo> RobotInfoTapped { get; }

        public RobotInfosViewModel()
        {
            Title = "Robots";
            Items = new ObservableCollection<RobotInfo>();
            LoadRobotInfosCommand = new Command(async () => await ExecuteLoadRobotInfosCommand());

            RobotInfoTapped = new Command<RobotInfo>(OnRobotInfoSelected);

            RunDogs = new Command(OnRunDogs);
        }

        async Task ExecuteLoadRobotInfosCommand()
        {
            IsBusy = true;

            try
            {
                Items.Clear();
                var RobotInfos = await DataStore.GetItemsAsync(true);
                foreach (var RobotInfo in RobotInfos)
                {
                    Items.Add(RobotInfo);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedRobotInfo = null;
        }

        public RobotInfo SelectedRobotInfo
        {
            get => _selectedRobotInfo;
            set
            {
                SetProperty(ref _selectedRobotInfo, value);
                OnRobotInfoSelected(value);
            }
        }

        private async void OnRunDogs(object obj)
        {
            Bluetooth bluetooth = DependencyService.Get<Bluetooth>();
            foreach (var item in Items)
            {
                await bluetooth.CallRobot(item.UUID);
            }
        }

        async void OnRobotInfoSelected(RobotInfo RobotInfo)
        {
            if (RobotInfo == null)
                return;

            // This will push the RobotInfoDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.ItemId)}={RobotInfo.UUID}");
        }
    }
}
