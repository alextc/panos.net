namespace PANOS
{
    using System.Net.Http;
    using System.Text;
    using System.Web;

    public static class HttpUtils
    {
        public static  string PostDataToString(FormUrlEncodedContent formUrlEncodedContent)
        {
            var res = HttpUtility.UrlDecode(formUrlEncodedContent.ReadAsByteArrayAsync().Result, Encoding.ASCII);
            return res;
        }
    }
}
