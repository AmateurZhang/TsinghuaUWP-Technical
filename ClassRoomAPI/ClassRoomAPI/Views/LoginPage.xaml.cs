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
            var _Username = TextBoxUsername.Text;
            var _Password = PWBoxPassword.Password;
            Password password = new Password
            {
                password = _Password,
                username = _Username
            };
            int flag = 0;

            try
            {   
               flag=await WebLearnViewModels.LoginInToWebLearnUsingPassword(_Username, _Password);

               
            }
            catch(MessageException err)
            {
                var notifyPopup = new NotifyPopup("登陆失败！");
                notifyPopup.Show();
                var _Mess=err.Message;
                Info.Text = _Mess;
            }

            if(flag==1)
            {
             
                LocalSettingHelper.SetLocalSettings<string>("username", password.username);

                var vault = new Windows.Security.Credentials.PasswordVault();
                vault.Add(new Windows.Security.Credentials.PasswordCredential(
                    "Tsinghua_Learn_Website", password.username, password.password));

                UserInfoTB.Text = GetUserNumber();

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

            try
            {
                await WebLearnViewModels.GetAllWebLearnViewModel(ParseDataMode.Remote);
            }
            catch
            {

            }

            try
            {

                await WebLearnTimeTableViewModel.GetTimeTableViewModel(ParseDataMode.Remote);
            }
            catch
            {
                await ClassLibrary.WaitTask(3);
                try
                {
                    await WebLearnTimeTableViewModel.GetTimeTableViewModel(ParseDataMode.Remote);
                }
                catch
                {

                }
               
            }
           
            ProgressStaue.IsIndeterminate = false;
            ProgressStaue.Visibility = Visibility.Collapsed;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ImageBrush imageBrush = new ImageBrush();
            imageBrush.ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/BuildingTwo.png", UriKind.Absolute));
            Login_Page.Background = imageBrush;
            if (WebLearnAPIService.CredentialAbsent())
            {
                LoginStackPanel.Visibility = Visibility.Visible;
                UserInfo.Visibility = Visibility.Collapsed;
            }
            else
            {
                LoginStackPanel.Visibility = Visibility.Collapsed;
                UserInfo.Visibility = Visibility.Visible;
                UserInfoTB.Text = GetUserNumber();
            }
            ProgressStaue.Visibility = Visibility.Visible;
            ProgressStaue.IsIndeterminate = true;
            try
            {
                await WebLearnViewModels.GetAllWebLearnViewModel(ParseDataMode.Remote);
            }
            catch
            {

            }

            try
            {

                await WebLearnTimeTableViewModel.GetTimeTableViewModel(ParseDataMode.Remote);
            }
            catch
            {
                await ClassLibrary.WaitTask(3);
                try
                {
                    await WebLearnTimeTableViewModel.GetTimeTableViewModel(ParseDataMode.Remote);
                }
                catch
                {

                }

            }
            ProgressStaue.IsIndeterminate = false;
            ProgressStaue.Visibility = Visibility.Collapsed;
        }

        private string GetUserNumber()
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            return localSettings.Values["username"].ToString();
        }

        private void ChangeIDBT_Click(object sender, RoutedEventArgs e)
        {
            StorageFolder localCacheFolder = ApplicationData.Current.LocalCacheFolder;
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

            LoginStackPanel.Visibility = Visibility.Visible;
            UserInfo.Visibility = Visibility.Collapsed;
           
        }
    }
}
