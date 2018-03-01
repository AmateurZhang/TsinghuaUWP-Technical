using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
using ClassRoomAPI.Controls;
using Windows.Storage.Streams;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;
using ClassRoomAPI.Helpers;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace ClassRoomAPI.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Performance : Page
    {
        public Performance()
        {
            this.InitializeComponent();
            //加载背景
            ImageBrush imageBrush = new ImageBrush();
            imageBrush.ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/Hall.png", UriKind.Absolute));
            Performance_Page.Background = imageBrush;
        }

        
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            
            ProgressStaue.Visibility = Visibility.Visible;
            ProgressStaue.IsIndeterminate = true;
            try
            {
                var _DataLocal = new ShowInfoData();
                if (UserHelper.IsDemo())
                {
                    _DataLocal = await PerformaceViewModels.GetHallInfoViewModel(ParseDataMode.Demo);
                    MainPivot.ItemsSource = _DataLocal.ListShowInfo;
                    ProgressStaue.IsIndeterminate = false;
                    ProgressStaue.Visibility = Visibility.Collapsed;
                    return;

                }
                else
                {
                    _DataLocal = await PerformaceViewModels.GetHallInfoViewModel(ParseDataMode.Local);
                }
        

                MainPivot.ItemsSource = _DataLocal.ListShowInfo;
                if ((DateTime.Now-_DataLocal.Date).TotalMinutes > 5)
                    throw new Exception("The Data are out-of-date.");
                else
                    MainPivot.ItemsSource = _DataLocal.ListShowInfo;
                
                var notifyPopup = new NotifyPopup("数据更新于"+ _DataLocal.Date);
                notifyPopup.Show();
            }
            catch
            {
                try
                {
                    var _DataRemote = await PerformaceViewModels.GetHallInfoViewModel(ParseDataMode.Remote);
                    MainPivot.ItemsSource = _DataRemote.ListShowInfo;
                    var notifyPopup = new NotifyPopup("演出信息已更新至最新状态！");
                    notifyPopup.Show();
                }
                catch
                {
                    var notifyPopup = new NotifyPopup("网络异常，刷新失败。");
                    notifyPopup.Show();
                }

            }
            ProgressStaue.IsIndeterminate = false;
            ProgressStaue.Visibility = Visibility.Collapsed;
        }


        private void MainPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
    }

    

}
