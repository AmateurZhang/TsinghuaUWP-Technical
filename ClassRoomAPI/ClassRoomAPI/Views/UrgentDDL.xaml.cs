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
using ClassRoomAPI.Models;
using ClassRoomAPI.Controls;

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
        
        private List<Deadline> Ur_DDL= new List<Deadline>();
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var _Data = await WebLearnViewModels.GetAllWebLearnViewModel(ParseDataMode.Local);

            
            if (_Data != null)
            {
               
                //var Dead_line = new List<>;
                var Dead_line = _Data.ListCourseInfoDetail;
                //没写完的作业
                for (int i = 0; i < Dead_line.Count; i++)
                {
                    if (Dead_line[i].Deadlines.Count != 0)
                    {
                        for (int j = 0; j < Dead_line[i].Deadlines.Count; j++)
                        {
                            //if ((Dead_line[i].Deadlines[j].hasBeenFinished == false)
                            if((Dead_line[i].Deadlines[j].hasBeenFinished == false)&&
                               (Dead_line[i].Deadlines[j].isPast() == false)
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

                //写完的作业
                var Ur_DDL_Finished = new List<Deadline>();
                for (int i = 0; i < Dead_line.Count; i++)
                {
                    if (Dead_line[i].Deadlines.Count != 0)
                    {
                        for (int j = 0; j < Dead_line[i].Deadlines.Count; j++)
                        {
                            if ((Dead_line[i].Deadlines[j].hasBeenFinished == true)&&
                               (Dead_line[i].Deadlines[j].isPast() == false)
                                && (Dead_line[i].Deadlines[j].shouldBeIgnored() == false)
                                )
                            {
                                var temp = Dead_line[i].Deadlines[j];
                                Ur_DDL_Finished.Add(temp);
                            }
                        }
                    }
                }

                Ur_DDL_Finished.Sort(new Icp_DDL());

                Ur_DDL.AddRange(Ur_DDL_Finished);
                ListViewDDLData.ItemsSource = Ur_DDL;
            }
            else {
                var notifyPopup = new NotifyPopup("请先登录！");
                notifyPopup.Show();
            }
        }

        private void ListViewDDLData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int Selectedindex = ListViewDDLData.SelectedIndex;
            var Selecteditem = Ur_DDL[Selectedindex];
            Frame rootFrame = Window.Current.Content as Frame;
            Shell ShellPage = rootFrame.Content as Shell;
            var page = ShellPage.RootFrame;
            page.Navigate(typeof(DDLdetail),Selecteditem);
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
