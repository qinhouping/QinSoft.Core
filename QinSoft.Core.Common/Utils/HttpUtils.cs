using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using log4net;
using System.Net;
using System.Net.Security;
using System.IO;
using System.Net.Http.Headers;

namespace QinSoft.Core.Common.Utils
{
    /// <summary>
    /// Http工具类
    /// </summary>
    public static class HttpUtils
    {
        /// <summary>
        /// Http默认日志
        /// </summary>
        public static ILog DefaultLog { get; set; } = LogManager.GetLogger(typeof(HttpUtils));

        /// <summary>
        /// 默认编码
        /// </summary>
        public static Encoding DefaultEncoding { get; set; } = Encoding.UTF8;

        static HttpUtils()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback((s, c, ch, e) => true);
        }

        public static HttpClient GetClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "QinSoft.Core.http");
            return client;
        }

        /// <summary>
        /// 发送请求
        /// </summary>
        public static async Task<HttpResponseMessage> SendAsync(HttpClient client, HttpRequestMessage request)
        {
            try
            {
                DefaultLog?.Debug(string.Format("http send:{0}", request.ToJson()));
                HttpResponseMessage response = await client.SendAsync(request);
                Console.WriteLine(response.StatusCode);
                DefaultLog?.Debug(string.Format("http send statuscode:{0}", response.StatusCode));
                return response;
            }
            catch (Exception e)
            {
                DefaultLog?.Debug("http send", e);
                throw e;
            }
        }

        /// <summary>
        /// GET请求
        /// </summary>
        public static async Task<O> GetAsync<O>(string url, IDictionary<string, string> headers = null)
        {
            HttpClient client = GetClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
            if (!headers.IsEmpty())
            {
                foreach (KeyValuePair<string, string> keyValue in headers)
                {
                    client.DefaultRequestHeaders.Add(keyValue.Key, keyValue.Value);
                }
            }
            HttpResponseMessage response = await SendAsync(client, request);
            string content = await response.Content.ReadAsStringAsync();
            return content.FromJson<O>();
        }

        /// <summary>
        /// DELETE请求
        /// </summary>
        public static async Task<O> DeleteAsync<O>(string url, IDictionary<string, string> headers = null)
        {
            HttpClient client = GetClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, url);
            if (!headers.IsEmpty())
            {
                foreach (KeyValuePair<string, string> keyValue in headers)
                {
                    client.DefaultRequestHeaders.Add(keyValue.Key, keyValue.Value);
                }
            }
            HttpResponseMessage response = await SendAsync(client, request);
            string content = await response.Content.ReadAsStringAsync();
            return content.FromJson<O>();
        }

        /// <summary>
        /// POST请求
        /// </summary>
        public static async Task<O> PostAsync<T, O>(string url, T param, IDictionary<string, string> headers = null)
        {
            HttpClient client = GetClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
            if (!headers.IsEmpty())
            {
                foreach (KeyValuePair<string, string> keyValue in headers)
                {
                    client.DefaultRequestHeaders.Add(keyValue.Key, keyValue.Value);
                }
            }
            request.Content = new StringContent(param.ToJson(), DefaultEncoding, "application/json");
            HttpResponseMessage response = await SendAsync(client, request);
            string content = await response.Content.ReadAsStringAsync();
            return content.FromJson<O>();
        }

        /// <summary>
        /// PUT请求
        /// </summary>
        public static async Task<O> PutAsync<T, O>(string url, T param, IDictionary<string, string> headers = null)
        {
            HttpClient client = GetClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, url);
            if (!headers.IsEmpty())
            {
                foreach (KeyValuePair<string, string> keyValue in headers)
                {
                    client.DefaultRequestHeaders.Add(keyValue.Key, keyValue.Value);
                }
            }
            request.Content = new StringContent(param.ToJson(), DefaultEncoding, "application/json");
            HttpResponseMessage response = await SendAsync(client, request);
            string content = await response.Content.ReadAsStringAsync();
            return content.FromJson<O>();
        }

        /// <summary>
        /// 文件上传
        /// </summary>
        public static async Task<O> UploadAsync<O>(string url, Stream stream, string name, string fileName, IDictionary<string, string> extdata = null, IDictionary<string, string> headers = null)
        {
            HttpClient client = GetClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
            if (!headers.IsEmpty())
            {
                foreach (KeyValuePair<string, string> keyValue in headers)
                {
                    client.DefaultRequestHeaders.Add(keyValue.Key, keyValue.Value);
                }
            }
            string boundary = Guid.NewGuid().ToString();
            MultipartFormDataContent multipartFormdataContent = new MultipartFormDataContent(boundary);
            multipartFormdataContent.Headers.ContentType = new MediaTypeHeaderValue(@"multipart/form-data");
            multipartFormdataContent.Headers.ContentType.Parameters.Add(new NameValueHeaderValue("boundary", boundary));
            multipartFormdataContent.Add(new StreamContent(stream), string.Format("\"{0}\"", name), string.Format("\"{0}\"", fileName));
            if (!extdata.IsEmpty())
            {
                foreach (string key in extdata.Keys)
                {
                    multipartFormdataContent.Add(new StringContent(extdata[key]), string.Format("\"{0}\"", key));
                }
            }
            request.Content = multipartFormdataContent;
            HttpResponseMessage response = await SendAsync(client, request);
            string content = await response.Content.ReadAsStringAsync();
            return content.FromJson<O>();
        }

        /// <summary>
        /// 文件下载(GET)
        /// </summary>
        public static async Task<Stream> DownloadAsync(string url, IDictionary<string, string> headers = null)
        {
            HttpClient client = GetClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
            if (!headers.IsEmpty())
            {
                foreach (KeyValuePair<string, string> keyValue in headers)
                {
                    client.DefaultRequestHeaders.Add(keyValue.Key, keyValue.Value);
                }
            }
            HttpResponseMessage response = await SendAsync(client, request);
            return await response.Content.ReadAsStreamAsync();
        }

        /// <summary>
        /// 文件下载(POST)
        /// </summary>
        public static async Task<Stream> DownloadAsync<T>(string url, T param, IDictionary<string, string> headers = null)
        {
            HttpClient client = GetClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
            if (!headers.IsEmpty())
            {
                foreach (KeyValuePair<string, string> keyValue in headers)
                {
                    client.DefaultRequestHeaders.Add(keyValue.Key, keyValue.Value);
                }
            }
            request.Content = new StringContent(param.ToJson(), DefaultEncoding, "application/json");
            HttpResponseMessage response = await SendAsync(client, request);
            return await response.Content.ReadAsStreamAsync();
        }
    }
}
