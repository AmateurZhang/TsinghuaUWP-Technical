using ClassRoomAPI.Controls;
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

            var _Data = await ClassRoomInfoViewModels.GetAllBuildingInfoViewModel(ParseDataMode.Local);
            var time = DateTime.Now;
            int[] classlist = new int[6];
            //判断当前是哪节课
            var now = WhichClass(time.Hour, time.Minute);
            var cl = 0;
            var Schedule = await WebLearnTimeTableViewModel.GetTimeTableViewModel(ParseDataMode.Local);
            //看一下今天是否有课，如果有课，就在课表a中的对应位置标记1
            foreach (Windows.ApplicationModel.Appointments.Appointment Items in Schedule.ListAppointment)
            {
                if ((Items.StartTime.Year == time.Year) && (Items.StartTime.Month == time.Month) && (Items.StartTime.Day == time.Day))
                {
                    cl = WhichClass(Items.StartTime.Hour, Items.StartTime.Minute);
                    if (cl == now) classlist[cl - 1] = 1;

                }
            }

            ClassRooms = _Data.ListClassRoomStatue[BuindingNames.BuildingName - 1].ListBuildingInfoData;
            var Re_ClassRooms = new List<ClassRoomStatueData>();
            for (int k = 0; k < ClassRooms.Count; k++)
            {
                var items = ClassRooms[k];
                //看一下每间教室在课表有课的时候是否空闲，如果空闲，就将它加入推荐教室里面

                for (int j = 0; j < 6; j++)
                {
                    //没课
                    if (classlist[j] == 0)
                    {
                        if (!items.ListBoolClassStatus[j]) Re_ClassRooms.Add(new ClassRoomStatueData
                        {
                            ListClassStatus = items.ListClassStatus,
                            ClassRoomName = items.ClassRoomName,
                            ListBoolClassStatus = items.ListBoolClassStatus
                        }

                    );
                        break;
                    }

                }

            }
            ListViewClassRoomData.ItemsSource = Re_ClassRooms;
            //ListViewClassRoomData.ItemsSource = _Data.ListClassRoomStatue[BuindingNames.BuildingName - 1].ListBuildingInfoData;

        }

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
