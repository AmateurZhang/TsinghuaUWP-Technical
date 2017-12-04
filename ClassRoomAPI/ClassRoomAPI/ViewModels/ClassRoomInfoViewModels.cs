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
    public class ClassRoomInfoViewModels
    {
        private static DateTime TimeBuildingTypeNamesLastLogin = DateTime.MinValue;
        private static int BUILDING_NAMES_LOGIN_TIMEOUT_MINUTES = 1;
        private static DateTime TimeAllBuildingInfoLogin = DateTime.MinValue;
        private static int ALL_BUILDING_INFO_LOGIN_TIMEOUT_MINUTES = 1;

        public static async Task<List<BuildingTypeNamesData>> GetBuildingNamesViewModel(ParseDataMode Mode = ParseDataMode.Remote)
        {
            if (Mode == ParseDataMode.Local)
            {
                try
                {
                    return await ClassRoomAPIService.ClassParseBuildingInfo.GetBuildingTypeMode(ParseDataMode.Local);
                }
                catch
                {
                    Debug.WriteLine("[GetBuildingNamesViewModel] return local data fails.");
                    var _Exception = new NumberException(ExceptionCodeClassRoomInfo.EXCEPTION_RETURN_LOCAL_DATA_FAILED);
                    throw _Exception;
                }
            }
            else if (Mode == ParseDataMode.Remote)
            {
                if((DateTime.Now - TimeBuildingTypeNamesLastLogin).TotalMinutes< BUILDING_NAMES_LOGIN_TIMEOUT_MINUTES)
                {
                    return await ClassRoomAPIService.ClassParseBuildingInfo.GetBuildingTypeMode(ParseDataMode.Local);
                }

                try
                {
                    Debug.WriteLine("[GetBuildingNamesViewModel] return remote data.");
                    var _ReturnData= await ClassRoomAPIService.ClassParseBuildingInfo.GetBuildingTypeMode(ParseDataMode.Remote);
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
                //demo
                return null;
            }
        }

        public static async Task<ClassBuildingInfo> GetAllBuildingInfoViewModel(ParseDataMode Mode = ParseDataMode.Remote)
        {
            if (Mode == ParseDataMode.Local)
            {
                try
                {
                    return await ClassRoomAPIService.ClassParseBuildingInfo.GetListAllBuildingInfoMode(ParseDataMode.Local);
                }
                catch
                {
                    Debug.WriteLine("[GetBuildingNamesViewModel] return local data fails.");
                    var _Exception = new NumberException(ExceptionCodeClassRoomInfo.EXCEPTION_RETURN_LOCAL_DATA_FAILED);
                    throw _Exception;
                }
            }
            else if (Mode == ParseDataMode.Remote)
            {
                if ((DateTime.Now - TimeAllBuildingInfoLogin).TotalMinutes < ALL_BUILDING_INFO_LOGIN_TIMEOUT_MINUTES)
                {
                    return await ClassRoomAPIService.ClassParseBuildingInfo.GetListAllBuildingInfoMode(ParseDataMode.Local);
                }

                try
                {
                    Debug.WriteLine("[GetBuildingNamesViewModel] return remote data.");
                    var _ReturnData = await ClassRoomAPIService.ClassParseBuildingInfo.GetListAllBuildingInfoMode(ParseDataMode.Remote);
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
                //demo
                var JSONString = JSONStringClassRoomInfo.jsonAllClassRoomInfoData;
                var Result = JSONHelper.Parse<ClassBuildingInfo>(JSONString);
                return Result;
            }
        }
    }
}
