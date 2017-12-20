using ClassRoomAPI.Helpers;
using ClassRoomAPI.Models;
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
    public class ClassRoomAPIService
    {

        public static class ClassParseBuildingInfo
        {
            //Define Cache Names
            private static string CacheBuildingTypeJSON = "JSONBuildingType";
            private static string CacheAllClassRoomInfoDataJSON = "JSONAllClassRoomInfoData";

            //Public Classes
            public static async Task<List<BuildingTypeNamesData>> GetBuildingTypeMode(ParseDataMode Mode= ParseDataMode.Remote)//Remote, Local, Demo
            {
                if(Mode==ParseDataMode.Local)
                {
                     var TempData = await CacheHelper.ReadCache(CacheBuildingTypeJSON);
                     var ReturnData = JSONHelper.Parse<List<BuildingTypeNamesData>>(TempData);
                     Debug.WriteLine("[GetBuildingTypeMode] return local data.");
                     return ReturnData;

                }
                else if(Mode== ParseDataMode.Remote)
                {
                     Debug.WriteLine("[GetBuildingTypeMode] return remote data.");
                     return await GetBuildingTypeAsync();                       

                }
                else
                {
                    //demo
                    return null;
                }
                
            }
            public static async Task<ClassBuildingInfo> GetListAllBuildingInfoMode(ParseDataMode Mode = ParseDataMode.Remote)//Remote, Local, Demo
            {
                if (Mode == ParseDataMode.Local)
                {
                    try
                    {
                        var TempData = await CacheHelper.ReadCache(CacheAllClassRoomInfoDataJSON);
                        var ReturnData = JSONHelper.Parse<ClassBuildingInfo>(TempData);
                        Debug.WriteLine("[GetListAllBuildingInfoMode] return local data.");
                        return ReturnData;
                    }
                    catch
                    {
                        Debug.WriteLine("[GetListAllBuildingInfoMode] return local data fails.");
                        var _Exception = new NumberException(ExceptionCodeClassRoomInfo.EXCEPTION_RETURN_LOCAL_DATA_FAILED);
                        throw _Exception;
                    }
                }
                else if (Mode == ParseDataMode.Remote)
                {
                    try
                    {
                        Debug.WriteLine("[GetListAllBuildingInfoMode] return remote data.");
                        return await GetListAllBuildingInfoAsync();
                    }
                    catch
                    {
                        Debug.WriteLine("[GetListAllBuildingInfoMode] return remote data fails.");
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

            //Private Classes
            private static async Task<List<BuildingTypeNamesData>> GetBuildingTypeAsync()
            {
                var Data = new List<BuildingTypeNamesData>();
                string html = "http://jxgl.cic.tsinghua.edu.cn/jxpg/f/wxjwxs/jsxx?menu=false";
                HtmlWeb web = new HtmlWeb();
                var htmlDoc = web.Load(html);
                var htmlNodes = htmlDoc.DocumentNode.SelectNodes("//html/body/div/div/div[@class='list-block list-class']/div/ul/li");

                for (int i = 1; i < htmlNodes.Count; i++)
                {
                    string uri = htmlNodes[i].ChildNodes[1].Attributes["href"].Value;
                    string PosName = htmlNodes[i].ChildNodes[1].ChildNodes[1].ChildNodes[1].ChildNodes[1].InnerText;
                    Data.Add(new BuildingTypeNamesData
                    {
                        DetailUri = uri,
                        PositionName = PosName
                    }
                    );
                }
                var StringfiedData = JSONHelper.Stringify(Data);
                await CacheHelper.WriteCache(CacheBuildingTypeJSON, StringfiedData);
                return Data;
            }
            private static async Task<List<ClassRoomStatueData>> GetListBuildingInfoAsync(BuildingTypeNamesData SourceData)
            {
                string html = "http://jxgl.cic.tsinghua.edu.cn/" + SourceData.DetailUri;
                HtmlWeb web = new HtmlWeb();
                var htmlDoc = web.Load(html);

                var _BuildingName = htmlDoc.DocumentNode.SelectNodes("//html/body/div/div/div[1]/div[1]/span/span")[0].InnerText;
                var _InnerTextDate = htmlDoc.DocumentNode.SelectSingleNode("//html/body/div/div/div[1]/ul/li[1]/div[2]/div/select").InnerHtml;

                Regex regexObj = new Regex(@"<option selected=""selected\"">(?<mycontent>[\s\S].*?)\r\n    \r\n    <option>");
                var resultString = regexObj.Match(_InnerTextDate).Groups["mycontent"].Value;
                var _Date = resultString;

                var _NodeClassRoom = htmlDoc.DocumentNode.SelectNodes("/html/body/div/div/div[1]/ul/li[2]/div[@class='card-footer no-border']");

                var Data = new List<ClassRoomStatueData>();
                for (int i = 0; i < _NodeClassRoom.Count; i++)
                {
                    var _NodeSpanClassRoom = _NodeClassRoom[i].ChildNodes;
                    var _ClassRoomName = _NodeSpanClassRoom[1].InnerText;

                    var _NodeClassState = _NodeSpanClassRoom[3].SelectNodes("i");

                    var _ListClassState = new List<string>();
                    var _ListBoolClassState = new List<bool>();

                    for (int j = 0; j < _NodeClassState.Count; j++)
                    {
                        var _StatueOfClassRoom = _NodeClassState[j].Attributes["class"].Value;
                        if (Regex.IsMatch(_StatueOfClassRoom, "ico_zy"))//占用
                        {
                            string item = "占用";
                            _ListClassState.Add(item);
                            bool boolitem = true;
                            _ListBoolClassState.Add(boolitem);
                        }
                        else
                        {
                            string item = "空闲";
                            _ListClassState.Add(item);
                            bool boolitem = false;
                            _ListBoolClassState.Add(boolitem);
                        }

                    }

                    Data.Add(new ClassRoomStatueData
                    {
                        BuildingName = _BuildingName,
                        Date = _Date,
                        ListClassStatus = _ListClassState,
                        ClassRoomName = _ClassRoomName,
                        ListBoolClassStatus=_ListBoolClassState
                    }
                    );
                }
                var StringfiedData = JSONHelper.Stringify(Data);
                await CacheHelper.WriteCache($"ClassBuildingData_{SourceData.PositionName}", StringfiedData);
                return Data;
            }
            private static async Task<ClassBuildingInfo> GetListAllBuildingInfoAsync()
            {
                var _ClassBuildingInfo = new ClassBuildingInfo();
                _ClassBuildingInfo.ListClassRoomStatue = new List<BuildingInfoData>();
                var _ClassNamesAsync = await GetBuildingTypeAsync();
                _ClassBuildingInfo.ListClassRoomInfo = _ClassNamesAsync;

                foreach (BuildingTypeNamesData item in _ClassNamesAsync)
                {
                    var _ListBuildingInfoData = await GetListBuildingInfoAsync(item);
                    _ClassBuildingInfo.Date = DateTime.Now.Date;
                    var _ListBuildingInfo = new BuildingInfoData();
                    _ListBuildingInfo.BuildingName = item.PositionName;
                    _ListBuildingInfo.ListBuildingInfoData = _ListBuildingInfoData;

                    _ClassBuildingInfo.ListClassRoomStatue.Add(_ListBuildingInfo);

                }
                var StringfiedData = JSONHelper.Stringify(_ClassBuildingInfo);
                await CacheHelper.WriteCache(CacheAllClassRoomInfoDataJSON, StringfiedData);
                return _ClassBuildingInfo;
            }

        }
    }  
}
