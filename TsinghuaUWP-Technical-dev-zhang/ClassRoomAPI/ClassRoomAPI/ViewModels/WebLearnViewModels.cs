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
    public class WebLearnViewModels
    {
        private static DateTime TimeAllWebLearnInfoLogin = DateTime.MinValue;
        private static int ALL_WEBLEARN_LOGIN_TIMEOUT_MINUTES = 1;

        public static async Task LoginInToWebLearnViewModel(WebLearnLoginMode Mode)
        {
            await WebLearnAPIService.LogintoWebLearnMode(Mode);
        }

        public static async Task<int> LoginInToWebLearnUsingPassword(string username = "", string password = "")
        {
            return await WebLearnAPIService.LogintoWebLearnAsync(username , password );
        }
        public static async Task<WebLearnInfo> GetAllWebLearnViewModel(ParseDataMode Mode = ParseDataMode.Remote)
        {
            if (Mode == ParseDataMode.Local)
            {
                try
                {
                    return await WebLearnAPIService.GetAllWebLearnInfoMode(ParseDataMode.Local);
                }
                catch
                {
                    Debug.WriteLine("[GetAllWebLearnViewModel] return local data fails.");
                    var _Exception = new NumberException(ExceptionCodeWebLearnData.EXCEPTION_RETURN_LOCAL_DATA_FAILED);
                    throw _Exception;
                }
            }
            else if (Mode == ParseDataMode.Remote)
            {
                if ((DateTime.Now - TimeAllWebLearnInfoLogin).TotalMinutes < ALL_WEBLEARN_LOGIN_TIMEOUT_MINUTES)
                {
                    return await WebLearnAPIService.GetAllWebLearnInfoMode(ParseDataMode.Local);
                }

                try
                {
                    Debug.WriteLine("[GetAllWebLearnViewModel] return remote data.");
                    var _ReturnData = await WebLearnAPIService.GetAllWebLearnInfoMode(ParseDataMode.Remote);
                    TimeAllWebLearnInfoLogin = DateTime.Now;
                    return _ReturnData;
                }
                catch
                {

                    Debug.WriteLine("[GetAllWebLearnViewModel] return remote data fails.");
                    var _Exception = new NumberException(ExceptionCodeWebLearnData.EXCEPTION_RETURN_REMOTE_DATA_FAILED);
                    throw _Exception;

                }
            }
            else
            {
                //demo
                var JSONString = Test.WebLearnTest.JSONAllWebLearnData;
                var Result = JSONHelper.Parse<WebLearnInfo>(JSONString);
                return Result;
            }
        }
    }
}
