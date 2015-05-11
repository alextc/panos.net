namespace PANOS
{
    using System.Net.Http;
    using System.Text;
    using System.Web;

    public static class HttpUtils
    {
        public static  string PrettyPrintPostData(FormUrlEncodedContent formUrlEncodedContent, bool excludeToken = true)
        {
            var postData = HttpUtility.UrlDecode(formUrlEncodedContent.ReadAsByteArrayAsync().Result, Encoding.ASCII);
            
            if (postData != null)
            {
                var sb = new StringBuilder();
                var postValuePairs = postData.Split('&');
                foreach (var keyValuePair in postValuePairs)
                {
                    if(keyValuePair.StartsWith("key=") && excludeToken)
                        continue;

                    sb.AppendLine(keyValuePair);
                }

                return sb.ToString();
            }

            return "";
        }
    }
}
