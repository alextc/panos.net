namespace PANOS
{
    using System;
    using System.Net.Http;

    public class HttpClientWrapper
    {
        private readonly Uri queryUri;

        private readonly FormUrlEncodedContent formUrlEncodedContent;

        public HttpClientWrapper(Uri uri, FormUrlEncodedContent formUrlEncodedContent)
        {
            queryUri = uri;
            this.formUrlEncodedContent = formUrlEncodedContent;
        }

        // Purposely turning this method to a blocking one (as opposed to using async)
        // See issues with PowerShell and Async here http://www.dev-one.com/2014/03/asynchronous-methods-in-powershell/
        // Work-around here

        public string ExecutePost()
        {
            using (var client = new HttpClient())
            {
                using (var response = client.PostAsync(queryUri, formUrlEncodedContent).Result)
                {
                    using (var content = response.Content)
                    {
                        return content.ReadAsStringAsync().Result;
                    }
                }
            }
        }
    }
}
