using ClassRoomAPI.Helpers;
using ClassRoomAPI.Models;
using ClassRoomAPI.Test;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ClassRoomAPI.Services
{
    class PerformanceAPI
    {
        public static class PerformanceInfo
        {
            //Define Cache Names

            private static string CacheAllShowInfoDataJSON = "JSONAllShowInfoData";

            //获取新清华学堂所有类型所有演出信息
            public static async Task<ShowInfoData> GetListAllShowInfoMode(ParseDataMode Mode = ParseDataMode.Remote)//Remote, Local, Demo
            {
                if (Mode == ParseDataMode.Local)
                {
                    try
                    {
                        var TempData = await CacheHelper.ReadCache(CacheAllShowInfoDataJSON);
                        var ReturnData = JSONHelper.Parse<ShowInfoData>(TempData);
                        Debug.WriteLine("[GetListAllShowInfoMode] return local data.");
                        return ReturnData;
                    }
                    catch
                    {
                        Debug.WriteLine("[GetListAllShowInfoMode] return local data fails.");
                        //此处异常类型需要修改
                        var _Exception = new NumberException(ExceptionCodeClassRoomInfo.EXCEPTION_RETURN_LOCAL_DATA_FAILED);
                        throw _Exception;
                    }
                }
                else if (Mode == ParseDataMode.Remote)
                {
                    try
                    {
                        Debug.WriteLine("[GetListAllShowInfoMode] return remote data.");
                        return await GetListAllHallInfoAsync();
                    }
                    catch
                    {
                        Debug.WriteLine("[GetListAllShowInfoMode] return remote data fails.");
                        //此处异常类型需要修改
                        var _Exception = new NumberException(ExceptionCodeClassRoomInfo.EXCEPTION_RETURN_REMOTE_DATA_FAILED);
                        throw _Exception;
                    }
                }
                else
                {
                    //demo
                    var TempData = UWPTest.JSONAllShowInfoData;
                    var ReturnData = JSONHelper.Parse<ShowInfoData>(TempData);
                    Debug.WriteLine("[GetListAllShowInfoMode] return local data.");
                    return ReturnData;
                }
            }

            //获取所有演出类别信息
            private static List<PerformanceType> GetShowType()
            {
                var Data = new List<PerformanceType>();
                Data.Add(new PerformanceType { Type =  "电影" });
                Data.Add(new PerformanceType { Type = "演出" });
                Data.Add(new PerformanceType { Type = "讲座" });
                Data.Add(new PerformanceType { Type = "艺术丛林" });
                return Data;
            }
            //获取每个类别对应的所有项目信息
            private static async Task<List<PerformanceData>> GetListShowInfoAsync(PerformanceType type)
            {
                //load进第一页的演出信息
                var URL="";
                if (type.Type == "电影") URL = "dy/";
                if (type.Type == "演出") URL = "yc/";
                if (type.Type == "讲座") URL = "jz/";
                if (type.Type == "艺术丛林") URL = "zl/";

                string html = "http://www.hall.tsinghua.edu.cn/columnEx/pwzx_hdap/" + URL + "1";
                HtmlWeb web = new HtmlWeb();
                var htmlDoc = web.Load(html);

                var htmlNodes = htmlDoc.DocumentNode.SelectNodes("/html/body/div[2]/div/div[2]/div/div");//timelist
                var InnerTest = htmlNodes[0].InnerHtml;
                Regex.Replace(InnerTest, "::after", "");//Remove after using System.Text.RegularExpressions;
                var doc = new HtmlDocument();
                doc.LoadHtml(InnerTest);
                var ListNodes = doc.DocumentNode.SelectNodes("/div");
                
                var Data = new List<PerformanceData>();

                for (int i = 1; i < ListNodes.Count; i++)
                {
                    string PerDay = ListNodes[i].ChildNodes[1].ChildNodes[3].ChildNodes[1].ChildNodes[0].InnerText; //演出的日
                    string PerDate = ListNodes[i].ChildNodes[1].ChildNodes[3].ChildNodes[1].ChildNodes[1].InnerText; //演出的年/月
                    string PerHour = ListNodes[i].ChildNodes[1].ChildNodes[3].ChildNodes[1].ChildNodes[3].InnerText;//演出在几点

                    PerHour = PerHour.Replace(" ", "");
                    string PerTime = PerDate + "-" + PerDay + "    " + PerHour;
                    PerTime = PerTime.Replace("\r", "");
                    PerTime = PerTime.Replace("\n", "");
                    PerTime = PerTime.Replace("\t", "");
                    string PerName = ListNodes[i].ChildNodes[1].ChildNodes[5].ChildNodes[1].InnerText;
                    string PerAddress = ListNodes[i].ChildNodes[1].ChildNodes[5].ChildNodes[5].InnerText;
                    PerAddress = PerAddress.Replace("\r", "");
                    PerAddress = PerAddress.Replace("\n", "");
                    PerAddress = PerAddress.Replace("\t", "");
                    string PerState = ListNodes[i].ChildNodes[1].ChildNodes[5].ChildNodes[7].InnerText;
                    PerState = PerState.Replace("\r", "");
                    PerState = PerState.Replace("\n", "");
                    PerState = PerState.Replace("\t", "");
                    PerState = PerState.Replace(" ", "");
                    if ((PerState != "结束")&& (PerState != "售罄") && (PerState != "已售罄") )
                    {
                        type.isEmpty = false;
                        Data.Add(new PerformanceData
                        {
                            PerformanceTime = PerTime,
                            PerformanceName = PerName,
                            PerformanceAddress = PerAddress,
                            PerformanceState = PerState
                        }
                        );
                    }
                }
                
                var StringfiedData = JSONHelper.Stringify(Data);
                await CacheHelper.WriteCache($"JSONAllHallInfoData_{type.Type+type.isEmpty}", StringfiedData);
                return Data;
            }
            //获取所有类型的演出信息，在不同的枢轴显示出来
            private static async Task<ShowInfoData> GetListAllHallInfoAsync()
            {
                //获取所有演出类型
                var _ShowType = GetShowType();
                //_ShowInfo.ListPerformanceTypeInfo = _ShowType;
                var _ShowInfoData = new ShowInfoData();
                _ShowInfoData.ListShowInfo = new List<ShowInfo>();
                

                foreach (PerformanceType item in _ShowType)
                {
                    //获取时间信息
                    _ShowInfoData.Date = DateTime.Now;
                    var _ListShowInfo = new ShowInfo();
                    //存储演出类型信息名字
                    _ListShowInfo.PerformanceType = item.Type;
                    //获取该类型演出的所有信息
                    var _ListShowInfoData = await GetListShowInfoAsync(item);
                    
                    //将获取到的演出信息传入models里面
                    _ListShowInfo.ListPerformanceInfo=_ListShowInfoData;
                    _ShowInfoData.ListShowInfo.Add(_ListShowInfo);

                }
                var StringfiedData = JSONHelper.Stringify(_ShowInfoData);
                await CacheHelper.WriteCache(CacheAllShowInfoDataJSON, StringfiedData);
                return _ShowInfoData;
            }
            
        }
    }
}
