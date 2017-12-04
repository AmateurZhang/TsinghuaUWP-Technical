using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace ClassRoomAPI.Helpers
{
    public class LocalSettingHelper
    {
        private static ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

        static public Windows.Foundation.Collections.IPropertySet GetLocalSettings()
        {
            return localSettings.Values;
        }


        static public void SetLocalSettings<Type>(string key, Type value)
        {
            localSettings.Values[key] = value;
        }
    }
}
