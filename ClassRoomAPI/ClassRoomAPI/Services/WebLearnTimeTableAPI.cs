using ClassRoomAPI.Converters;
using ClassRoomAPI.Helpers;
using ClassRoomAPI.Models;
using ClassRoomAPI.Test;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassRoomAPI.Services
{
    public class WebLearnTimeTableAPI
    {
        //Consts
        private static string CacheTimeTableJSON = "JSONTimeTableType";

        //Public Classes
        public static async Task<WebLearnTimeTable> GetTimeTableMode(ParseDataMode Mode = ParseDataMode.Remote)//Remote, Local, Demo
        {
            if (Mode == ParseDataMode.Local)
            {
                await WebLearnAPIService.LogintoWebLearnMode(WebLearnLoginMode.Local);
                var TempData = await CacheHelper.ReadCache(CacheTimeTableJSON);
                var ReturnData = JSONHelper.Parse<WebLearnTimeTable>(TempData);
                Debug.WriteLine("[GetTimeTableMode] return local data.");
                return ReturnData;

            }
            else if (Mode == ParseDataMode.Remote)
            {
                await WebLearnAPIService.LogintoWebLearnMode(WebLearnLoginMode.Remote);
                await LoginInToTimeTableAddOnMode(WebLearnLoginMode.Remote);
                Debug.WriteLine("[GetBuildingTypeMode] return remote data.");
                return await GetTimeTableAsync();

            }
            else
            {
                //demo
                var TempData = UWPTest.JSONTimeTableType;
                var ReturnData = JSONHelper.Parse<WebLearnTimeTable>(TempData);
                return ReturnData;
            }

        }

        //Private Classes
        private static async Task LoginInToTimeTableAddOnMode(WebLearnLoginMode Mode = WebLearnLoginMode.Remote)
        {
            if (Mode == WebLearnLoginMode.Local)
            {
                Debug.WriteLine("[LoginInToTimeTable] Reuse Session.");
            }
            else if (Mode == WebLearnLoginMode.Remote)
            {
                var ticket = await PostGetHelper.POST(
                 "http://learn.cic.tsinghua.edu.cn:80/gnt",
                 "appId=ALL_ZHJW");
                await ClassLibrary.WaitTask();
                try
                {
                    var zhjw = await PostGetHelper.GET(
                        $"http://zhjw.cic.tsinghua.edu.cn/j_acegi_login.do?url=/&ticket={ticket}");
                }
                catch (System.Runtime.InteropServices.COMException e)
                {
                    if (e.Message.IndexOf("403") == -1)
                        throw e;
                    Debug.WriteLine("[getRemoteTimetable] outside campus network");

                }
            }
            else if (Mode == WebLearnLoginMode.Anonymous)
            {
                Debug.WriteLine("[LoginInToTimeTable] Anonymous.");
            }
            else
            {
                //demo
            }
        }
        private static async Task<WebLearnTimeTable> GetTimeTableAsync()
        {
            Timetable timetable = new Timetable();
            for (int i = -6; i <= 4; i += 2)
            {
                string page;
                try
                {
                    page = await GetCalendarPage(
                        DateTime.Now.AddMonths(i).AddDays(1).ToString("yyyyMMdd"),
                        DateTime.Now.AddMonths(i + 2).ToString("yyyyMMdd")
                        );
                    
                }
                catch (Exception)
                {
                    page = await GetCalendarPage(
                        DateTime.Now.AddMonths(i).AddDays(1).ToString("yyyyMMdd"),
                        DateTime.Now.AddMonths(i + 2).ToString("yyyyMMdd")
                        );
                }
                var set_to_be_appended = ParseTimetablePage(page);
                foreach (var _____ in set_to_be_appended)
                {
                    timetable.Add(_____);
                }
            }
            var _Return = new List<Windows.ApplicationModel.Appointments.Appointment>();
            foreach (var item in timetable)
            {
                var _Appointment = TimeTableToAppointConverter.GetAppointment(item);
                _Return.Add(_Appointment);
            }
            _Return.Reverse();

            var _ReturnData = new WebLearnTimeTable();
            _ReturnData.Date = DateTime.Now;
            _ReturnData.ListAppointment = new List<Windows.ApplicationModel.Appointments.Appointment>();
            _ReturnData.ListAppointment = _Return;

            Debug.WriteLine("[GetRemoteTimetable] returning direct");
            var StringfiedData = JSONHelper.Stringify(_ReturnData);
            await CacheHelper.WriteCache(CacheTimeTableJSON, StringfiedData);
            return _ReturnData;
        }

        private static Timetable ParseTimetablePage(string page)
        {
            if (page.Length < "_([])".Length)
                throw new MessageException("timetable_javascript");
            string json = page.Substring(2, page.Length - 3);
            return JSONHelper.Parse<Timetable>(json);
        }

        private static async Task<string> GetCalendarPage(string starting_date, string ending_date)
        {
            Debug.WriteLine($"[get_calendar_page] {starting_date}-{ending_date}");
            var stamp = (long)ClassLibrary.UnixTime().TotalMilliseconds;
            return await PostGetHelper.GET(
                $"http://zhjw.cic.tsinghua.edu.cn/jxmh.do?m=bks_jxrl_all&p_start_date={starting_date}&p_end_date={ending_date}&jsoncallback=_&_={stamp}");
        }
    }
    
}
