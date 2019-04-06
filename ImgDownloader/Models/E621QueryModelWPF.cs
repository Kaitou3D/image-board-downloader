using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ImageBoardProcessor.Models;

namespace ImgDownloader.Models
{

    public class E621QueryModelWPF : Query, IDataErrorInfo
    {



        public string this[string columnName]
        {
            get
            {
                string result = null;
                if (columnName == "searchName")
                {
                    if(string.IsNullOrWhiteSpace(searchName))
                        result = "The search name cannot be empty";
                }
                if (columnName == "tag0")
                {
                    if (string.IsNullOrWhiteSpace(tag0))
                        result = "The first tag must have a value";
                    if (tag0.Contains(' '))
                        result = "Tags Cannot contain spaces";
                }
                if (columnName == "tag1")
                {
                    if (tag1.Contains(' '))
                        result = "Tags Cannot contain spaces";
                }
                if (columnName == "tag2")
                {
                    if (tag2.Contains(' '))
                        result = "Tags Cannot contain spaces";
                }
                if (columnName == "tag3")
                {
                    if (tag3.Contains(' '))
                        result = "Tags Cannot contain spaces";
                }
                if (columnName == "tag4")
                {
                    if (tag4.Contains(' '))
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
