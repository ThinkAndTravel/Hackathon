using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace AppTandT.BLL.Services
{
    public class ClientService
    {
        protected static HttpClient GetClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            return client;
        }

        protected class JwtModel
        {
            public bool status { get; set; }
            public string message { get; set; }
        }
    }
}
