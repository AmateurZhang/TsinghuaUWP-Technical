using ClassRoomAPI.Controls;
using ClassRoomAPI.Helpers;
using ClassRoomAPI.Models;
using ClassRoomAPI.Services;
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
    public sealed partial class WebLearn : Page
    {
        public WebLearn()
        {
            this.InitializeComponent();
           // ImageBrush imageBrush = new ImageBrush();
            //imageBrush.ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/BuildingSix.png", UriKind.Absolute));
            //WebLearnPage.Background = imageBrush;
        }

      


        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ProgressStaue.Visibility = Visibility.Visible;
            ProgressStaue.IsIndeterminate = true;
            if (UserHelper.CredentialAbsent())
            {
                var notifyPopup = new NotifyPopup("未登录！");
                notifyPopup.Show();
            }
            else if(UserHelper.IsDemo())
            {
                var _Data = await WebLearnViewModels.GetAllWebLearnViewModel(ParseDataMode.Demo);
                MainPivot.ItemsSource = _Data.ListCourseInfoDetail;
                ProgressStaue.Visibility = Visibility.Collapsed;
                ProgressStaue.IsIndeterminate = false;
                return;
            }
            else
            {
                try
                {

                    var _Data = await WebLearnViewModels.GetAllWebLearnViewModel(ParseDataMode.Local);
                    MainPivot.ItemsSource = _Data.ListCourseInfoDetail;
                    if ((DateTime.Now - _Data.Date).Minutes > 5)
                    {

                        throw new Exception("The Data are out-of-date.");
                    }

                    var notifyPopup = new NotifyPopup("正在使用本地数据。");
                    notifyPopup.Show();


                }
                catch
                {
                    try
                    {
                        var _DataRemote = await WebLearnViewModels.GetAllWebLearnViewModel(ParseDataMode.Remote);
                        MainPivot.ItemsSource = _DataRemote.ListCourseInfoDetail;
                        var notifyPopup = new NotifyPopup("数据已经刷新。");
                        notifyPopup.Show();
                    }
                    catch
                    {
                        var notifyPopup = new NotifyPopup("网络异常，请检查网络。");
                        notifyPopup.Show();
                    }

                }
            }
            ProgressStaue.IsIndeterminate = false;
            ProgressStaue.Visibility = Visibility.Collapsed;
        }



        private void ListViewClassRoomData_ItemClick(object sender, ItemClickEventArgs e)
        {
            Deadline item = e.ClickedItem as Deadline;
            DetailFrame.Navigate(typeof(DDLdetail), item);

        }

        private void ListViewClassRoomData2_ItemClick(object sender, ItemClickEventArgs e)
        {
            Announce item = e.ClickedItem as Announce;
            DetailFrame.Navigate(typeof(AncDetail), item);
        }
    }
}