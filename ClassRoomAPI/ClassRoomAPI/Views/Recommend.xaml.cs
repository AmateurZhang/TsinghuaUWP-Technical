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
using ClassRoomAPI.Helpers;
using System.Threading.Tasks;

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
          
            //ImageBrush imageBrush = new ImageBrush();
            //imageBrush.ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/BuildingTwo.png", UriKind.Absolute));
           // RecommendPage.Background = imageBrush;
        }

        private async Task<int> RefreshWebLearn()
        {
            try
            {
                await WebLearnViewModels.GetAllWebLearnViewModel(ParseDataMode.Remote);
                var notifyPopup = new NotifyPopup("学堂数据已经刷新！");
                notifyPopup.Show();
                UrgentDDLFrame.Navigate(typeof(UrgentDDL));
            }
            catch
            {
                var notifyPopup = new NotifyPopup("学堂数据刷新失败！");
                notifyPopup.Show();
            }
            return 1;
        }

        private async Task<int> RefreshPerform()
        {
            try
            {
                await PerformaceViewModels.GetHallInfoViewModel(ParseDataMode.Remote);
                var notifyPopup = new NotifyPopup("演出数据已经刷新！");
                notifyPopup.Show();
                PerformanceFrame.Navigate(typeof(RecPerformance));
            }
            catch
            {
                var notifyPopup = new NotifyPopup("演出数据刷新失败！");
                notifyPopup.Show();
            }
            return 1;
        }

        private async Task<int> RefreshBuilding()
        {
            try
            {
                await ClassRoomInfoViewModels.GetAllBuildingInfoViewModel(ParseDataMode.Remote);
                var notifyPopup = new NotifyPopup("教室数据已经刷新！");
                notifyPopup.Show();
                ClassRoomFrame.Navigate(typeof(RecClassroom));
            }
            catch
            {
                var notifyPopup = new NotifyPopup("教室数据刷新失败！");
                notifyPopup.Show();
            }
            return 1;
        }

        private async Task<int> RefreshCourse()
        {
            try
            {

                await WebLearnTimeTableViewModel.GetTimeTableViewModel(ParseDataMode.Remote);
                var notifyPopup = new NotifyPopup("课程数据已经刷新！");
                notifyPopup.Show();
                CoursesFrame.Navigate(typeof(CourseNext));
            }
            catch
            {
                await ClassLibrary.WaitTask(3);
                try
                {
                    await WebLearnTimeTableViewModel.GetTimeTableViewModel(ParseDataMode.Remote);
                    var notifyPopup = new NotifyPopup("课程数据已经刷新！");
                    notifyPopup.Show();
                    CoursesFrame.Navigate(typeof(CourseNext));
                }
                catch
                {
                    var notifyPopup = new NotifyPopup("课程数据刷新失败！");
                    notifyPopup.Show();
                }

            }
            return 1;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
          

            UrgentDDLFrame.Navigate(typeof(UrgentDDL));
            CoursesFrame.Navigate(typeof(CourseNext));


            ClassRoomFrame.Navigate(typeof(RecClassroom));
            PerformanceFrame.Navigate(typeof(RecPerformance));

           
            
        }

        private async void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            var notifyPopup = new NotifyPopup("正在刷新数据，请稍等！");
            notifyPopup.Show();
            AppBarBottom.IsOpen = false;
            ProgressStaue.Visibility = Visibility.Visible;
            ProgressStaue.IsIndeterminate = true;
            if(UserHelper.IsDemo())
            {
                ProgressStaue.IsIndeterminate = false;
                ProgressStaue.Visibility = Visibility.Collapsed;

                ClassRoomFrame.Navigate(typeof(RecClassroom));
                PerformanceFrame.Navigate(typeof(RecPerformance));

                UrgentDDLFrame.Navigate(typeof(UrgentDDL));
                CoursesFrame.Navigate(typeof(CourseNext));
                return;
            }

            await RefreshWebLearn();
            await RefreshBuilding();
            await RefreshCourse();
            await RefreshPerform();

            ProgressStaue.IsIndeterminate = false;
            ProgressStaue.Visibility = Visibility.Collapsed;

            //ClassRoomFrame.Navigate(typeof(RecClassroom));
            //PerformanceFrame.Navigate(typeof(RecPerformance));

            //UrgentDDLFrame.Navigate(typeof(UrgentDDL));
            //CoursesFrame.Navigate(typeof(CourseNext));
        }
    }
}
