using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace ClassRoomAPI.Helpers
{
    public class CookieHelper
    {
        // 通用方法类，以后可能单独成为一个.cs
        private static Windows.Web.Http.Filters.HttpBaseProtocolFilter bpf = new Windows.Web.Http.Filters.HttpBaseProtocolFilter();
        private static HttpClient m_httpClient = new HttpClient(bpf);
        static public HttpCookieManager GetCookieManager()
        {
            return bpf.CookieManager;
        }

        private static HttpResponseMessage httpResponse = new HttpResponseMessage();
        public static async Task<string> GET(string url)
        {
            //getPage
            httpResponse = await m_httpClient.GetAsync(new Uri(url));
            httpResponse.EnsureSuccessStatusCode();
            return await httpResponse.Content.ReadAsStringAsync();
        }
    }
}
