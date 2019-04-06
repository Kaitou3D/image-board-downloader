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

        const string E621BASEQUERYAPPEND = "&limit=320&before_id=";
        const string RULE34QUERYAPPEND = "&s=post&q=index";
        const string PROGRAMID = "ImgBoardDownloader/Alpha(kaitoukitsune)";
        [XmlAttribute]
        public virtual string searchName { get; set; }
        public virtual string tag0 { get; set; }
        public virtual string tag1 { get; set; }
        public virtual string tag2 { get; set; }
        public virtual string tag3 { get; set; }
        public virtual string tag4 { get; set; }

        [XmlIgnore]
        public string[] searchTerms { get; set; } = new string[5];
        public virtual StringCollection searchBlackList { get; set; } = new StringCollection();
        public bool useGlobalBlacklist { get; set; }
        public StringCollection sortTags { get; set; } = new StringCollection();
        public bool useSortTags { get; set; }
        public DateTime lastExecute { get; set; }
        public virtual string downloadDirectory { get; set; }
        public QueryType SearchType { get; set; }

        public static bool IsProper(Query query)
        {
            bool result;
            if (String.IsNullOrEmpty(query.searchName) || String.IsNullOrEmpty(query.searchName) ||
                query.searchTerms.All(term => string.IsNullOrEmpty(term)))
            {
                result = false;
            }
            else
            {
                result = true;
            }


            return result;
        }
        public UriBuilder URLbuilder;
        string BooruBase;


        public Query()
        { searchTerms = new string[5];
            tag0 = string.Empty;
            tag1 = string.Empty;
            tag2 = string.Empty;
            tag3 = string.Empty;
            tag4 = string.Empty;
            searchTerms[0] = string.Empty;
            searchTerms[1] = string.Empty;
            searchTerms[2] = string.Empty;
            searchTerms[3] = string.Empty;
            searchTerms[4] = string.Empty;

            URLbuilder = new UriBuilder();



        }

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

            tag0 = string.Empty;
            tag1 = string.Empty;
            tag2 = string.Empty;
            tag3 = string.Empty;
            tag4 = string.Empty;

            searchTerms = new string[] {tag0,tag1,tag2,tag3,tag4 };
        }


        public void ParseSearchQuery()
        {
            searchTerms[0] = tag0;
            searchTerms[1] = tag1;
            searchTerms[2] = tag2;
            searchTerms[3] = tag3;
            searchTerms[4] = tag4;
            URLbuilder.Query = string.Join("+", searchTerms);
           


        }

        public Uri GetQuery(string[] tags )
        {
            if(tags.Length == 0 || tags.Length>5)
            {
                throw new ArgumentOutOfRangeException("tags","The incoming arry must be at least length of 1 and not longer than 5");
            }
            if(string.IsNullOrWhiteSpace(tags[0]))
            {
                throw new ArgumentException("The first tag in the array is invalid.", "tags");
            }
            var query = HttpUtility.ParseQueryString(URLbuilder.Query);
            string tagcombined = string.Join("+", tags);
            query["tags"] = tagcombined;
            
            URLbuilder.Query = HttpUtility.UrlDecode(query.ToString());
            return URLbuilder.Uri;
        }

        public bool isValid()
        {
            if (string.IsNullOrWhiteSpace(searchName))
                return false;
            if (string.IsNullOrEmpty(tag0) || tag0.Contains(' '))
                return false;
            if (tag1.Contains(' '))
                return false;
            if (tag2.Contains(' '))
                return false;
            if (tag3.Contains(' '))
                return false;
            if (tag4.Contains(' '))
                return false;
            if (string.IsNullOrWhiteSpace(downloadDirectory) || (!char.IsLetterOrDigit(downloadDirectory.Last())))
                return false;



            return true;
        }


    }
}
