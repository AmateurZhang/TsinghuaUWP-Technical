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
                var notifyPopup = new NotifyPopup("登录失败！");
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
                var notifyPopup = new NotifyPopup("登录成功！");
                notifyPopup.Show();
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if(WebLearnAPIService.CredentialAbsent())
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
        }

        private string GetUserNumber()
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            return localSettings.Values["username"].ToString();
        }

        private async void ChangeIDBT_Click(object sender, RoutedEventArgs e)
        {
            StorageFolder localCacheFolder = ApplicationData.Current.LocalCacheFolder;
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            try
            {
                localSettings.Values["username"] = null;
            }
            catch
            {

            }
            
            try
            {
              //Cache 处理
               
            
            }
            catch
            {

            }
        

            LoginStackPanel.Visibility = Visibility.Visible;
            UserInfo.Visibility = Visibility.Collapsed;
            

        }

    }
}
