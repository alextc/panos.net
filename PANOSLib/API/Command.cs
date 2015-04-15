namespace PANOS
{
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;

    using PANOS.Logging;

    public class Command<TApiResponse> : ICommand<TApiResponse> where TApiResponse : ApiResponse
    {
        private readonly Uri apiUri;
        private readonly FormUrlEncodedContent apiPostData;
        private readonly FirewallObject objectActedUpon;
        private readonly string nameActedUpon;

        public Command(Uri uri, FormUrlEncodedContent postData)
        {
            apiUri = uri;
            apiPostData = postData;
        }

        public Command(Uri uri, FormUrlEncodedContent postData, string nameActedUpon)
        {
            apiUri = uri;
            apiPostData = postData;
            this.nameActedUpon = nameActedUpon;
        }

        public Command(Uri uri, FormUrlEncodedContent postData, FirewallObject objectActedUpon)
        {
            apiUri = uri;
            apiPostData = postData;
            this.objectActedUpon = objectActedUpon;
        }

        public TApiResponse Execute()
        {
            string httpResponse;
            using (var client = new HttpClient())
            {
                try
                {
                    using (var response = client.PostAsync(apiUri, apiPostData).Result)
                    {
                        Logger.LogPanosResponseStatus(response);
                        using (var content = response.Content)
                        {
                            httpResponse = content.ReadAsStringAsync().Result;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogPanosConnectionFailure(ex);    
                    throw;
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
                var deserializedResonse = Deserialize(xml);
                if (objectActedUpon != null)
                {
                    deserializedResonse.ObjectActedUpon = objectActedUpon;
                }
                
                if(!string.IsNullOrEmpty(nameActedUpon))
                {
                    deserializedResonse.NameActedUpon = nameActedUpon;
                }
                
                return deserializedResonse;
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
            try
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
            catch (Exception ex)
            {
                Logger.LogDeserializationFailure(ex);  
                throw;
            }
        }
    }
}
