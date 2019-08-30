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
using PropertyChanged;
using System.Diagnostics;
using System.Threading;

namespace ImgDownloader.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class QueryTabViewModel : BindableBase, IDataErrorInfo
    {
        public Query query { get; set; } = new Query();

        ImgBrdProcessor processor;

        public int Progress { get; set; }

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
                return query.searchTerms[0];
            }
            set
            {
                query.searchTerms[0] = value;
                search.RaiseCanExecuteChanged();
            }
        }

        public string SearchTag1
        {
            get
            {
                return query.searchTerms[1];
            }
            set
            {
                query.searchTerms[1] = value;
                search.RaiseCanExecuteChanged();
            }
        }

        public string SearchTag2
        {
            get
            {
                return query.searchTerms[2];
            }
            set
            {
                query.searchTerms[2] = value;
                search.RaiseCanExecuteChanged();
            }
        }

        public string SearchTag3
        {
            get
            {
                return query.searchTerms[3];
            }
            set
            {
                query.searchTerms[3] = value;
                search.RaiseCanExecuteChanged();
            }
        }

        public string SearchTag4
        {
            get
            {
                return query.searchTerms[4];
            }
            set
            {
                query.searchTerms[4] = value;
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
                        if (string.IsNullOrWhiteSpace(SearchName))
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
            query.searchTerms[0] = "instant_loss_2koma";
            query.searchTerms[1] = "";
            query.searchTerms[2] = "";
            query.searchTerms[3] = "";
            query.searchTerms[4] = "";
            query.downloadDirectory = @"D:\Pictures\Rouge";
            search = new DelegateCommand(async () => await Search());
            save = new DelegateCommand(SaveQuery, CanSaveQuery);
            load = new DelegateCommand(LoadQuery, CanLoadQuery);
            OpenFolder = new DelegateCommand(LoadDirectory);
            processor = new ImgBrdProcessor();
        }

        private async Task Search()
        {
            //query.ParseSearchQuery();
            Console.WriteLine($"Started at {DateTime.UtcNow.ToLongTimeString()}");
            var tasking = await Task.Run(() => processor.E621Search(query.searchTerms.ToList()));

            Console.WriteLine($"Moved on at {DateTime.UtcNow.ToLongTimeString()}");

            await Task.Run(() => download(tasking));


            Console.WriteLine($"We completed the search! we found {tasking.Count}");


        }
        private void download(IEnumerable<E621Model> results)
        {
            if (!Directory.Exists(query.downloadDirectory))
            {
                Console.WriteLine("Directory missing, making it now");
                Directory.CreateDirectory(query.downloadDirectory);
            }
            if (results.Any())
            {
                Parallel.ForEach(results, new ParallelOptions { MaxDegreeOfParallelism = 4 },
                            file =>
                            {
                                WebClient wc = new WebClient();
                                wc.DownloadFileAsync(new Uri(file.File_url), $@"{query.downloadDirectory}\{file.filename}");
                                wc.Dispose();
                            });
                Console.WriteLine("We're done!");
            }
            else
            { Console.WriteLine("We didnt get anything back!"); }

            Console.WriteLine($"SCompleted at {DateTime.UtcNow.ToLongTimeString()}");
            query.lastExecute = DateTime.UtcNow;

        }


        private bool CanSearch()
        {
            return true;
            //return query.isValid();
            //return query.isValid();
        }

        private void SaveQuery()
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
                if (diag.ShowDialog() == DialogResult.OK)
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
