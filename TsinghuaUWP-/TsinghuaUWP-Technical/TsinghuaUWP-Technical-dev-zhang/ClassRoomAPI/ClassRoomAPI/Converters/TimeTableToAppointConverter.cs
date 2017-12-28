using ClassRoomAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassRoomAPI.Converters
{
    public class TimeTableToAppointConverter
    {
        public static Windows.ApplicationModel.Appointments.Appointment GetAppointment(Event e)
        {
            var a = new Windows.ApplicationModel.Appointments.Appointment();
            a.Subject = e.nr;
            a.Location = e.dd;

            a.StartTime = DateTime.Parse(e.nq + " " + e.kssj);
            a.Duration = DateTime.Parse(e.nq + " " + e.jssj) - a.StartTime;
            // 修正考试时间 12 小时制
            if (e.fl == "考试")
            {
                if (a.StartTime.Hour < 8)
                {
                    a.StartTime += TimeSpan.FromHours(12);
                }
                a.Subject += "考试";
            }
            a.AllDay = false;
            return a;
        }
    }

}
