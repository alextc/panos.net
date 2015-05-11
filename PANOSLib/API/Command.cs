namespace PANOS
{
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using log4net;

    public class Command<TApiResponse> : ICommand<TApiResponse> where TApiResponse : ApiResponse
    {
        private readonly Uri apiUri;
        private readonly FormUrlEncodedContent apiPostData;
        private readonly ILog log = LogManager.GetLogger(typeof(Command<TApiResponse>));
        
        public Command(Uri uri, FormUrlEncodedContent postData)
        {
            Logger.Configure();
            apiUri = uri;
            apiPostData = postData;
        }

        public TApiResponse Execute()
        {
            if (log.IsDebugEnabled)
            {
                log.DebugFormat("About to POST to {0}, with the payload of:{1}{2}", apiUri, Environment.NewLine, HttpUtils.PrettyPrintPostData(apiPostData));
            }

            string httpResponse;
            using (var client = new HttpClient())
            {
                using (var response = client.PostAsync(apiUri, apiPostData).Result)
                {
                    using (var content = response.Content)
                    {
                        httpResponse = content.ReadAsStringAsync().Result;
                    }
                }   
            }

            if (log.IsDebugEnabled)
            {
                log.DebugFormat("PANOS Responsed to POST with:{0}{1}", Environment.NewLine, XmlUtils.PetttyPrintXml(httpResponse));
            } 

            return ProcessResponse(httpResponse);
        }

        private TApiResponse ProcessResponse(string xml)
        {
            var responseStatus = GetResponseStatus(xml);
            if (responseStatus == ResponseStatus.Success)
            {
                // TODO: Validate Schema
                return Deserialize(xml);
            }

            throw ErrorHandler.GenerateException(xml);
        }

        private ResponseStatus GetResponseStatus(string xml)
        {
            var responseBase = Deserialize(xml) as ApiResponse;

            if (responseBase != null)
            {
                var status = responseBase.Status;

                switch (status)
                {
                    case "success":
                        return ResponseStatus.Success;
                    case "error":
                        return ResponseStatus.Error;
                }
            }

            throw new ArgumentException(string.Format("Unrecongized response status was received: {0}", xml));
        }

        private static TApiResponse Deserialize(string xml)
        {
            using (var memoryStream = new MemoryStream(Encoding.Unicode.GetBytes(xml)))
            {
                using (var reader = XmlReader.Create(memoryStream))
                {
                    var ser = new XmlSerializer(typeof(TApiResponse));
                    return (TApiResponse)ser.Deserialize(reader);
                }
            }
        }
    }
}
