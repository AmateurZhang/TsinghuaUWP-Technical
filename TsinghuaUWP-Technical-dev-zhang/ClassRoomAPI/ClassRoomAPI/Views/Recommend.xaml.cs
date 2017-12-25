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
            var width = Window.Current.Bounds.Width/3;
            BuildingOne.Width = width;
            
            BuildingTwo.Width = width;
            BuildingThree.Width = width;
            BuildingFour.Width = width;
            BuildingFive.Width = width;
            BuildingSix.Width = width;
            var _Data = await ClassRoomInfoViewModels.GetAllBuildingInfoViewModel(ParseDataMode.Local);
            
        }

        

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            BuindingNames.BuildingName = 1;
            Frame rootFrame = Window.Current.Content as Frame;
            Shell ShellPage = rootFrame.Content as Shell;
            var page = ShellPage.RootFrame;
            if (page.CurrentSourcePageType == typeof(Recommend)) {
                page.Navigate(typeof(ClassRoomRecommand));
            }

        }

        private void BuildingTwo_Click(object sender, RoutedEventArgs e)
        {
            BuindingNames.BuildingName = 2;
            Frame rootFrame = Window.Current.Content as Frame;
            Shell ShellPage = rootFrame.Content as Shell;
            var page = ShellPage.RootFrame;
            if (page.CurrentSourcePageType == typeof(Recommend))
            {
                page.Navigate(typeof(ClassRoomRecommand));
            }
        }

        private void BuildingThree_Click(object sender, RoutedEventArgs e)
        {
            BuindingNames.BuildingName = 3;
            Frame rootFrame = Window.Current.Content as Frame;
            Shell ShellPage = rootFrame.Content as Shell;
            var page = ShellPage.RootFrame;
            if (page.CurrentSourcePageType == typeof(Recommend))
            {
                page.Navigate(typeof(ClassRoomRecommand));
            }
        }

        private void BuildingFour_Click(object sender, RoutedEventArgs e)
        {
            BuindingNames.BuildingName = 4;
            Frame rootFrame = Window.Current.Content as Frame;
            Shell ShellPage = rootFrame.Content as Shell;
            var page = ShellPage.RootFrame;
            if (page.CurrentSourcePageType == typeof(Recommend))
            {
                page.Navigate(typeof(ClassRoomRecommand));
            }
        }

        private void BuildingFive_Click(object sender, RoutedEventArgs e)
        {
            BuindingNames.BuildingName = 5;
            Frame rootFrame = Window.Current.Content as Frame;
            Shell ShellPage = rootFrame.Content as Shell;
            var page = ShellPage.RootFrame;
            if (page.CurrentSourcePageType == typeof(Recommend))
            {
                page.Navigate(typeof(ClassRoomRecommand));
            }
        }

        private void BuildingSix_Click(object sender, RoutedEventArgs e)
        {
            BuindingNames.BuildingName = 6;
            Frame rootFrame = Window.Current.Content as Frame;
            Shell ShellPage = rootFrame.Content as Shell;
            var page = ShellPage.RootFrame;
            if (page.CurrentSourcePageType == typeof(Recommend))
            {
                page.Navigate(typeof(ClassRoomRecommand));
            }
        }
    }
}
