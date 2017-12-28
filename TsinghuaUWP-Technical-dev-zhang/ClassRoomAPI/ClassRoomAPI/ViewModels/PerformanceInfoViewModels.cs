using ClassRoomAPI.Helpers;
using ClassRoomAPI.Models;
using ClassRoomAPI.Services;
using ClassRoomAPI.Test;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassRoomAPI.ViewModels
{
    public class PerformaceViewModels
    {
        private static DateTime TimeBuildingTypeNamesLastLogin = DateTime.MinValue;
        private static int BUILDING_NAMES_LOGIN_TIMEOUT_MINUTES = 1;
        private static DateTime TimeAllBuildingInfoLogin = DateTime.MinValue;
        private static int ALL_BUILDING_INFO_LOGIN_TIMEOUT_MINUTES = 1;

        public static async Task<ShowInfoData> GetHallInfoViewModel(ParseDataMode Mode = ParseDataMode.Remote)
        {
            if (Mode == ParseDataMode.Local)
            {
                try
                {
                    return await PerformanceAPI.PerformanceInfo.GetListAllShowInfoMode(ParseDataMode.Local);
                }
                catch
                {
                    Debug.WriteLine("[GetHallViewModel] return local data fails.");
                    var _Exception = new NumberException(ExceptionCodeClassRoomInfo.EXCEPTION_RETURN_LOCAL_DATA_FAILED);
                    throw _Exception;
                }
            }
            else if (Mode == ParseDataMode.Remote)
            {
                if ((DateTime.Now - TimeBuildingTypeNamesLastLogin).TotalMinutes < BUILDING_NAMES_LOGIN_TIMEOUT_MINUTES)
                {
                    return await PerformanceAPI.PerformanceInfo.GetListAllShowInfoMode(ParseDataMode.Local);
                }

                try
                {
                    Debug.WriteLine("[GetBuildingNamesViewModel] return remote data.");
                    var _ReturnData = await PerformanceAPI.PerformanceInfo.GetListAllShowInfoMode(ParseDataMode.Remote);
                    TimeBuildingTypeNamesLastLogin = DateTime.Now;
                    return _ReturnData;
                }
                catch
                {

                    Debug.WriteLine("[GetBuildingNamesViewModel] return remote data fails.");
                    var _Exception = new NumberException(ExceptionCodeClassRoomInfo.EXCEPTION_RETURN_REMOTE_DATA_FAILED);
                    throw _Exception;

                }
            }
            else
            {
                var JSONString = Test.PerformaceTest.jsonAllPerformance;
                var Result = JSONHelper.Parse<ShowInfoData>(JSONString);
                return Result;
            }
        }

       
    }
}
