using System;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Specialized;
using ImageBoardProcessor.Enumerations;
using System.Web;

namespace ImageBoardProcessor.Models
{
    [Serializable()]
    public class Query
    {
        const string E621BASEURL = "https://e621.net/post/index.json";
        const string GELBOORUBASEURL = "https://e621.net/post/index.json";
        const string RULE34BASEURL = "https://rule34.xxx//index.php";

        const string PROGRAMID = "ImgBoardDownloader/Alpha(kaitoukitsune)";
        [XmlAttribute]
        public string searchName { get; set; }
        public string[] searchTerms { get; set; } = new string[5];
        public StringCollection searchBlackList { get; set; } = new StringCollection();
        public bool useGlobalBlacklist { get; set; }
        public StringCollection sortTags { get; set; } = new StringCollection();
        public bool useSortTags { get; set; }
        public DateTime lastExecute { get; set; }
        public string downloadDirectory { get; set; }
        public QueryType SearchType { get; set; }

        public UriBuilder URLbuilder { get; private set; }
        

        public Query(QueryType queryType)
        {
            SearchType = queryType;
            switch (queryType)
            {
                case QueryType.E621:
                    URLbuilder = new UriBuilder(E621BASEURL);
                    var E621query = HttpUtility.ParseQueryString(URLbuilder.Query);
                    E621query["limit"] = 32.ToString();
                    E621query["beforeID"] = "";
                    E621query["tags"] = "";
                    URLbuilder.Query = E621query.ToString();
                    break;
                case QueryType.Rule34:
                    URLbuilder = new UriBuilder(RULE34BASEURL);
                    var Rule34query = HttpUtility.ParseQueryString(URLbuilder.Query);
                    Rule34query["page"] = "dapi";
                    Rule34query["s"] = "post";
                    Rule34query["pid"] = "0";
                    Rule34query["q"] = "index";
                    URLbuilder.Query = Rule34query.ToString();
                    break;
                case QueryType.Booru:
                    break;
                default:
                    break;
            }

        }

        public Query()
        {

        }

        /// <summary>
        /// Takes the current search terms and generate the query
        /// </summary>
        public void FinalizeQuery()
        {
            if (string.IsNullOrWhiteSpace(searchTerms[0]))
            {
                throw new ArgumentException("The first tag in the array is invalid.", "searchTerms[0]");
            }
            var query = HttpUtility.ParseQueryString(URLbuilder.Query);
            string tagcombined = string.Join("+", searchTerms);
            query["tags"] = tagcombined;
            //HttpUtility escapes the \ and + signs, need to get them back.
            URLbuilder.Query = HttpUtility.UrlDecode(query.ToString());

        }

        public Uri GetQuery(string[] tags )
        {
            if(tags.Length == 0 || tags.Length>5)
            {
                throw new ArgumentOutOfRangeException("tags","The incoming arry must be at least length of 1 and not longer than 5");
            }
            
            
            return URLbuilder.Uri;
        }

        public bool isValid()
        {
            if (string.IsNullOrWhiteSpace(searchName))
                throw new ArgumentException("SearchNAme is either null or contains and invalid character",nameof(searchName));
            if (string.IsNullOrEmpty(searchTerms[0]))
                throw new ArgumentException("The first search term is either null or contains and invalid character", nameof(searchTerms));
            foreach (var term in searchTerms)
            {
                if (term.Contains(" "))
                    throw new ArgumentException("A term in the array contains a space", nameof(searchTerms));
            }

            return true;

        }
         public void IAMNEW()
        {

        }

    }
}
