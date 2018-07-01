using ClassRoomAPI.Controls;
using ClassRoomAPI.Helpers;
using ClassRoomAPI.Models;
using ClassRoomAPI.ViewModels;
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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;


// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace ClassRoomAPI.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class ClassRoomRecommand : Page
    {
        private List<ClassRoomStatueData> ClassRooms;


        public ClassRoomRecommand()
        {
            this.InitializeComponent();
        }
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var _Data = new ClassBuildingInfo();
            if(UserHelper.IsDemo())
                _Data = await ClassRoomInfoViewModels.GetAllBuildingInfoViewModel(ParseDataMode.Demo);
            else
                _Data = await ClassRoomInfoViewModels.GetAllBuildingInfoViewModel(ParseDataMode.Local);

            if (_Data != null)
            {
                var time = DateTime.Now;
                int[] classlist = new int[6];
                //判断当前是哪节课
                var now = WhichClass(time.Hour, time.Minute);
                ClassRooms = _Data.ListClassRoomStatue[BuindingNames.BuildingName - 1].ListBuildingInfoData;
                var Re_ClassRooms = new List<ClassRoomStatueData>();
                for (int k = 0; k < ClassRooms.Count; k++)
                {
                    var items = ClassRooms[k];
                    if (now < 10 && now>0)
                    {
                        if (!items.ListBoolClassStatus[now-1]) Re_ClassRooms.Add(new ClassRoomStatueData
                        {
                            ListClassStatus = items.ListClassStatus,
                            ClassRoomName = items.ClassRoomName,
                            ListBoolClassStatus = items.ListBoolClassStatus
                        }
                            );
                    }
                    else {
                        Re_ClassRooms.Add(new ClassRoomStatueData
                        {
                            ListClassStatus = items.ListClassStatus,
                            ClassRoomName = items.ClassRoomName,
                            ListBoolClassStatus = items.ListBoolClassStatus
                        }
                            );
                    }
                }
                ListViewClassRoomData.ItemsSource = Re_ClassRooms;
                if(Re_ClassRooms.Count()==0)
                {
                    noteBlock.Text = "当前教学楼无空闲教室";
                    noteBlock.Visibility = Visibility.Visible;
                }
                else
                {
                    noteBlock.Visibility = Visibility.Collapsed;
                }
                    
            }
            else {
                var notifyPopup = new NotifyPopup("请连接校内网获取教室信息");
                notifyPopup.Show();
            }
        }
        //判断是哪节课
        private int WhichClass(int hour, int minite)
        {

            int NumOfClass = 0;
            if (hour == 8) NumOfClass = 1;
            if (hour == 9)
            {
                if (minite <= 35) NumOfClass = 1;
                else if (minite >= 50) NumOfClass = 2;
                else NumOfClass = 12;
            }
            if (hour == 10) NumOfClass = 2;
            if (hour == 11)
            {
                if (minite <= 25) NumOfClass = 2;
                else NumOfClass = 23;
            }
            if (hour == 12) NumOfClass = 23;
            if (hour == 13)
            {
                if (minite >= 30) NumOfClass = 3;
                else NumOfClass = 23;
            }
            if (hour == 14) NumOfClass = 3;
            if (hour == 15)
            {
                if (minite <= 5) NumOfClass = 3;
                else if (minite >= 20) NumOfClass = 4;
                else NumOfClass = 34;
            }
            if (hour == 16)
            {
                if (minite <= 55) NumOfClass = 4;
                else NumOfClass = 45;
            }
            if (hour == 17)
            {
                if (minite >= 5) NumOfClass = 5;
                else NumOfClass = 45;
            }
            if (hour == 18)
            {
                if (minite <= 40) NumOfClass = 5;
                else NumOfClass = 56;
            }
            if (hour == 19)
            {
                if (minite >= 20) NumOfClass = 6;
                else NumOfClass = 56;
            }
            if (hour == 20)
            {
                if (minite <= 50) NumOfClass = 6;
                else NumOfClass = 10;
            }
            return NumOfClass;
        }
    }
}
