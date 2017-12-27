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

        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //教室信息的推荐
            var width = Window.Current.Bounds.Width / 3;
            //BuildingOne.Width = width;
            //BuildingTwo.Width = width;
            //BuildingThree.Width = width;
            //BuildingFour.Width = width;
            //BuildingFive.Width = width;
            //BuildingSix.Width = width;
            var _Data = await ClassRoomInfoViewModels.GetAllBuildingInfoViewModel(ParseDataMode.Local);
            BuildingList.ItemsSource = GetBuildingName(_Data);
            //演出信息的推荐页面
            var _DataLocal = await PerformaceViewModels.GetHallInfoViewModel(ParseDataMode.Local);
            DetailList.ItemsSource = GetRecommend(_DataLocal.ListShowInfo);
            FixedList.ItemsSource = GetRecommend(_DataLocal.ListShowInfo);

        }

        private List<BuildingTypeNamesData> GetBuildingName(ClassBuildingInfo Data)
        {
            var AllBuildingName = new List<BuildingTypeNamesData>();
            foreach (BuildingTypeNamesData items in Data.ListClassRoomInfo)
            {
                AllBuildingName.Add(new BuildingTypeNamesData
                {
                    PositionName = items.PositionName
                });
            }
            return AllBuildingName;
        }

        private List<PerformanceData> GetRecommend(List<ShowInfo> ListShowInfo)
        {
            // 所有的演出类型汇集成一张表
            var NewListPerformanceInfo = new List<PerformanceData>();
            foreach (ShowInfo TempShowInfo in ListShowInfo)
            {
                if (TempShowInfo.ListPerformanceInfo.Count == 0)
                {
                    continue;
                }
                foreach (PerformanceData TempPerformanceData in TempShowInfo.ListPerformanceInfo)
                {

                    //if (TempPerformanceData.PerformanceState == "售票中")
                    //{
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


                    NewListPerformanceInfo.Add(NewPerformaneData);
                    //}
                }
            }

            // 升序，小日期在前
            NewListPerformanceInfo.Sort((x, y) => { return x.PerformanceTime.CompareTo(y.PerformanceTime); });
            return NewListPerformanceInfo;
        }



        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            BuindingNames.BuildingName = 1;
            Frame rootFrame = Window.Current.Content as Frame;
            Shell ShellPage = rootFrame.Content as Shell;
            var page = ShellPage.RootFrame;

            if (page.CurrentSourcePageType == typeof(Recommend))
                page.Navigate(typeof(ClassRoomRecommand));
        }

        private void BuildingTwo_Click(object sender, RoutedEventArgs e)
        {
            BuindingNames.BuildingName = 2;
            Frame rootFrame = Window.Current.Content as Frame;
            Shell ShellPage = rootFrame.Content as Shell;
            var page = ShellPage.RootFrame;

            if (page.CurrentSourcePageType == typeof(Recommend))
                page.Navigate(typeof(ClassRoomRecommand));
        }

        private void BuildingThree_Click(object sender, RoutedEventArgs e)
        {
            BuindingNames.BuildingName = 3;
            Frame rootFrame = Window.Current.Content as Frame;
            Shell ShellPage = rootFrame.Content as Shell;
            var page = ShellPage.RootFrame;

            if (page.CurrentSourcePageType == typeof(Recommend))
                page.Navigate(typeof(ClassRoomRecommand));
        }

        private void BuildingFour_Click(object sender, RoutedEventArgs e)
        {
            BuindingNames.BuildingName = 4;
            Frame rootFrame = Window.Current.Content as Frame;
            Shell ShellPage = rootFrame.Content as Shell;
            var page = ShellPage.RootFrame;

            if (page.CurrentSourcePageType == typeof(Recommend))
                page.Navigate(typeof(ClassRoomRecommand));
        }

        private void BuildingFive_Click(object sender, RoutedEventArgs e)
        {
            BuindingNames.BuildingName = 5;
            Frame rootFrame = Window.Current.Content as Frame;
            Shell ShellPage = rootFrame.Content as Shell;
            var page = ShellPage.RootFrame;

            if (page.CurrentSourcePageType == typeof(Recommend))
                page.Navigate(typeof(ClassRoomRecommand));
        }

        private void BuildingSix_Click(object sender, RoutedEventArgs e)
        {
            BuindingNames.BuildingName = 6;
            Frame rootFrame = Window.Current.Content as Frame;
            Shell ShellPage = rootFrame.Content as Shell;
            var page = ShellPage.RootFrame;

            if (page.CurrentSourcePageType == typeof(Recommend))
                page.Navigate(typeof(ClassRoomRecommand));
        }

        private void BuildingList_ItemClick(object sender, ItemClickEventArgs e)
        {
            ListView item = sender as ListView;
            var k = e.ClickedItem;
            var j = 1;
        }

        private void BuildingList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView item = sender as ListView;
            var k = 1;
        }
    }
}
