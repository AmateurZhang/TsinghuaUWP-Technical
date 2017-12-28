using ClassRoomAPI.Helpers;
using ClassRoomAPI.Models;
using ClassRoomAPI.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassRoomAPI.ViewModels
{
    public class WebLearnTimeTableViewModel
    {
        private static DateTime TimeTimeTableLastLogin = DateTime.MinValue;
        private static int TIMETABLE_LOGIN_TIMEOUT_MINUTES = 1;

        public static async Task<WebLearnTimeTable> GetTimeTableViewModel(ParseDataMode Mode = ParseDataMode.Remote)
        {
            if (Mode == ParseDataMode.Local)
            {
                try
                {

                    return await WebLearnTimeTableAPI.GetTimeTableMode(ParseDataMode.Local);
                }
                catch
                {
                    Debug.WriteLine("[GetTimeTableViewModel] return local data fails.");
                    var _Exception = new NumberException(ExceptionCodeClassRoomInfo.EXCEPTION_RETURN_LOCAL_DATA_FAILED);
                    throw _Exception;
                }
            }
            else if (Mode == ParseDataMode.Remote)
            {
                if ((DateTime.Now - TimeTimeTableLastLogin).TotalMinutes < TIMETABLE_LOGIN_TIMEOUT_MINUTES)
                {
                    return await WebLearnTimeTableAPI.GetTimeTableMode(ParseDataMode.Local);
                }

                try
                {
                    Debug.WriteLine("[GetTimeTableViewModel] return remote data.");
                    var _ReturnData = await WebLearnTimeTableAPI.GetTimeTableMode(ParseDataMode.Remote);
                    TimeTimeTableLastLogin = DateTime.Now;
                    return _ReturnData;
                }
                catch
                {

                    Debug.WriteLine("[GetTimeTableViewModel] return remote data fails.");
                    var _Exception = new NumberException(ExceptionCodeClassRoomInfo.EXCEPTION_RETURN_REMOTE_DATA_FAILED);
                    throw _Exception;

                }
            }
            else
            {
                //demo
                var JSONString = Test.WebLearnTimetableTest.JSONAllDDL;
                var Result = JSONHelper.Parse<WebLearnTimeTable>(JSONString);
                return Result;
            }
        }
    }
}
