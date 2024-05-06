using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace Core.CL.Helper
{
    public class HttpCaller
    {
        public HttpCaller(string baseAddress)
        {
            _baseAddress = baseAddress;
        }
        public string _baseAddress { get; set; }

        #region Generic Methods For Get & Post Calls

        public T Get<T>(string endPoint)
        {
            return GetCall<T>(endPoint);
        }

        public T Post<T>(string endPoint, object data)
        {
            return PostCall<T>(endPoint, data);
        }

        private T GetCall<T>(string endPoint)
        {
            if (!string.IsNullOrEmpty(endPoint))
            {
                HttpWebRequest request = GetWebRequestObject(endPoint);
                try
                {
                    using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            using (Stream stream = response.GetResponseStream())
                            {
                                var result = JsonConvert.DeserializeObject<T>(new StreamReader(stream).ReadToEnd());
                                return result;
                            }
                        }
                    }
                }
                catch (WebException e)
                {
                    string c = string.Empty;
                    if (e.Status == WebExceptionStatus.ProtocolError)
                    {
                        Stream stremm = ((HttpWebResponse)e.Response).GetResponseStream();
                        c = new StreamReader(stremm).ReadToEnd();
                    }

                    throw new SystemException(c);
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return default(T);
        }

        private T PostCall<T>(string endPoint,
            object postValues)
        {
            if (!string.IsNullOrEmpty(endPoint))
            {
                string json = JsonConvert.SerializeObject
                (
                    postValues, Formatting.None, new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        NullValueHandling = NullValueHandling.Ignore
                    }
                );

                HttpWebRequest request = GetWebRequestObject(endPoint);
                request.ContentType = "text/json";
                request.Method = "POST";
                UTF8Encoding encoding = new UTF8Encoding();
                byte[] byte1 = encoding.GetBytes(json);
                request.ContentLength = byte1.Length;

                request.Credentials = CredentialCache.DefaultCredentials;
                using (StreamWriter streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(json);
                }

                try
                {
                    HttpWebResponse httpResponse;
                    using (httpResponse = (HttpWebResponse)request.GetResponse())
                    {
                        using (Stream stream = httpResponse.GetResponseStream())
                        {
                            var result = JsonConvert.DeserializeObject<T>(new StreamReader(stream).ReadToEnd());
                            return result;
                        }
                    }
                }
                catch (WebException e)
                {
                    string c = string.Empty;
                    if (e.Status == WebExceptionStatus.ProtocolError)
                    {
                        Stream stremm = ((HttpWebResponse)e.Response).GetResponseStream();
                        c = new StreamReader(stremm).ReadToEnd();
                    }

                    throw new SystemException(c);
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return default(T);
        }

        private HttpWebRequest GetWebRequestObject(string endPoint)
        {
            HttpWebRequest request;
            if (endPoint.Contains("http"))
            {
                request = (HttpWebRequest)WebRequest.Create(endPoint);
            }
            else
            {
                request = (HttpWebRequest)WebRequest.Create($"{this._baseAddress}api/{endPoint}");
            }

            request.ProtocolVersion = HttpVersion.Version10;
            request.ServicePoint.Expect100Continue = false;
            request.KeepAlive = false;
            request.Timeout = 15 * 60000;
            return request;
        }

        #endregion Generic Methods For Get & Post Calls
    }
}