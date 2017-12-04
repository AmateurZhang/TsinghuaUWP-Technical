using ClassRoomAPI.Helpers;
using ClassRoomAPI.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Storage;

namespace ClassRoomAPI.Services
{
    public class WebLearnAPIService
    {
        //Consts
        private static string CacheAllWebLearnDataJSON = "JSONAllWebLearnData";

        //Public Class
        //Login 
        private static ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        static public bool CredentialAbsent()
        {
            var username = localSettings.Values["username"];
            return username == null
                || username.ToString() == "__anonymous";
        }

        static public bool SupposedToWorkAnonymously()
        {
            var username = localSettings.Values["username"];
            return username != null
                && username.ToString() == "__anonymous";
        }

        static public bool IsDemo()
        {
            return
                localSettings.Values["username"] != null &&
                localSettings.Values["username"].ToString() == "233333";
        }

        public static async 
        Task
         LogintoWebLearnMode(WebLearnLoginMode Mode = WebLearnLoginMode.Remote)
        {
            if (Mode == WebLearnLoginMode.Local)
            {
                Debug.WriteLine("[LogintoWebLearnMode] Reuse Session.");
            }
            else if (Mode == WebLearnLoginMode.Remote)
            {
                if (CredentialAbsent())
                {
                    throw new MessageException("没有指定用户名和密码");
                }
                var username = LocalSettingHelper.GetLocalSettings()["username"].ToString();

                if (username == "__anonymous")
                {
                    throw new MessageException("没有指定用户名和密码");
                }

                var vault = new Windows.Security.Credentials.PasswordVault();
                var password = vault.Retrieve("Tsinghua_Learn_Website", username).Password;

                await LogintoWebLearnAsync(username, password);
            }
            else if(Mode == WebLearnLoginMode.Anonymous)
            {
                Debug.WriteLine("[LogintoWebLearnMode] Anonymous.");
            }
            else
            {
                //demo
            }
        }
        //EmailName
        public static async Task<string> GetEmailName()
        {
            var Page=await PostGetHelper.GET(URLWebLearnInfoPage);
            return ParseEmailUserName(Page);
        }

        //WeblearnInfo
        public static async Task<WebLearnInfo> GetAllWebLearnInfoMode(ParseDataMode Mode = ParseDataMode.Remote)
        {
            if (Mode == ParseDataMode.Local)
            {
                try
                {
                    await LogintoWebLearnMode(WebLearnLoginMode.Local);
                    var TempData = await CacheHelper.ReadCache(CacheAllWebLearnDataJSON);
                    var ReturnData = JSONHelper.Parse<WebLearnInfo>(TempData);
                    Debug.WriteLine("[GetAllWebLearnInfoMode] return local data.");
                    return ReturnData;
                }
                catch
                {
                    Debug.WriteLine("[GetListAllBuildingInfoMode] return local data fails.");
                    var _Exception = new NumberException(ExceptionCodeWebLearnData.EXCEPTION_RETURN_LOCAL_DATA_FAILED);
                    throw _Exception;
                }
            }
            else if (Mode == ParseDataMode.Remote)
            {
                try
                {
                    await LogintoWebLearnMode();
                    Debug.WriteLine("[GetListAllBuildingInfoMode] return remote data.");
                    return await GetAllCourseInfoAysnc();
                }
                catch
                {
                    Debug.WriteLine("[GetListAllBuildingInfoMode] return remote data fails.");
                    var _Exception = new NumberException(ExceptionCodeWebLearnData.EXCEPTION_RETURN_REMOTE_DATA_FAILED);
                    throw _Exception;
                }
            }
            else
            {
                //demo
                return null;
            }
        }

