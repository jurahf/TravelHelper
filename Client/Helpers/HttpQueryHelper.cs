using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace Client.Helpers
{
    public static class HttpQueryHelper
    {
        private static string serviceUrl = ConfigurationManager.AppSettings["ServerAddress"];

        public static HttpClient CreateHttpClient()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(serviceUrl);
            // TODO: auth info
            return client;
        }
    }
}