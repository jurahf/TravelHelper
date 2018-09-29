using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace Client.Helpers
{
    public static class HttpQueryHelper
    {
        private const string serviceUrl = "http://localhost:62012";        // TODO: в конфиг, или DI

        public static HttpClient CreateHttpClient()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(serviceUrl);
            // TODO: auth info
            return client;
        }
    }
}