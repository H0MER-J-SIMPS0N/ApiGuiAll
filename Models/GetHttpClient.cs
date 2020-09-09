using System.Net.Http;
using System;

namespace ApiGuiAll.Models
{
    public class GetHttpClient
    {
        private static HttpClient httpClient;
        private static object syncRoot = new object();
        public static HttpClient Get()
        {
            if (httpClient is null)
                lock (syncRoot)
                {
                    if (httpClient is null)
                    {
                        httpClient = new HttpClient();
                        httpClient.Timeout = new TimeSpan(0, 3, 0);
                    }
                }
            return httpClient;
        }
    }
}