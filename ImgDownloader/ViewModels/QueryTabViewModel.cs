using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using Prism.Commands;
using ImageBoardProcessor.Models;
using ImageBoardProcessor.Processors;
using ImgDownloader.Models;
using System.Windows.Input;
using System.Net;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.IO;
using System.ComponentModel;
using ImageBoardProcessor.Serializers;
using System.Windows.Forms;

namespace ImgDownloader.ViewModels
{
    public class QueryTabViewModel : BindableBase, IDataErrorInfo
    {
        public Query query { get; set; } = new Query();
        public Person person { get; set; } = new Person();
        ImgBrdProcessor processor;

        public string SearchName
        {
            get
            {
                return query.searchName;
            }
            set
            {
                query.searchName = value;
                search.RaiseCanExecuteChanged();

            }
        }

        public string SearchTag0
        {
            get
            {
                return query.tag0;
            }
            set
            {
                query.tag0 = value;
                search.RaiseCanExecuteChanged();
            }
        }       

        public string SearchTag1
        {
            get
            {
                return query.tag1;
            }
            set
            {
                query.tag1 = value;
                search.RaiseCanExecuteChanged();
            }
        }

        public string SearchTag2
        {
            get
            {
                return query.tag2;
            }
            set
            {
                query.tag2= value;
                search.RaiseCanExecuteChanged();
            }
        }

        public string SearchTag3
        {
            get
            {
                return query.tag3;
            }
            set
            {
                query.tag3 = value;
                search.RaiseCanExecuteChanged();
            }
        }

        public string SearchTag4
        {
            get
            {
                return query.tag4;
            }
            set
            {
                query.tag4 = value;
                search.RaiseCanExecuteChanged();
            }
        }

        public string SearchDir
        {
            get
            {
                return query.downloadDirectory;
            }
            set
            {
                query.downloadDirectory = value;
                search.RaiseCanExecuteChanged();


            }
        }






        public bool isSearch { get; set; }

        

        public DelegateCommand search { get; private set; }
        public DelegateCommand save { get; private set; }
        public DelegateCommand load { get; private set; }
        public DelegateCommand OpenFolder { get; private set; }

        public string Error => throw new NotImplementedException();

        public string this[string columnName]
        {
            get
            {
                string result = null;
                switch (columnName)
                {
                    case "SearchName":
                        if(string.IsNullOrWhiteSpace(SearchName))
                            result = "The search name cannot be empty";
                        break;
                    case "SearchTag0":
                        if (string.IsNullOrWhiteSpace(SearchTag0))
                            result = "The first tag must have a value";
                        if (SearchTag0.Contains(' '))
                            result = "Tags Cannot contain spaces";
                        break;
                    case "SearchTag1":
                        
                        if (SearchTag1.Contains(' '))
                            result = "Tags Cannot contain spaces";
                        break;
                    case "SearchTag2":
                        if (SearchTag2.Contains(' '))
                            result = "Tags Cannot contain spaces";
                        break;
                    case "SearchTag3":
                        if (SearchTag3.Contains(' '))
                            result = "Tags Cannot contain spaces";
                        break;
                    case "SearchTag4":
                        if (SearchTag4.Contains(' '))
                            result = "Tags Cannot contain spaces";
                        break;
                    case "SearchDir":
                        if (SearchDir.EndsWith(@"\"))
                            result = @"Remove the last \";
                        if (string.IsNullOrWhiteSpace(SearchDir))
                            result = "Must have a value";
                        break;
                    default:
                        break;
                }
                return result;

                
            }



        }

        public QueryTabViewModel()
        {
            query.searchName = "E621";
            query.tag0 = "instant_loss_2koma";
            query.tag1 = "";
            query.tag2 = "";
            query.tag3 = "";
            query.tag4 = "";
            query.downloadDirectory = @"D:\Pictures\Rouge";
            search  =   new DelegateCommand(Search, CanSearch);
            save    =   new DelegateCommand(SaveQuery, CanSaveQuery);
            load = new DelegateCommand(LoadQuery, CanLoadQuery);
            OpenFolder = new DelegateCommand(LoadDirectory);
            processor = new ImgBrdProcessor();
        }

        private async void Search()
        {
            query.ParseSearchQuery();
            List<E621Model> result = await processor.E621Search(query.searchTerms.ToList());
            
            Console.WriteLine($"We completed the search! we found {result.Count}");
            if(!Directory.Exists(query.downloadDirectory))
            {
                Console.WriteLine("Directory missing, making it now");
                Directory.CreateDirectory(query.downloadDirectory);
            }
            if (result.Any())
            {
                Parallel.ForEach(result, new ParallelOptions { MaxDegreeOfParallelism = 4 },
                            file =>
                            {
                                WebClient wc = new WebClient();
                                wc.DownloadFile(file.File_url,$@"{query.downloadDirectory}\{file.filename}");                                
                                wc.Dispose();
                            });
                Console.WriteLine("We're done!");
                query.lastExecute = DateTime.UtcNow;
            }
            else
            { Console.WriteLine("We didnt get anything back!"); }
            
        }

        private bool CanSearch()
        {
            //return query.isValid();
            return query.isValid();
        }

        private  void SaveQuery()
        {
            
            QuerySerilizer.SaveQuery(query);
        }

        private bool CanSaveQuery()
        {
            return query.isValid();
           
        }

        private void LoadQuery()
        {
            
            using (OpenFileDialog diag = new OpenFileDialog())
            {
                diag.Filter = "xml Files (*.xml)|*.xml";
                if (diag.ShowDialog() == DialogResult.OK)
                {
                    query = QuerySerilizer.LoadQuery(diag.FileName);
                }
            }
        }

        private void LoadDirectory()
        {
            using (FolderBrowserDialog diag = new FolderBrowserDialog())
            {
                if(diag.ShowDialog() == DialogResult.OK)
                {
                    SearchDir = diag.SelectedPath;
                }
            }
        }


        private bool CanLoadQuery()
        {
            return true;
        }

       
    }
}
