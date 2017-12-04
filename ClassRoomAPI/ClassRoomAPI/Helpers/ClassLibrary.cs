using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Web.Http;

namespace ClassRoomAPI
{
    public class ClassLibrary
    {

       
        public static TimeSpan UnixTime()
        {
            return (DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)));
        }

        public static async Task WaitTask(double seconds = 1)
        {
            await Task.Delay(TimeSpan.FromSeconds(seconds));
        }

        
    }
}
