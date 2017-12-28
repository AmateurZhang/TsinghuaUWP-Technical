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

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace ClassRoomAPI.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class RecommendPerformance : Page
    {
        public RecommendPerformance()
        {
            this.InitializeComponent();

        }

        private List<PerformanceData> GetRecommend(List<ShowInfo> ListShowInfo)
        {
            // 所有的演出类型汇集成一张表
            var NewListPerformanceInfo = new List<PerformanceData>();
            var PastListPerformanceInfo = new List<PerformanceData>();
            foreach (ShowInfo TempShowInfo in ListShowInfo)
            {
                if (TempShowInfo.ListPerformanceInfo.Count == 0)
                {
                    continue;
                }
                foreach (PerformanceData TempPerformanceData in TempShowInfo.ListPerformanceInfo)
                {
                    var NewPerformaneData = new PerformanceData();
                    // 为了测试效果，先把所有内容都收集起来
                    var PerformanceName = TempPerformanceData.PerformanceName; // 考虑提取《》或""或“”之间的内容
                    var PerformanceState = String.Format("时间：{0}\n类型：{1}\n地点：{2}\n",
                        TempPerformanceData.PerformanceTime, TempShowInfo.PerformanceType, TempPerformanceData.PerformanceAddress);

                    NewPerformaneData.PerformanceName = PerformanceName;
                    NewPerformaneData.PerformanceState = PerformanceState;
                    NewPerformaneData.PerformanceTime = TempPerformanceData.PerformanceTime;
                    // 在PerformanceAddress中存颜色
                    switch (TempShowInfo.PerformanceType)
                    {
                        case "电影":
                            NewPerformaneData.PerformanceAddress = "Purple";
                            break;
                        case "演出":
                            NewPerformaneData.PerformanceAddress = "DarkOrange";
                            break;
                        case "讲座":
                            NewPerformaneData.PerformanceAddress = "BlueViolet";
                            break;
                        case "艺术丛林":
                            NewPerformaneData.PerformanceAddress = "DarkGreen";
                            break;
                    }
                    try
                    {
                        DateTime PerformanceTime = DateTime.ParseExact(NewPerformaneData.PerformanceTime.Substring(0, 10), "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
                        if ((PerformanceTime.Date - DateTime.Now.Date).Days >= 0)
                        {
                            NewListPerformanceInfo.Add(NewPerformaneData);
                        }
                        else
                        {
                            NewPerformaneData.PerformanceAddress = "DimGray";
                            PastListPerformanceInfo.Add(NewPerformaneData);
                        }
                    }
                    catch
                    {
                        // 如果日期有问题
                        NewPerformaneData.PerformanceAddress = "DimGray";
                        PastListPerformanceInfo.Add(NewPerformaneData);
                    }
                }
            }
            try
            {
                // 升序，小日期在前
                NewListPerformanceInfo.Sort((x, y) => { return x.PerformanceTime.CompareTo(y.PerformanceTime); });
                PastListPerformanceInfo.Sort((x, y) => { return -x.PerformanceTime.CompareTo(y.PerformanceTime); });
                NewListPerformanceInfo.AddRange(PastListPerformanceInfo);
            }
            catch { }
            return NewListPerformanceInfo;
            
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var _DataLocal = await PerformaceViewModels.GetHallInfoViewModel(ParseDataMode.Local);

                if ((_DataLocal.Date.Date - DateTime.Now.Date).Days < 0)
                    throw new Exception("The Data are out-of-date.");
                else
                {
                    var ShowList = GetRecommend(_DataLocal.ListShowInfo);
                    if (ShowList.Count == 0)
                    {
                        var notifyPopup = new NotifyPopup("暂时没有演出相关信息！");
                        notifyPopup.Show();
                    }
                    else
                    {
                        DetailList.ItemsSource = ShowList;
                        FixedList.ItemsSource = ShowList;
                    }
                }
            }
            catch
            {
                try
                {
                    var _DataRemote = await PerformaceViewModels.GetHallInfoViewModel(ParseDataMode.Remote);
                    var ShowList = GetRecommend(_DataRemote.ListShowInfo);
                    if (ShowList.Count == 0)
                    {
                        var notifyPopup = new NotifyPopup("暂时没有演出相关信息！");
                        notifyPopup.Show();
                    }
                    else
                    {
                        DetailList.ItemsSource = ShowList;
                        FixedList.ItemsSource = ShowList;
                    }
                }
                catch
                {
                }

            }
        }
    }
}