        //Private Class
        //Login
        private static string LoginUri = "https://learn.tsinghua.edu.cn/MultiLanguage/lesson/teacher/loginteacher.jsp";
        private static string CourseListUrl_Login = "http://learn.tsinghua.edu.cn/MultiLanguage/lesson/student/MyCourse.jsp?language=cn";
        public static async Task<int> LogintoWebLearnAsync(string username = "", string password = "")
        {
            
            Debug.WriteLine("[login] begin");
            try
            {
                string _LoginResponse;

                //login to learn.tsinghua.edu.cn

                _LoginResponse = await PostGetHelper.POST(
                    LoginUri,
                    $"leixin1=student&userid={username}&userpass={password}");

                //check if successful
                var alertInfoGroup = Regex.Match(_LoginResponse, @"window.alert\(""(.+)""\);").Groups;

                if (alertInfoGroup.Count > 1)
                {
                    throw new MessageException(alertInfoGroup[1].Value.Replace("\\r\\n", "\n"));
                 }
                if (_LoginResponse.IndexOf(@"window.location = ""loginteacher_action.jsp"";") == -1)
                {
                    throw new MessageException("login_redirect");
                }

                //get iframe src
                HtmlDocument htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(await PostGetHelper.GET(CourseListUrl_Login));

                string iframeSrc = "";
                try
                {
                    iframeSrc = htmlDoc.DocumentNode.Descendants("iframe")/*MAGIC*/.First().Attributes["src"].Value;
                }
                catch (Exception)
                {
                    throw new MessageException("CIC URL CHANGED.");
                }


                //login to learn.cic.tsinghua.edu.cn
                await PostGetHelper.GET(iframeSrc);

            }
            catch (Exception e)
            {
                Debug.WriteLine("[login] unsuccessful");
                throw e;
            }
            return 1;

        }

        //GetEmailName
        private static string URLWebLearnInfoPage = "http://learn.tsinghua.edu.cn/MultiLanguage/vspace/vspace_userinfo1.jsp";
        private static string ParseEmailUserName(string page)
        {
            string resultString = null;
            var subjectString = page;
            try
            {
                Regex regexObj = new Regex(@"<td class=""title"" height=""20"">电子邮件</td>\r\n\t\t\t\r\n\t\t\t\r\n\t\t<td class=""tr_l""><input class=""input_blue"" type=hidden name=email size=40 value=""(?<mycontent>[\s\S]+)@mails.tsinghua.edu.cn"">");
                resultString = regexObj.Match(subjectString).Groups["mycontent"].Value;
            }
            catch
            {
                // Syntax error in the regular expression
                Debug.WriteLine("get email name fails");
            }

            return resultString;

        }

        //GetListCourseInfoData
        private static async Task<WebLearnInfo> GetAllCourseInfoAysnc()
        {
            var _AllCourseData = new WebLearnInfo();
            var _ListCourseInfoData = new List<ListCourseInfoData>();
            var _ListCourseInfo= await getRemoteCourseList();
            _AllCourseData.ListCourseInfo = _ListCourseInfo;
            _AllCourseData.ListCourseInfoDetail = new List<ListCourseInfoData>();
            _AllCourseData.Date = DateTime.Now;

            foreach(var item in _ListCourseInfo)
            {
                var _CourseInfoData = new ListCourseInfoData();
                _CourseInfoData.CourseInfo = item;

                if (!item.isNew)
                {
                    _CourseInfoData.Deadlines = await getRemoteHomeworkList(item);
                }
                else
                {
                    _CourseInfoData.Deadlines = await getRemoteHomeworkListNew(item);
                }
                _ListCourseInfoData.Add(_CourseInfoData);
            }
            _AllCourseData.ListCourseInfoDetail = _ListCourseInfoData;
            var StringfiedData = JSONHelper.Stringify(_AllCourseData);
            await CacheHelper.WriteCache(CacheAllWebLearnDataJSON, StringfiedData);
            return _AllCourseData;

        }

        //GetHomeWork by ID
        private static async Task<List<Deadline>> getRemoteHomeworkList(Course CourseInfo)
        {
            return await parseHomeworkListPageAsync(await getHomeworkListPage(CourseInfo.id));
        }

        private static async Task<List<Deadline>> getRemoteHomeworkListNew(Course CourseInfo)
        {
            //await LogintoWebLearnMode();
            var _Page = await getHomeworkListPageNew(CourseInfo.id);
            return await parseHomeworkListPageNew(_Page);
        }

