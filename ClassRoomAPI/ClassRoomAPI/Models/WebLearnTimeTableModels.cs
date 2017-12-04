using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassRoomAPI.Models
{
    public class WebLearnTimeTable
    {
        public DateTime Date;
        public List<Windows.ApplicationModel.Appointments.Appointment> ListAppointment;
    }
}
