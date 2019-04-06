using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

namespace ImageBoardProcessor.Clients
{
     public static class ApiClient
    {
        public static HttpClient WebClient { get; set; }

        public static void InitilizeClient()
        {
            WebClient = new HttpClient();            
            WebClient.DefaultRequestHeaders.Accept.Clear();
            WebClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            WebClient.DefaultRequestHeaders.Add("User-Agent", "DesktopProject/1.0 (by kaitoukitsune on e621)");
        }
    }
}