        //Semester
        private static async Task<Semesters> getRemoteSemesters()
        {
            var _remoteCalendar = parseSemestersPage(await getCalendarPage());
            return new Semesters
            {
                currentSemester = _remoteCalendar.currentSemester,
                nextSemester = _remoteCalendar.nextSemester
            };
        }

        //Courses
        private static async Task<List<Course>> getRemoteCourseList()
        {
            return parseCourseList(await getCourseListPage());
        }

        // remote object URLs and wrappers

        private static string courseListUrl = "http://learn.tsinghua.edu.cn/MultiLanguage/lesson/student/MyCourse.jsp?language=cn";

        private static async Task<string> getHomeworkListPage(string courseId)
        {
            return await PostGetHelper.GET($"http://learn.tsinghua.edu.cn/MultiLanguage/lesson/student/hom_wk_brw.jsp?course_id={courseId}");
        }

        private static async Task<string> getHomeworkListPageNew(string courseId)
        {
            var timestamp = ClassLibrary.UnixTime().TotalMilliseconds;
            string url = $"http://learn.cic.tsinghua.edu.cn/b/myCourse/homework/list4Student/{courseId}/0?_={timestamp}";
            return await PostGetHelper.GET(url);
        }

        private static async Task<string> getCourseListPage()
        {
            return await PostGetHelper.GET(courseListUrl);
        }

        private static async Task<string> getCalendarPage()
        {
            return await PostGetHelper.GET("http://learn.cic.tsinghua.edu.cn/b/myCourse/courseList/getCurrentTeachingWeek");
        }

        
        // parse HTML or JSON, and return corresponding Object

        private static async Task<List<Deadline>> parseHomeworkListPageAsync(string page)
        {
            try
            {
                HtmlDocument htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(page);

                string _name, _due, _course;

                _course = htmlDoc.DocumentNode.Descendants("td")/*MAGIC*/.First().InnerText;
                _course = _course.Trim();
                _course = _course.Substring(6/*MAGIC*/);
                _course = WebUtility.HtmlDecode(_course);

                HtmlNode[] nodes = htmlDoc.DocumentNode.Descendants("tr")/*MAGIC*/.ToArray();


                List<Deadline> deadlines = new List<Deadline>();
                for (int i = 4/*MAGIC*/; i < nodes.Length - 1/*MAGIC*/; i++)
                {
                    HtmlNode node = nodes[i];

                    var tds = node.Descendants("td");

                    var _isFinished = (tds.ElementAt(3/*MAGIC*/).InnerText.Trim() == "已经提交");

                    _due = tds.ElementAt(2/*MAGIC*/).InnerText;

                    var link_to_detail = node.Descendants("a")/*MAGIC*/.First();
                    _name = link_to_detail.InnerText;
                    _name = WebUtility.HtmlDecode(_name);

                    var _href = link_to_detail.Attributes["href"].Value;
                    var _id = Regex.Match(_href, @"[^_]id=(\d+)").Groups[1].Value;
                    var _courseid=Regex.Match(_href, @"[^_]course_id=(\d+)").Groups[1].Value;
                    var _cplhref = "http://learn.tsinghua.edu.cn/MultiLanguage/lesson/student/" + _href;

                    var _detail = await parseHWListContent(_cplhref);
                    //_detail = "<b>" + _name + "</b>" + "<br>" + _detail;

                    deadlines.Add(new Deadline
                    {
                        name = _name,
                        ddl = _due,
                        course = _course,
                        hasBeenFinished = _isFinished,
                        id = "@" + _id,
                        detail = _detail,
                        courseid=_courseid
                    });
                }
                return deadlines;
            }
            catch (Exception)
            {
                throw new MessageException("AssignmentList");
            }

        }

