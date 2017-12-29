using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;
using ClassRoomAPI.Services;
using ClassRoomAPI.ViewModels;
using Windows.UI.Popups;
using ClassRoomAPI.Models;


// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace ClassRoomAPI
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void HHH_Click(object sender, RoutedEventArgs e)
        {
           
            try
            {
                var Data = await ClassRoomInfoViewModels.GetBuildingInfoByName();
                NavMenuPrimaryListView.ItemsSource = Data;
            }
            catch(MessageException _e)
            {
                var Data = await ClassRoomInfoViewModels.GetBuildingInfoByName();
                NavMenuPrimaryListView.ItemsSource = Data;
                MessageDialog ms1g = new MessageDialog(_e.Message);
                await ms1g.ShowAsync();
            }
            
            

        }

        public async void TempRefreshDataCortana()
        {
            var Data = await ClassRoomInfoViewModels.GetBuildingInfoByName();
            NavMenuPrimaryListView.ItemsSource = Data;
        }

        private async void GetHallList_Click(object sender, RoutedEventArgs e)
        {
            var Data = await ClassRoomInfoViewModels.GetBuildingNames();
            HallListView.ItemsSource = Data;
        }



        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var Data = await ClassRoomInfoViewModels.GetBuildingInfoByName();
            NavMenuPrimaryListView.ItemsSource = Data;
            try
            {
                var DataNames = await ClassRoomInfoViewModels.GetBuildingNamesViewModel(ParseDataMode.Local);
                HallListView.ItemsSource = DataNames;
            }
            catch
            {

            }
           
        }
    }
}
