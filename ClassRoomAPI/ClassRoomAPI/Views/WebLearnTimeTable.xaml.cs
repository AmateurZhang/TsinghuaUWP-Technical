﻿using ClassRoomAPI.Controls;
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
    public sealed partial class WebLearnTimeTable : Page
    {
        public WebLearnTimeTable()
        {
            this.InitializeComponent();
        }

       

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ImageBrush imageBrush = new ImageBrush();
            imageBrush.ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/BuildingFive.png", UriKind.Absolute));
            WebLearnTimeTablePage.Background = imageBrush;
            if (UserHelper.CredentialAbsent())
            {
                var notifyPopup = new NotifyPopup("未登录！");
                notifyPopup.Show();
            }
            else if(UserHelper.IsDemo())
            {
                var _Data = await WebLearnTimeTableViewModel.GetTimeTableViewModel(ParseDataMode.Demo);
                MainListView.ItemsSource = _Data.ListAppointment;
                return;
            }
            else
            {
                try
                {
                    var _Data = await WebLearnTimeTableViewModel.GetTimeTableViewModel(ParseDataMode.Local);
                    MainListView.ItemsSource = _Data.ListAppointment;
                    if(_Data.ListAppointment!=null)
                    {
                        int SelectedItem = 0;
                        foreach (var item in _Data.ListAppointment)
                        {
                            if ((item.StartTime - DateTime.Now).Minutes >= 0)
                            {
                                SelectedItem += 1;
                            }
                        }
                        if (SelectedItem > 0)
                        {
                            SelectedItem -= 1;
                        }
                        else
                        {
                            SelectedItem = 0;
                        }
                        MainListView.SelectedIndex = SelectedItem;
                    }
                    
                    var notifyPopup = new NotifyPopup("正在使用本地数据。");
                    notifyPopup.Show();

                    if ((DateTime.Now - _Data.Date).Minutes > 5)
                        throw new Exception("The Data are out-of-date.");
                   
                }
                catch
                {
                    try
                    {
                        var _DataRemote = await WebLearnTimeTableViewModel.GetTimeTableViewModel(ParseDataMode.Remote);
                        MainListView.ItemsSource = _DataRemote.ListAppointment;
                        if (_DataRemote.ListAppointment != null)
                        {
                            int SelectedItem = 0;
                            foreach (var item in _DataRemote.ListAppointment)
                            {
                                if ((item.StartTime - DateTime.Now).Minutes >= 0)
                                {
                                    SelectedItem += 1;
                                }
                            }
                            if (SelectedItem > 0)
                            {
                                SelectedItem -= 1;
                            }
                            else
                            {
                                SelectedItem = 0;
                            }
                            MainListView.SelectedIndex = SelectedItem;
                        }
                        var notifyPopup = new NotifyPopup("数据已刷新！");
                        notifyPopup.Show();
                    }
                    catch
                    {
                        var notifyPopup = new NotifyPopup("网络错误，请检查网络。");
                        notifyPopup.Show();
                    }

                }
            }
            
        }
    }
}
