using ClassRoomAPI.Controls;
using ClassRoomAPI.Helpers;
using ClassRoomAPI.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace ClassRoomAPI.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Emails : Page
    {
        public Emails()
        {
            this.InitializeComponent();
        }
        private async void Oninitial(object sender, RoutedEventArgs e)
        {

            if (UserHelper.IsDemo())
            {
                var notifyPopup = new NotifyPopup("测试用户！");
                notifyPopup.Show();
                Webemail.Visibility = Visibility.Visible;
                Warning.Visibility = Visibility.Collapsed;
            }
            else
                try
                {
                    string username = UserHelper.GetUserNumber();
                    string password = UserHelper.GetUserPassword();

                    var emailname = UserHelper.GetUserEmailName();

                    if(emailname==null)
                    {
                        try
                        {
                             emailname = await WebLearnAPIService.GetEmailName();
                        }
                        catch
                        {
                            var notifyPopup = new NotifyPopup("无法获取用户名称，请手动输入！");
                            notifyPopup.Show();
                        }
                    }
                    //string emailname = await WebLearnAPIService.GetEmailName();
                    // Webemail.Visibility = Visibility.Visible;
                    // Warning.Visibility = Visibility.Collapsed;

                    string js1 = "";
                    js1 += $"document.getElementsByName('password')[0].setAttribute('value','{password}');";
                    await Webemail.InvokeScriptAsync("eval", new string[] { js1 });

                    string js = $"var nm='{emailname}';";
                    js += "document.getElementById('username').value=nm;";
                    await Webemail.InvokeScriptAsync("eval", new string[] { js });

                    string js2 = "document.getElementsByName('action:login')[0].click();";
                    await Webemail.InvokeScriptAsync("eval", new string[] { js2 });

                    Webemail.Visibility = Visibility.Visible;
                    Warning.Visibility = Visibility.Collapsed;
                }
                catch
                {
                  
                    var notifyPopup = new NotifyPopup("网络故障或未登录！");
                    notifyPopup.Show();
                }


        }
    }
}
