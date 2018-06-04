using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using JuCheap.Infrastructure.Extentions;

namespace JuCheap.Infrastructure.Utilities
{
    public class HttpRequestProvider
    {
        /// <summary>
        /// 发送Http请求
        /// </summary>
        /// <param name="method">方式</param>
        /// <param name="url">请求地址</param>
        /// <param name="parameters">参数(可为空)</param>
        /// <param name="encoding">编码方式</param>
        /// <param name="timeout">超时时间</param>
        /// <param name="ip">代理ip地址</param>
        /// <param name="port">代理端口</param>
        /// <returns></returns>
        public string Send(HttpMethod method, string url, IList<Parameter> parameters, Encoding encoding, int timeout = 15, string ip = "", int port = 0)
        {
            var parameterPart = parameters == null ? string.Empty : parameters.Join("&", kv => kv.Key + "=" + kv.Value);

            if (method != HttpMethod.Post)
            {
                if (parameterPart != string.Empty)
                    url += "?" + parameterPart;
            }
            if (method == HttpMethod.None) return url;

            var webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Timeout = timeout * 1000;
            if (ip.IsNotBlank() && port > 0)
            {
                webRequest.Proxy = new WebProxy(ip, port);
            }

            if (method == HttpMethod.Post)
            {
                if (parameterPart != string.Empty)
                {
                    var byteRequestParamters = encoding.GetBytes(parameterPart);

                    webRequest.Method = "POST";
                    webRequest.ContentType = "application/x-www-form-urlencoded";
                    webRequest.ContentLength = byteRequestParamters.Length;
                    
                    using (var requestStream = webRequest.GetRequestStream())
                    {
                        requestStream.Write(byteRequestParamters, 0, byteRequestParamters.Length);
                    }
                }
            }
            

            using (var responseStream = webRequest.GetResponse().GetResponseStream())
            {
                using (var streamReader = new StreamReader(responseStream, encoding))
                {
                    return streamReader.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// 异步发送Http请求
        /// </summary>
        /// <param name="method">方式</param>
        /// <param name="url">请求地址</param>
        /// <param name="parameters">参数(可为空)</param>
        /// <param name="encoding">编码方式</param>
        /// <param name="timeout">超时时间</param>
        /// <returns></returns>
        public async Task<string> SendAsync(HttpMethod method, string url, IList<Parameter> parameters, Encoding encoding, int timeout = 15)
        {
            var parameterPart = parameters == null ? string.Empty : parameters.Join("&", kv => kv.Key + "=" + kv.Value);

            if (method != HttpMethod.Post)
            {
                if (parameterPart != string.Empty)
                    url += "?" + parameterPart;
            }
            if (method == HttpMethod.None) return url;

            var webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Timeout = timeout * 1000;

            if (method == HttpMethod.Post)
            {
                if (parameterPart != string.Empty)
                {
                    var byteRequestParamters = encoding.GetBytes(parameterPart);

                    webRequest.Method = "POST";
                    webRequest.ContentType = "application/x-www-form-urlencoded";
                    webRequest.ContentLength = byteRequestParamters.Length;

                    using (var requestStream = webRequest.GetRequestStream())
                    {
                        await requestStream.WriteAsync(byteRequestParamters, 0, byteRequestParamters.Length);
                    }
                }
            }
            using (var responseStream = webRequest.GetResponse().GetResponseStream())
            {
                using (var streamReader = new StreamReader(responseStream, encoding))
                {
                    return await streamReader.ReadToEndAsync();
                }
            }
        }

        /// <summary>
        /// 请参数转化为Json后POST提交
        /// </summary>
        /// <param name="url"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public string PostAsJson(string url, object content)
        {
            using (var client = new HttpClient())
            {
                var requestJson = JsonConvert.SerializeObject(content);

                var httpContent = new StringContent(requestJson);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var httpResponseMessage = client.PostAsync(url, httpContent).Result;

                var result = httpResponseMessage.Content.ReadAsStringAsync().Result;

                return result;
            }
        }

        /// <summary>
        /// Get
        /// </summary>
        /// <param name="url"></param>
        /// <param name="parameters"></param>
        /// <param name="ip">代理ip地址</param>
        /// <param name="port">代理端口</param>
        /// <returns></returns>
        public string Get(string url, IList<Parameter> parameters = null, string ip = "", int port = 0)
        {
            return Send(HttpMethod.Get, url, parameters, Encoding.UTF8, 15, ip, port);
        }
    }

    /// <summary>
    /// http请求参数
    /// </summary>
    public class Parameter
    {
        public string Key { set; get; }

        public string Value { set; get; }

        public bool NotSign { set; get; }
    }

    /// <summary>
    /// http请求方式
    /// </summary>
    public enum HttpMethod
    {
        /// <summary>
        /// 不实际执行Http请求，但要返回查询字符串
        /// </summary>
        None = 0,
        Post = 1,
        Get = 2
    }
}
