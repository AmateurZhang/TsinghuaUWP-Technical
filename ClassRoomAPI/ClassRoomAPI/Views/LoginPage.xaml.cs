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
using ClassRoomAPI.ViewModels;
using ClassRoomAPI.Services;
using Windows.Storage;
using ClassRoomAPI.Models;
using ClassRoomAPI.Helpers;
using ClassRoomAPI.Controls;
using Windows.UI.Xaml.Media.Imaging;
using System.Threading.Tasks;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace ClassRoomAPI.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            this.InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            LoginProgressbar.Visibility = Visibility.Visible;
            LoginProgressbar.IsIndeterminate = true;
            LoginButton.IsEnabled = false;

            var _Username = TextBoxUsername.Text;
            var _Password = PWBoxPassword.Password;
            Password password = new Password
            {
                password = _Password,
                username = _Username
            };
            int flag = 0;

            //demo
            if(UserHelper.CheckDemo(password.username))
            {
                flag = 1;
                UserHelper.SetUserInfo(password);

                UserInfoTB.Text = UserHelper.GetUserNumber();

                LoginStackPanel.Visibility = Visibility.Collapsed;
                UserInfo.Visibility = Visibility.Visible;
                var notifyPopup = new NotifyPopup("测试用户登陆成功！");
                notifyPopup.Show();
                return;
            }

            try
            {
                try
                {
                    flag = await WebLearnViewModels.LoginInToWebLearnUsingPassword(_Username, _Password);
                }
               catch(Exception error)
                {
                    if (error.GetType() == typeof(MessageException))
                        throw error;
                    else
                    {
                        var notifyPopup = new NotifyPopup("网络故障！");
                        notifyPopup.Show();
                    }
                }
               
            }
            catch(MessageException err)
            {
                var notifyPopup = new NotifyPopup("登陆失败！");
                notifyPopup.Show();
                var _Mess=err.Message;
                Info.Text = _Mess;
                LoginProgressbar.Visibility = Visibility.Collapsed;
                LoginProgressbar.IsIndeterminate = false;
                LoginButton.IsEnabled = true;
                return;
            }

            LoginProgressbar.Visibility = Visibility.Collapsed;
            LoginProgressbar.IsIndeterminate = false;
            LoginButton.IsEnabled = true;



            if (flag==1)
            {

                UserHelper.SetUserInfo(password);

                UserInfoTB.Text = UserHelper.GetUserNumber();
                try
                {
                    var _Name = await WebLearnAPIService.GetEmailName();
                    UserHelper.SetUserEmailName(_Name);
                }
                catch
                {
                   
                }

                LoginStackPanel.Visibility = Visibility.Collapsed;
                UserInfo.Visibility = Visibility.Visible;
                var notifyPopup = new NotifyPopup("登陆成功！");
                notifyPopup.Show();
            }
            ProgressStaue.Visibility = Visibility.Visible;
            ProgressStaue.IsIndeterminate = true;

            try
            {
                await Windows.Storage.ApplicationData.Current.ClearAsync(ApplicationDataLocality.LocalCache);
            }
            catch
            {

            }
            var notifyPopup5 = new NotifyPopup("正在刷新数据，请稍等！");
            notifyPopup5.Show();

            await RefreshWebLearn();
            await RefreshBuilding();
            await RefreshCourse();
            await RefreshPerform();

            ProgressStaue.IsIndeterminate = false;
            ProgressStaue.Visibility = Visibility.Collapsed;
        }

        private async Task<int> RefreshWebLearn()
        {
            try
            {
                await WebLearnViewModels.GetAllWebLearnViewModel(ParseDataMode.Remote);
                var notifyPopup = new NotifyPopup("学堂数据已经刷新！");
                notifyPopup.Show();
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
            return 1;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ImageBrush imageBrush = new ImageBrush();
            imageBrush.ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/BuildingTwo.png", UriKind.Absolute));
            Login_Page.Background = imageBrush;
            if (UserHelper.CredentialAbsent())
            {
                LoginStackPanel.Visibility = Visibility.Visible;
                UserInfo.Visibility = Visibility.Collapsed;
            }
            else
            {
                LoginStackPanel.Visibility = Visibility.Collapsed;
                UserInfo.Visibility = Visibility.Visible;
                UserInfoTB.Text = UserHelper.GetUserNumber();
            }
            ProgressStaue.Visibility = Visibility.Visible;
            ProgressStaue.IsIndeterminate = true;

            var notifyPopup = new NotifyPopup("正在刷新数据，请稍等！");
            notifyPopup.Show();
            await RefreshWebLearn();
            await RefreshBuilding();
            await RefreshCourse();
            await RefreshPerform();

            ProgressStaue.IsIndeterminate = false;
            ProgressStaue.Visibility = Visibility.Collapsed;
        }

        //private string GetUserNumber()
        //{
        //    ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        //    return localSettings.Values["username"].ToString();
        //}

        private void ChangeIDBT_Click(object sender, RoutedEventArgs e)
        {
            StorageFolder localCacheFolder = ApplicationData.Current.LocalCacheFolder;
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

            LoginStackPanel.Visibility = Visibility.Visible;
            UserInfo.Visibility = Visibility.Collapsed;
           
        }
    }
}
