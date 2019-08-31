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
using System.Collections.ObjectModel;
using System.Windows.Data;

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


        public ObservableCollection<string> ResultList { get; set; } = new ObservableCollection<string>();

        private object _lock = new object();
        private ObservableCollection<string> _data;

        public DelegateCommand search { get; private set; }
        public DelegateCommand save { get; private set; }
        public DelegateCommand load { get; private set; }
        public DelegateCommand OpenFolder { get; private set; }

        public string Error => throw new NotImplementedException();

        /// <summary>
        /// Error string for when something is not correct with the form. Displayed to user
        /// </summary>
        /// <param name="columnName"> Which control is producing the error</param>
        /// <returns></returns>
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

            //BindingOperations.EnableCollectionSynchronization(_data, _lock);
        }

        /// <summary>
        /// Executes the query agsint the image board
        /// </summary>
        /// <returns></returns>
        private async Task Search()
        {
            //query.ParseSearchQuery();
            Console.WriteLine($"Started at {DateTime.UtcNow.ToLongTimeString()}");

            var tasking = await Task.Run(() => processor.E621Search(query.searchTerms.ToList()));

            Console.WriteLine($"Moved on at {DateTime.UtcNow.ToLongTimeString()}");
            Progress<DownloadProgress> progress = new Progress<DownloadProgress>();
            progress.ProgressChanged += ReportProgress;

            await Task.Run(() => download(tasking, progress));

            Console.WriteLine($"We completed the search! we found {tasking.Count}");


        }

        private void ReportProgress(object sender, DownloadProgress e)
        {
            Progress = (e.FilesDownloaded.Count() * 100) / e.TotalToDownload;
            /*
            lock(_data)
            {
               
            }
            */
        }

        /// <summary>
        /// Parses through the query and begins downloading the images
        /// </summary>
        /// <param name="queryResults"> the list of items to download</param>
        private void download(IEnumerable<IFile> queryResults, IProgress<DownloadProgress> progress)
        {
            DownloadProgress report = new DownloadProgress();
            report.TotalToDownload = queryResults.Count();
            if (!Directory.Exists(query.downloadDirectory))
            {
                Console.WriteLine("Directory missing, making it now");
                Directory.CreateDirectory(query.downloadDirectory);
            }
            if (queryResults.Any())
            {
                foreach (var file in queryResults)
                {
                    WebClient wc = new WebClient();
                    wc.DownloadFileAsync(new Uri(file.File_url), $@"{query.downloadDirectory}\{file.filename}");
                    report.FilesDownloaded.Add(file.filename);
                    report.TotalDownloaded++;
                    progress.Report(report);
                    wc.Dispose();

                }
                /*
                Parallel.ForEach(queryResults, new ParallelOptions { MaxDegreeOfParallelism = 4 },
                            file =>
                            {


                            });
                            */
                Console.WriteLine("We're done!");
            }
            else
            { Console.WriteLine("We didnt get anything back!"); }

            Console.WriteLine($"Completed at {DateTime.UtcNow.ToLongTimeString()}");
            query.lastExecute = DateTime.UtcNow;

        }

        /// <summary>
        /// Validates the state of the query for execution
        /// </summary>
        /// <returns>True = proceed with execution</returns>
        private bool CanSearch()
        {
            return true;
            //return query.isValid();
            //return query.isValid();
        }

        /// <summary>
        /// Writes out the query to file
        /// </summary>
        private void SaveQuery()
        {

            QuerySerilizer.SaveQuery(query);
        }

        /// <summary>
        /// Validates the state of the query for saving
        /// </summary>
        /// <returns>True: Proceed with writing</returns>
        private bool CanSaveQuery()
        {
            return query.isValid();

        }

        /// <summary>
        /// Loads a query from disk
        /// </summary>
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

        /// <summary>
        /// Loads a directory to the form
        /// </summary>
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

        /// <summary>
        /// Validates the state of the program  for loading a query
        /// </summary>
        /// <returns>True: proceed with load</returns>
        private bool CanLoadQuery()
        {
            return true;
        }


    }
}
