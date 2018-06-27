using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace AbsUserCenter.SSOClient
{
    /// <summary>
    /// sso验证token请求
    /// </summary>
    public class BaseSsoApiReference
    {
        private static HttpClient _httpClient;
        public static string BaseApiUrl { get; set; }
        public BaseSsoApiReference(string baseurl)
        {
            BaseApiUrl = baseurl;
        }
        private static void InitHttpClient(out HttpClient httpClient)
        {
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            httpClientHandler.UseCookies = true;
            httpClient = new HttpClient(httpClientHandler);
            httpClient.BaseAddress = new Uri(BaseApiUrl);
            httpClient.Timeout = TimeSpan.FromMinutes(30);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private static object _syncRoot = new object();


        public HttpClient HttpClient
        {
            get
            {
                if (_httpClient == null)
                {
                    lock (_syncRoot)
                    {
                        HttpClient httpClient;
                        InitHttpClient(out httpClient);
                        if (_httpClient == null)
                        {
                            _httpClient = httpClient;
                        }
                    }
                }
                return _httpClient;
            }
        }
        public HttpClient HttpClientToken(string token)
        {
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            httpClientHandler.UseCookies = true;
            HttpClient httpClient = new HttpClient(httpClientHandler);
            httpClient.BaseAddress = new Uri(BaseApiUrl);
            httpClient.Timeout = TimeSpan.FromMinutes(30);
            httpClient.DefaultRequestHeaders.Add("token", token);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return httpClient;
        }



    }
}
