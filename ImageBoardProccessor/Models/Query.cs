using System;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Specialized;
using ImageBoardProcessor.Enumerations;


namespace ImageBoardProcessor.Models
{
    [Serializable()]
    public class Query
    {
        const string E621BASEURL = "https://e621.net/post/index.json";
        const string GELBOORUBASEURL = "https://e621.net/post/index.json";

        const string E621BASEQUERYAPPEND = "&limit=320&before_id=";
        const string PROGRAMID = "ImgBoardDownloader/Alpha(kaitoukitsune)";
        [XmlAttribute]
        public virtual string searchName { get; set; }          
        public virtual string tag0 { get; set; }
        public virtual string tag1 { get; set; }
        public virtual string tag2 { get; set; }
        public virtual string tag3 { get; set; }
        public virtual string tag4 { get; set; }

        [XmlIgnore]
        public string[] searchTerms { get; set; }
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
            if(String.IsNullOrEmpty(query.searchName) || String.IsNullOrEmpty(query.searchName) || 
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
        UriBuilder URLbuilder;
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
            searchBlackList.Add("Fool");
            sortTags.Add("deadweight");
            URLbuilder = new UriBuilder();



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
