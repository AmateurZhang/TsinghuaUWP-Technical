using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace ClassRoomAPI.Helpers
{
    public class CacheHelper
    {
        //Cache 读写
        public static async Task WriteCache(string filename, string value)
        {
            StorageFolder localCacheFolder = ApplicationData.Current.LocalCacheFolder;
            StorageFile file;
            try
            {
                file = await localCacheFolder.GetFileAsync(filename);
            }
            catch
            {
                file = await localCacheFolder.CreateFileAsync(filename);
            }
            await FileIO.WriteTextAsync(file, value);
        }

        public static async Task<string> ReadCache(string filename)
        {
            try
            {
                StorageFolder localCacheFolder = ApplicationData.Current.LocalCacheFolder;
                StorageFile file = await localCacheFolder.GetFileAsync(filename);
                String fileContent = await FileIO.ReadTextAsync(file);
                return fileContent;
            }
            catch
            {
                return "";
            }
        }
    }
}
