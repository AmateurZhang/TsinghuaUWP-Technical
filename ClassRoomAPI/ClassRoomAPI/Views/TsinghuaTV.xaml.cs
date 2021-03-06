﻿using ClassRoomAPI.Helpers;
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

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace ClassRoomAPI.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class TsinghuaTV : Page
    {
        public TsinghuaTV()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if(UserHelper.IsDemo())
            {
                TVWeb.Source = new Uri("http://ivi.bupt.edu.cn/");
            }
        }

        public void Page_Refresh()
        {
            if (UserHelper.IsDemo())
            {
                TVWeb.Source = new Uri("http://ivi.bupt.edu.cn/");
            }
            else
            {
                TVWeb.Source = new Uri("https://iptv.tsinghua.edu.cn/");
            }
        }
    }
}
