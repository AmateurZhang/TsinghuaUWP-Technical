using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassRoomAPI.Models
{
    //Modes
    public enum WebLearnLoginMode
    {
        Remote,
        Local,
        Demo,
        Anonymous
    }
    public class Password
    {
        public string username;
        public string password;
    }
    //Ensemble
    public class Course
    {
        public string id;
        public string name;
        public bool isNew; //uses (`learn.cic` or `learn`?) .tsinghua.edu.cn
        public string semester;

        override public string ToString()
        {
            return "#" + id + ": " + name;
        }
    }

    public class Deadline
    {
        public string id;
        public string name;
        public string ddl;
        public string course;
        public string detail;
        public string courseid;
        public bool hasBeenFinished;

        public bool shouldBeIgnored()
        {
            string[] keywords = {
                "补交",
                "迟交"
            };
            foreach (var keyword in keywords)
            {
                if (name.IndexOf(keyword) != -1)
                    return true;
            }

            string[] courses = {
                "实验室科研探究"
            };
            foreach (var _course in courses)
            {
                if (course.IndexOf(_course) != -1)
                    return true;
            }

            return false;
        }


        public double daysFromNow()
        {
            return (DateTime.Parse(ddl + " 23:59") - DateTime.Now).TotalDays;
        }

        public string timeLeft()
        {
            return timeLeftChinese();
        }

        public bool isPast()
        {
            return DateTime.Parse(ddl + " 23:59") < DateTime.Now;
        }

        public string timeLeftChinese()
        {
            TimeSpan timeDelta = DateTime.Parse(ddl + " 23:59") - DateTime.Now;

            var daysLeft = timeDelta.TotalDays;
            string timeLeft = "";

            if (daysLeft > 10)
            {
                var d = Math.Round(daysLeft / 7);
                timeLeft = "还有 " + d.ToString() + " 周";
            }
            if (daysLeft > 1)
            {
                var d = Math.Round(daysLeft);
                timeLeft = "只剩 " + d.ToString() + " 天";
            }
            else if (daysLeft > 0)
            {
                var d = timeDelta.Hours;
                if (d > 0)
                    timeLeft = "只剩 " + d.ToString() + " 小时";
                else
                    timeLeft = "即将到期！";
            }
            else if (daysLeft > -1)
            {
                var d = (-timeDelta.Hours);
                timeLeft = "已经过去 " + d.ToString() + " 小时";
            }
            else if (daysLeft > -10)
            {
                var d = (-timeDelta.Days);
                timeLeft = "已经过去 " + d.ToString() + " 天";
            }
            else
            {
                var d = Math.Round(timeDelta.TotalDays / -7);
                timeLeft = "已经过去 " + d.ToString() + " 周";
            }


            return timeLeft;
        }

        public string paindetail
        {
            get
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(detail);

                return doc.DocumentNode.InnerText;
            }
           
        }
        public string HasbeenFnishedChinese
        {
            get
            {
                if(hasBeenFinished)
                {
                    return "已完成";
                }
                else
                {
                    return "未完成";
                }
            }
        }

    }

    public class ListCourseInfoData
    {
        public Course CourseInfo;
        public List<Deadline> Deadlines;
        public int LengthOfDeadLines
        {
            get
            {
                return Deadlines.Count;
            }
        }
        public string StringLenOfDeadlines
        {
            get
            {
                string _Result = "共有" + LengthOfDeadLines.ToString() + "条数据";
                return _Result;
            }
        }
    }

    public class WebLearnInfo
    {
        public DateTime Date;
        public List<Course> ListCourseInfo;
        public List<ListCourseInfoData> ListCourseInfoDetail;

    }

    public class ExceptionCodeWebLearnData
    {

        //Define Exception Codes
        public static int EXCEPTION_RETURN_LOCAL_DATA_FAILED = 1001;
        public static int EXCEPTION_RETURN_REMOTE_DATA_FAILED = 1002;
    }




    //JSON FORM DATASTRUCT DO NOT EDIT HERE!
    public class CourseAssignmentsRootobject
    {
        public string message { get; set; }
        public Resultlist[] resultList { get; set; }
    }

    public class Resultlist
    {
        public Coursehomeworkrecord courseHomeworkRecord { get; set; }
        public Coursehomeworkinfo courseHomeworkInfo { get; set; }
    }

    public class Coursehomeworkrecord
    {
        public int seqId { get; set; }
        public string studentId { get; set; }
        public string teacherId { get; set; }
        public string homewkId { get; set; }
        public long? regDate { get; set; }
        public string homewkDetail { get; set; }
        public Resourcesmappingbyhomewkaffix resourcesMappingByHomewkAffix { get; set; }
        public object replyDetail { get; set; }
        public object resourcesMappingByReplyAffix { get; set; }
        public int? mark { get; set; }
        public long? replyDate { get; set; }
        public object iffine { get; set; }
        public string status { get; set; }
        public string ifDelay { get; set; }
        public string gradeUser { get; set; }
        public int groupId { get; set; }
        public object groupName { get; set; }
    }

    public class Resourcesmappingbyhomewkaffix
    {
        public string fileId { get; set; }
        public string resourcesId { get; set; }
        public int diskId { get; set; }
        public long regDate { get; set; }
        public string fileName { get; set; }
        public int browseNum { get; set; }
        public int downloadNum { get; set; }
        public string extension { get; set; }
        public string fileSize { get; set; }
        public string courseId { get; set; }
        public object playUrl { get; set; }
        public object downloadUrl { get; set; }
        public int resourcesStatus { get; set; }
        public string userCode { get; set; }
    }

    public class Coursehomeworkinfo
    {
        public int homewkId { get; set; }
        public long? regDate { get; set; }
        public long beginDate { get; set; }
        public long endDate { get; set; }
        public int teachingWeekId { get; set; }
        public string title { get; set; }
        public string detail { get; set; }
        public object homewkAffix { get; set; }
        public object homewkAffixFilename { get; set; }
        public object homewkIndex { get; set; }
        public object answerDetail { get; set; }
        public object answerLink { get; set; }
        public object answerLinkFilename { get; set; }
        public object answerDate { get; set; }
        public string courseId { get; set; }
        public object homewkGroupNum { get; set; }
        public string courseSource { get; set; }
        public int noteId { get; set; }
        public object courseKnowledge { get; set; }
        public int weiJiao { get; set; }
        public int yiJiao { get; set; }
        public int yiYue { get; set; }
        public int yiPi { get; set; }
        public int jiaoed { get; set; }
    }

    //Sememster
    public class Semesters
    {
        public Semester currentSemester { get; set; }
        public Semester nextSemester { get; set; }
    }

    public class Semester
    {
        public string id { get; set; }
        public string semesterName { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public string semesterEname { get; set; }

        public string getWeekName()
        {
            var semesterStart = DateTime.Parse(startDate);
            var delta = DateTime.Now - semesterStart;
            var days = delta.TotalDays;

            /* 0 ~  6.9 -> 1
               7 ~ 13.9 -> 2 */
            return (Math.Floor(days / 7) + 1).ToString();
        }
    }

    public class SemestersRootObject
    {
        public Currentteachingweek currentTeachingWeek { get; set; }
        public Semester currentSemester { get; set; }
        public string currentDate { get; set; }
        public Semester nextSemester { get; set; }
    }

    public class Currentteachingweek
    {
        public int teachingWeekId { get; set; }
        public string weekName { get; set; }
        public string beginDate { get; set; }
        public string endDate { get; set; }
        public string semesterId { get; set; }
    }


    public class Timetable : List<Event>
    {

    }

    public class Event
    {
        public string dd { get; set; }
        public string fl { get; set; }
        public int grrlID { get; set; }
        public string jssj { get; set; }
        public string kssj { get; set; }
        public string nq { get; set; }
        public string nr { get; set; }
        public string sfSjtz { get; set; }
        public string skjc { get; set; }
        public string sm { get; set; }
    }

    //JSON FORM DATASTRUCT DO NOT EDIT HERE!
}
