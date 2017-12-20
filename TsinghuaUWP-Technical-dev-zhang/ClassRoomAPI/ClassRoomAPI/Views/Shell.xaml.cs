using ClassRoomAPI.Controls;
using ClassRoomAPI.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System.Profile;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace ClassRoomAPI
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Shell : Page
    {
        private static List<NavMenuItem> navMenuPrimaryItem = new List<NavMenuItem>(
            new[]
            {
                 new NavMenuItem()
                {
                    FontFamily = new FontFamily("Segoe MDL2 Assets"),
                    Icon = "\xE7F6",
                    Label = "发现音乐",
                    Selected = Visibility.Visible,
                    DestPage = typeof(Recommendation)
                },
                new NavMenuItem()
                {
                    FontFamily = new FontFamily("Segoe MDL2 Assets"),
                    Icon = "\xE10F",
                    Label = "校园新闻",
                    Selected = Visibility.Visible,
                    DestPage = typeof(News)
                },
                new NavMenuItem()
                {
                    FontFamily = new FontFamily("Segoe MDL2 Assets"),
                    Icon = "\xE825",
                    Label = "演出信息",
                    Selected = Visibility.Visible,
                    DestPage = typeof(Performance)
                },
                new NavMenuItem()
                {
                    FontFamily = new FontFamily("Segoe MDL2 Assets"),
                    Icon = "\xEC06",
                    Label = "教室信息",
                    Selected = Visibility.Visible,
                    DestPage = typeof(ClassRoomInfo)
                },

                new NavMenuItem()
                {
                    FontFamily = new FontFamily("Segoe MDL2 Assets"),
                    Icon = "\xE772",
                    Label = "作业信息",
                    Selected = Visibility.Visible,
                    DestPage = typeof(WebLearn)
                },

                new NavMenuItem()
                {
                    FontFamily = new FontFamily("Segoe MDL2 Assets"),
                    Icon = "\xE771",
                    Label = "课程信息",
                    Selected = Visibility.Visible,
                    DestPage = typeof(WebLearnTimeTable)
                },

                new NavMenuItem()
                {
                    FontFamily = new FontFamily("Segoe MDL2 Assets"),
                    Icon = "\xE7F4",
                    Label = "IPTV",
                    Selected = Visibility.Visible,
                    //DestPage = typeof(WebLearnTimeTable)
                },

                new NavMenuItem()
                {
                    FontFamily = new FontFamily("Segoe MDL2 Assets"),
                    Icon = "\xE715",
                    Label = "邮件",
                    Selected = Visibility.Visible,
                    //DestPage = typeof(WebLearnTimeTable)
                },

            });

        private static List<NavMenuItem> navMenuSecondaryItem = new List<NavMenuItem>(
            new[]
            {
                new NavMenuItem()
                {
                    FontFamily = new FontFamily("Segoe MDL2 Assets"),
                    Icon = "\xE13D",
                    Label = "登录",
                    Selected = Visibility.Collapsed,
                    DestPage = typeof(LoginPage)
                }
            });

        public Shell()
        {
            this.InitializeComponent();
            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;

            NavigationCacheMode = NavigationCacheMode.Enabled;
            var bb = AnalyticsInfo.VersionInfo.DeviceFamily;
            if (bb == "Windows.Desktop" || bb == "Windows.Tablet")
            {
                var titleBar = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar;
                titleBar.BackgroundColor = Colors.Purple;
                titleBar.ButtonHoverBackgroundColor = Colors.Wheat;
                titleBar.ButtonBackgroundColor = Colors.Purple;
            }
            else
            {
                Debug.WriteLine("[Shell]This version do not support Windows Mobile Any Longer");

            }

           
            // SplitView 开关
            PaneOpenButton.Click += (sender, args) =>
            {
                RootSplitView.IsPaneOpen = !RootSplitView.IsPaneOpen;
            };
            // 导航事件
            NavMenuPrimaryListView.ItemClick += NavMenuListView_ItemClick;
            NavMenuSecondaryListView.ItemClick += NavMenuListView_ItemClick;
            // 默认页

            foreach (var np in navMenuPrimaryItem)
            {
                np.Selected = Visibility.Collapsed;
            }
            foreach (var ns in navMenuSecondaryItem)
            {
                ns.Selected = Visibility.Collapsed;
            }
            RootFrame.SourcePageType = navMenuPrimaryItem[0].DestPage;
            navMenuPrimaryItem[0].Selected = Visibility.Visible;
            NavMenuPrimaryListView.ItemsSource = navMenuPrimaryItem;
            NavMenuSecondaryListView.ItemsSource = navMenuSecondaryItem;
        }
        private void OnBackRequested(object sender, BackRequestedEventArgs e)

        {

            if (!e.Handled && RootFrame.CanGoBack)

            {

                e.Handled = true;
                
               RootFrame.GoBack();

            }
            SyncMenu();

        }

        private void SyncMenu()
        {
            var PrimarySource=NavMenuPrimaryListView.ItemsSource;
            var SecondSource = NavMenuSecondaryListView.ItemsSource;
            var PageType = RootFrame.SourcePageType;

            foreach (var np in navMenuPrimaryItem)
            {
                if(np.DestPage == PageType)
                {
                    np.Selected = Visibility.Visible;
                    TitleTextBlock.Text = np.Label;
                }
                    
                else
                    np.Selected = Visibility.Collapsed;
            }
            foreach (var ns in navMenuSecondaryItem)
            {
                if (ns.DestPage == PageType)
                {
                    ns.Selected = Visibility.Visible;
                    TitleTextBlock.Text = ns.Label;
                }
                else
                    ns.Selected = Visibility.Collapsed;
            }
            NavMenuPrimaryListView.ItemsSource = navMenuPrimaryItem;
            NavMenuSecondaryListView.ItemsSource = navMenuSecondaryItem;
        }

        //UI界面处理函数
        private void NavMenuListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            // 遍历，将选中Rectangle隐藏
            foreach (var np in navMenuPrimaryItem)
            {
                np.Selected = Visibility.Collapsed;
            }
            foreach (var ns in navMenuSecondaryItem)
            {
                ns.Selected = Visibility.Collapsed;
            }

            NavMenuItem item = e.ClickedItem as NavMenuItem;
            // Rectangle显示并导航
            item.Selected = Visibility.Visible;
            TitleTextBlock.Text = item.Label;
            if (item.DestPage != null)
            {
                RootFrame.Navigate(item.DestPage);
            }

            RootSplitView.IsPaneOpen = false;
        }

        private void MainFrame_OnNavigated(object sender, NavigationEventArgs e)
        {

            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =

                    ((Frame)sender).CanGoBack

                        ? AppViewBackButtonVisibility.Visible

                        : AppViewBackButtonVisibility.Collapsed;

        }


        //cortana service
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var storageFile =

              await Windows.Storage.StorageFile.GetFileFromApplicationUriAsync(

                new Uri("ms-appx:///VoiceCommandDictionary.xml"));

            await Windows.ApplicationModel.VoiceCommands.VoiceCommandDefinitionManager

                .InstallCommandDefinitionsFromStorageFileAsync(storageFile);
        }
    }
}
