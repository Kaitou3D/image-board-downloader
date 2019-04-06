using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ImageBoardProcessor.Interfaces;
using ImageBoardProcessor.Models;
using ImageBoardProcessor.Clients;
using System.Drawing;
using System.IO;
using System.Collections.Concurrent;
using ImageBoardProcessor.Models;

namespace ImageBoardProcessor.Processors
{
    public class ImgBrdProcessor 
    {
        // An example query:
        //  https://e621.net/post/index.json?tags=fluffy+rating:s&limit=320&before_id=1751083
        //  https://e621.net/post/index.json?tags=fluffy+rating:s&limit=320
        const string BASEURL = "https://e621.net/post/index.json";

        const string BASEQUERYAPPEND = "&limit=320&before_id=";

        const string PROGRAMID = "DesktopProject/Alpha(kaitoukitsune)";
               

        static UriBuilder url = new UriBuilder(BASEURL);   
       

        /// <value>
        /// The query we plan to run
        /// </value>
        public static Query query { get; set ; }

        

        public ImgBrdProcessor()
        {
            
        }

        /// <summary>
        /// Execute the query
        /// </summary>
        
        public async Task<List<E621Model>> E621Search(List<string> tags ) 
        {
            List<E621Model>files = new List<E621Model>();
            string beforeID = "";

            //Form the query string and add it to the URI        
            string query = "tags=" + string.Join("+", tags) + BASEQUERYAPPEND;
            
            url.Query = query;              
                        
            //The first time we iterate through our request lists, BEFORE_ID will be null. If we have over 320 results from the
            //last query, take the last model ID from the list and set BEFORE_ID to it. Repeat the qeury until we have reached the end
            //of of the sites list
            bool cont = false;
            do
            {
                List<E621Model> results = new List<E621Model>();
                ApiClient.InitilizeClient();

                HttpResponseMessage response = await ApiClient.WebClient.GetAsync(url.Uri);
                {
                    if (response.IsSuccessStatusCode)
                    {
                         results.AddRange(await response.Content.ReadAsAsync<List<E621Model>>());
                    }
                    else
                    {
                        throw new Exception(response.ReasonPhrase);
                    }
                    
                }
                                
                beforeID = results.Last().Id.ToString();

                files.AddRange(results);

                if(results.Count ==320)
                {
                    cont = true;
                    url.Query = query + beforeID;

                }
                else
                {
                    cont = false;
                }

            } while (cont);
            
            return files;
        }      

        public async Task<List<Rule34Model>> Rule34Search(Query search)
        {

        }
        

        public async Task DownloadFiles(IProgress<DownloadProgress> progress)
        {
            var files = new List<E621Model>();
            ConcurrentQueue<E621Model> foo = new ConcurrentQueue<E621Model>();
            DownloadProgress dp = new DownloadProgress();
            var client = new WebClient();
            

            dp.Total = foo.Count;
           
            foreach (var item in foo)
            {
                client.DownloadFileAsync(new Uri(item.File_url), item.filename);
                
            }
        }
    }
}
