﻿using ClassRoomAPI.Controls;
using ClassRoomAPI.Models;
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
using Windows.UI.Xaml.Navigation;


// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace ClassRoomAPI.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class UrgentDDL : Page
    {
        public UrgentDDL()
        {
            this.InitializeComponent();
        }
        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }
        private List<Deadline> DDL;
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var _Data = await WebLearnViewModels.GetAllWebLearnViewModel(ParseDataMode.Local);
            var Ur_DDL = new List<Deadline>();
            var Dead_line = _Data.ListCourseInfoDetail;
            for (int i = 0; i < Dead_line.Count; i++) {
                if (Dead_line[i].Deadlines.Count!=0) {
                    for (int j = 0; j < Dead_line[i].Deadlines.Count; j++) {
                        if ((Dead_line[i].Deadlines[j].hasBeenFinished == false)
                            && (Dead_line[i].Deadlines[j].isPast() == false)
                            && (Dead_line[i].Deadlines[j].shouldBeIgnored() == false)
                            )
                        {
                            var temp = Dead_line[i].Deadlines[j];
                            Ur_DDL.Add(temp);
                        }
                    }
                }
            }
            Ur_DDL.Sort(new Icp_DDL());
            ListViewDDLData.ItemsSource = Ur_DDL;
        }
    }

    public class Icp_DDL : IComparer<Deadline>
    {
        public int Compare(Deadline x, Deadline y)
        {
            return x.ddl.CompareTo(y.ddl);
        }
    }


}
