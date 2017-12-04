using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ClassRoomAPI.Models
{
    //Consts
    public enum ParseDataMode
    {
        Remote,
        Local,
        Demo
    }

    public class ExceptionCodeClassRoomInfo
    {

        //Define Exception Codes
        public static int EXCEPTION_RETURN_LOCAL_DATA_FAILED = 1001;
        public static int EXCEPTION_RETURN_REMOTE_DATA_FAILED = 1002;
    }

    //DataStruct
    public class BuildingTypeNamesData
    {
        public string PositionName;
        public string DetailUri;
    }
    public class ClassRoomStatueData
    {
        public string BuildingName;
        public string Date;
        public string ClassRoomName;

        //6节课1-6
        public List<string> ListClassStatus;
        //true=占用
        public List<bool> ListBoolClassStatus;

        public List<string> ConvertBooltoSymbol
        {
            get
            {
                var _Return=new List<string>();
                for(int i=0;i<ListBoolClassStatus.Count;i++)
                    try
                    {
                         if (ListBoolClassStatus[i] == true)
                         {
                            _Return.Add("\xEDAF");
                         }
                         else
                         {
                        _Return.Add("\xECCA");

                          }
                     }
                     catch
                    {
                        _Return.Add("\xECCA");
                     }
                return _Return;
            }
            set {; }
            
        }
    }

    public class BuildingInfoData
    {
        public string BuildingName;
        public List<ClassRoomStatueData> ListBuildingInfoData;
    }

    public class ClassBuildingInfo
    {
        public DateTime Date;
        public List<BuildingTypeNamesData> ListClassRoomInfo;
        public List<BuildingInfoData> ListClassRoomStatue;
    }

    
}
