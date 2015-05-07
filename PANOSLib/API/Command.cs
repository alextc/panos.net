﻿namespace PANOS
{
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    
    public class Command<TApiResponse> : ICommand<TApiResponse> where TApiResponse : ApiResponse
    {
        private readonly Uri apiUri;
        private readonly FormUrlEncodedContent apiPostData;
        
        public Command(Uri uri, FormUrlEncodedContent postData)
        {
            apiUri = uri;
            apiPostData = postData;
        }

        public TApiResponse Execute()
        {
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

            // I can't and probably should not log every response
            // Also I am limited to 32766 chars for the log payload
            // Logger.LogPanosHttpResponse(httpResponse);
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

        private static ResponseStatus GetResponseStatus(string xml)
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
