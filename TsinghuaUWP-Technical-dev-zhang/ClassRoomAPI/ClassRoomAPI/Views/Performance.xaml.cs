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

        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var _Data = await PerformaceViewModels.GetHallInfoViewModel(ParseDataMode.Local);
                if ((_Data.Date.Date - DateTime.Now.Date).Days < 0)
                    throw new Exception("The Data are out-of-date.");
                else
                    MainPivot.ItemsSource = _Data.ListShowInfo;
                var notifyPopup = new NotifyPopup("正在使用本地数据。");
                notifyPopup.Show();
            }
            catch
            {
                try
                {
                    var _DataRemote = await PerformaceViewModels.GetHallInfoViewModel(ParseDataMode.Remote);
                    MainPivot.ItemsSource = _DataRemote.ListShowInfo;
                    var notifyPopup = new NotifyPopup("演出信息已更新！");
                    notifyPopup.Show();
                }
                catch
                {
                    var notifyPopup = new NotifyPopup("网络异常，刷新失败。");
                    notifyPopup.Show();
                }

            }
        }
    }

}
