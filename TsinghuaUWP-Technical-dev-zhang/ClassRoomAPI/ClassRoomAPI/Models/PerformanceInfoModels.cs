using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassRoomAPI.Models
{
    public class PerformanceData
    {
        //public string Type; //用于记录演出信息
        //public string Date;
        //以下分别是演出的名字、时间、地点、售票状态
        //Performance指代所有演出，drama指代细分领域里面的演出
        public string PerformanceName;
        public string PerformanceTime;
        public string PerformanceAddress;
        public string PerformanceState;

        //public string Date; //用于记录数据被获取的日期
        public static int SelectedItem;
    }
    public class PerformanceType//类比于BuildingTypeNamesData
    {
        public string Type;
        public bool isEmpty;
    }
   
    public class ShowInfo //最上层数据结构，包含演出类别，获取数据的时间，以及每个演出的数据结构
    {
        
        public string PerformanceType;
        public static bool IsItemsEmpty;
        public List<PerformanceData> ListPerformanceInfo;
    }
    public class ShowInfoData  //特意用于写在本底的数据结构，在信息呈现的时候不调用这个
    {
        public DateTime Date;
        public List<ShowInfo> ListShowInfo;
    }
}
