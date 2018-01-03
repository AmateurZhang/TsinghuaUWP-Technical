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
using ClassRoomAPI.Models;
using ClassRoomAPI.ViewModels;
using Windows.UI.Xaml.Media.Imaging;
using ClassRoomAPI.Controls;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace ClassRoomAPI.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Recommend : Page
    {
        public Recommend()
        {
            this.InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Enabled;
        }



        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ImageBrush imageBrush = new ImageBrush();
            imageBrush.ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/BuildingTwo.png", UriKind.Absolute));
            RecommendPage.Background = imageBrush;

            UrgentDDLFrame.Navigate(typeof(UrgentDDL));
            CoursesFrame.Navigate(typeof(CourseNext));


            ClassRoomFrame.Navigate(typeof(RecClassroom));
            PerformanceFrame.Navigate(typeof(RecPerformance));

            



            
        }

        private async void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            AppBarBottom.IsOpen = false;
            ProgressStaue.Visibility = Visibility.Visible;
            ProgressStaue.IsIndeterminate = true;
            try
            {
                await WebLearnViewModels.GetAllWebLearnViewModel(ParseDataMode.Remote);
                var notifyPopup = new NotifyPopup("作业数据已经刷新！");
                notifyPopup.Show();
            }
            catch
            {
                var notifyPopup = new NotifyPopup("作业数据刷新失败！");
                notifyPopup.Show();
            }
            try
            {
                await PerformaceViewModels.GetHallInfoViewModel(ParseDataMode.Remote);
                var notifyPopup = new NotifyPopup("课程数据已经刷新！");
                notifyPopup.Show();
            }
            catch
            {
                var notifyPopup = new NotifyPopup("课程数据刷新失败！");
                notifyPopup.Show();
            }
            try
            {
                await ClassRoomInfoViewModels.GetAllBuildingInfoViewModel(ParseDataMode.Remote);
                var notifyPopup = new NotifyPopup("演出数据已经刷新！");
                notifyPopup.Show();
            }
            catch
            {
                var notifyPopup = new NotifyPopup("演出数据刷新失败！");
                notifyPopup.Show();
            }
            try
            {

                await WebLearnTimeTableViewModel.GetTimeTableViewModel(ParseDataMode.Remote);
                var notifyPopup = new NotifyPopup("课程数据已经刷新！");
                notifyPopup.Show();
            }
            catch
            {
                await ClassLibrary.WaitTask(3);
                try
                {
                    await WebLearnTimeTableViewModel.GetTimeTableViewModel(ParseDataMode.Remote);
                    var notifyPopup = new NotifyPopup("课程数据已经刷新！");
                    notifyPopup.Show();
                }
                catch
                {
                    var notifyPopup = new NotifyPopup("课程数据刷新失败！");
                    notifyPopup.Show();
                }

            }

            ProgressStaue.IsIndeterminate = false;
            ProgressStaue.Visibility = Visibility.Collapsed;

            ClassRoomFrame.Navigate(typeof(RecClassroom));
            PerformanceFrame.Navigate(typeof(RecPerformance));

            UrgentDDLFrame.Navigate(typeof(UrgentDDL));
            CoursesFrame.Navigate(typeof(CourseNext));
        }
    }
}
