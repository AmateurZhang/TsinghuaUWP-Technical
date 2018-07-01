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
using ClassRoomAPI.Controls;
using ClassRoomAPI.Helpers;
using ClassRoomAPI.Models;
using ClassRoomAPI.Services;
using ClassRoomAPI.ViewModels;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace ClassRoomAPI.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class CourseNext : Page
    {

        //public List<> 
        public CourseNext()
        {
            this.InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var time = DateTime.Now;
            if (UserHelper.CredentialAbsent())
            {
                var notifyPopup = new NotifyPopup("未登录！");
                notifyPopup.Show();
            }
            else if(UserHelper.IsDemo())
            {
                var None = new List<Windows.ApplicationModel.Appointments.Appointment>();
                None.Add(new Windows.ApplicationModel.Appointments.Appointment
                {
                    Subject = "最近没有要上的课程",
                    Location = "Have fun!",
                    StartTime = DateTime.Now,

                });
                MainListView.ItemsSource = None;
            }
            else
            {
                var _Data = await WebLearnTimeTableViewModel.GetTimeTableViewModel(ParseDataMode.Local);
                var Now = DateTime.Now;
                if (_Data != null)
                {
                    //MainListView.ItemsSource = _Data.ListAppointment;
                    if (_Data.ListAppointment != null)
                    {
                        var AllCourses = new List<Windows.ApplicationModel.Appointments.Appointment>();
                        //筛选出没上的课
                        foreach (var Items in _Data.ListAppointment)
                        {
                            //计算课程时间与当前时间的时间差
                            var distance = (Items.StartTime+Items.Duration - Now).Minutes;
                            //未上的课
                            if (distance >= 0)
                            {
                                AllCourses.Add(new Windows.ApplicationModel.Appointments.Appointment
                                {
                                    Subject = Items.Subject,
                                    Location = Items.Location,
                                    StartTime = Items.StartTime,
                                    Duration = Items.Duration
                                });

                            }
                        }
                        var Courses = new List<Windows.ApplicationModel.Appointments.Appointment>();
                        var count = AllCourses.Count;
                        //从没上的课里面筛选出4门距离现在最近的课
                        if (count >= 4)
                        {
                            for (int i = count - 1; i > (count - 5); i--)
                            {
                                Courses.Add(new Windows.ApplicationModel.Appointments.Appointment
                                {
                                    Subject = AllCourses[i].Subject,
                                    Location = AllCourses[i].Location,
                                    StartTime = AllCourses[i].StartTime,
                                    Duration = AllCourses[i].Duration

                                });
                            }
                            MainListView.ItemsSource = Courses;
                        }
                        //若所剩课程少于三门就全部列举
                        else if (count > 0)
                        {
                            MainListView.ItemsSource = AllCourses;
                        }
                        //若没有所剩课程就提示用户已经没有课程
                        else
                        {
                            var None = new List<Windows.ApplicationModel.Appointments.Appointment>();
                            None.Add(new Windows.ApplicationModel.Appointments.Appointment
                            {
                                Subject = "最近没有要上的课程",
                                Location = "Have fun!",
                                StartTime = Now,

                            });
                            MainListView.ItemsSource = None;
                        }
                    }
                }
                else
                {
                    var None = new List<Windows.ApplicationModel.Appointments.Appointment>();
                    None.Add(new Windows.ApplicationModel.Appointments.Appointment
                    {
                        Subject = "无数据",
                        Location = "N/A",
                        StartTime = Now,

                    });
                    MainListView.ItemsSource = None;
                }
            }
        }
    }
}
