using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace ClassRoomAPI.Helpers
{
    public class PostGetHelper
    {
        private static Windows.Web.Http.Filters.HttpBaseProtocolFilter bpf = new Windows.Web.Http.Filters.HttpBaseProtocolFilter();
        private static HttpClient m_httpClient = new HttpClient(bpf);
        private static HttpResponseMessage httpResponse = new HttpResponseMessage();
        public static async Task<string> GET(string url)
        {
            //getPage
            httpResponse = await m_httpClient.GetAsync(new Uri(url));
            httpResponse.EnsureSuccessStatusCode();
            return await httpResponse.Content.ReadAsStringAsync();
        }

        //注意此处需要改回private
        public static async Task<string> POST(string url, string form_string)
        {
            HttpStringContent stringContent = new HttpStringContent(
                form_string,
                Windows.Storage.Streams.UnicodeEncoding.Utf8,
                "application/x-www-form-urlencoded");

            httpResponse = await m_httpClient.PostAsync(new Uri(url), stringContent);
            httpResponse.EnsureSuccessStatusCode();
            return await httpResponse.Content.ReadAsStringAsync();
        }
    }
}