        private static async Task<List<Deadline>> parseHomeworkListPageNew(string page)
        {

            List<Deadline> deadlines = new List<Deadline>();

            string _course = "";
            var root = JSONHelper.Parse<CourseAssignmentsRootobject>(page);
            foreach (var item in root.resultList)
            {
                var _isFinished = (item.courseHomeworkRecord.status != "0" /*MAGIC*/);

                var _dueTimeStamp = item.courseHomeworkInfo.endDate;
                var _dueDate = (new DateTime(1970, 1, 1, 0, 0, 0, 0)).ToLocalTime().AddMilliseconds(_dueTimeStamp).Date;
                string _due = $"{_dueDate.Year}-{_dueDate.Month}-{_dueDate.Day}";


                string _name = item.courseHomeworkInfo.title;
                string _courseId = item.courseHomeworkInfo.courseId;
                string _detail = item.courseHomeworkInfo.detail;
                if (_detail == null)
                    _detail = "无内容";

                _detail = "<b>" + _name + "</b><br>" + _detail;
                if (_course == "")
                    _course = _courseId;
                if (_course == _courseId)
                {
                    foreach (var course in await getRemoteCourseList())
                    {
                        if (course.id == _courseId)
                            _course = course.name;
                    }
                }

                _course = WebUtility.HtmlDecode(_course);
                _name = WebUtility.HtmlDecode(_name);

                deadlines.Add(new Deadline
                {
                    name = _name,
                    ddl = _due,
                    course = _course,
                    hasBeenFinished = _isFinished,
                    id = "_" + item.courseHomeworkInfo.homewkId,
                    detail = _detail,
                    courseid = _courseId
                });
            }
            return deadlines;
        }

        private static List<Course> parseCourseList(string page)
        {
            try
            {
                List<Course> courses = new List<Course>();

                HtmlDocument htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(page);
                var links = htmlDoc.DocumentNode.Descendants("table")/*MAGIC*/.Last()/*MAGIC*/.Descendants("a")/*MAGIC*/.ToArray();

                foreach (var link in links)
                {
                    string _name = link.InnerText.Trim();
                    string _url = link.Attributes["href"].Value;
                    var match = Regex.Match(_name, "(.+?)\\((\\d+)\\)\\((.+?)\\)");
                    string _semester = match.Groups[3].Value;
                    _name = match.Groups[1].Value;
                    bool _isNew = false;
                    string _id = "";

                    if (_url.StartsWith("http://learn.cic.tsinghua.edu.cn/"))
                    {
                        _isNew = true;
                        _id = Regex.Match(_url, "/([-\\d]+)").Groups[1].Value;
                    }
                    else
                    {
                        _isNew = false;
                        _id = Regex.Match(_url, "course_id=(\\d+)").Groups[1].Value;
                    }
                    courses.Add(new Course
                    {
                        name = _name,
                        isNew = _isNew,
                        id = _id,
                        semester = _semester
                    });
                }
                return courses;
            }
            catch (Exception)
            {
                throw new MessageException("CourseList");
            }
        }

        private static SemestersRootObject parseSemestersPage(string page)
        {
            return JSONHelper.Parse<SemestersRootObject>(page);
        }

        private static Timetable parseTimetablePage(string page)
        {
            if (page.Length < "_([])".Length)
                throw new MessageException("timetable_javascript");
            string json = page.Substring(2, page.Length - 3);
            return JSONHelper.Parse<Timetable>(json);
        }

        private static async Task<string> parseHWListContent(string uri)
        {
            string page = await PostGetHelper.GET(uri);
            string resultString = null;
            var subjectString = page;
            try
            {
                Regex regexObj = new Regex(@"<td height=""100"" class=""title""> 作业说明</td>\r\n\t\t\t<td class=""tr_2""><textarea cols=55  rows=""7"" style=""width:650px""  wrap=VIRTUAL>(?<mycontent>[\s\S].*?)</textarea></td>");
                resultString = regexObj.Match(subjectString).Groups["mycontent"].Value;

                if (resultString.Length == 0)
                {
                    try
                    {
                        Regex regexObj1 = new Regex(@"<textarea cols=55  rows=""7"" style=""width:650px""  wrap=VIRTUAL>(?<mycontent>[\s\S]+)\n</textarea>");
                        resultString = regexObj1.Match(subjectString).Groups["mycontent"].Value;
                        if (resultString.Length == 0)
                            resultString = "无内容或如题";
                    }
                    catch
                    {

                    }
                }


            }
            catch
            {
                // Syntax error in the regular expression
            }

            return resultString;
        }

    }
}
