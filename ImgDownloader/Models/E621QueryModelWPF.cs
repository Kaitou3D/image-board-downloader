using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ImageBoardProcessor.Models;
using ImageBoardProcessor.Enumerations;

namespace ImgDownloader.Models
{

    public class E621QueryModelWPF : Query, IDataErrorInfo
    {
        public E621QueryModelWPF(QueryType queryType) : base(queryType)
        {
        }

        public string this[string columnName]
        {
            get
            {
                string result = null;
                if (columnName == "searchName")
                {
                    if (string.IsNullOrWhiteSpace(searchName))
                        result = "The search name cannot be empty";
                }
                if (columnName == "searchterms[0]")
                {
                    if (string.IsNullOrWhiteSpace(searchTerms[0]))
                        result = "The first tag must have a value";
                    if (searchTerms[0].Contains(' '))
                        result = "Tags Cannot contain spaces";
                }
                if (columnName == "searchterms[1]")
                {
                    if (searchTerms[1].Contains(' '))
                        result = "Tags Cannot contain spaces";
                }
                if (columnName == "searchterms[2]")
                {
                    if (searchTerms[2].Contains(' '))
                        result = "Tags Cannot contain spaces";
                }
                if (columnName == "searchterms[3]")
                {
                    if (searchTerms[3].Contains(' '))
                        result = "Tags Cannot contain spaces";
                }
                if (columnName == "searchterms[4]")
                {
                    if (searchTerms[4].Contains(' '))
                        result = "Tags Cannot contain spaces";
                }
                if (columnName == "downloadDirectory")
                {
                    if (downloadDirectory.EndsWith(@"\"))
                        result = @"Remove the last \";
                    if (string.IsNullOrWhiteSpace(downloadDirectory))
                        result = "Must have a value";
                }

                return result;
            }



        }

        public string Error { get { return this[null]; } }


    }
}
