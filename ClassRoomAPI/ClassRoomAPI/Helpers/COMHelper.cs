using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassRoomAPI.Helpers
{
    public class COMHelper
    {
        //网络环境
        public static async Task<bool> OutOfCampus()
        {
            bool outside_campus_network = false;
            string aca = "https://academic.tsinghua.edu.cn";
            try
            {
                var result = await PostGetHelper.GET(aca);
            }
            catch
            {
                outside_campus_network = true;
            }
            return outside_campus_network;
        }
    }
}
