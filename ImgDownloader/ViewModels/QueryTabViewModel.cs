using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using Prism.Commands;
using ImageBoardProcessor.Models;
using ImageBoardProcessor.Processors;
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
    /// <summary>
    /// Main source 
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public class QueryTabViewModel :  IDataErrorInfo
    {
        private CancellationTokenSource _tokensource;

        private CancellationToken _token;

        ImgBrdProcessor processor;

        /// <value>
        /// Holds the  query information
        /// </value>
        public Query QueryObj { get; set; } = new Query();
     
        public int Progress { get; set; } = 0;
      
        public string SearchName
        {
            get
            {
                return QueryObj.searchName;
            }
            set
            {
                QueryObj.searchName = value;
                search.RaiseCanExecuteChanged();

            }
        }

        public string SearchTag0
        {
            get
            {
                return QueryObj.searchTerms[0];
            }
            set
            {
                QueryObj.searchTerms[0] = value;
                search.RaiseCanExecuteChanged();
            }
        }

        public string SearchTag1
        {
            get
            {
                return QueryObj.searchTerms[1];
            }
            set
            {
                QueryObj.searchTerms[1] = value;
                search.RaiseCanExecuteChanged();
            }
        }

        public string SearchTag2
        {
            get
            {
                return QueryObj.searchTerms[2];
            }
            set
            {
                QueryObj.searchTerms[2] = value;
                search.RaiseCanExecuteChanged();
            }
        }

        public string SearchTag3
        {
            get
            {
                return QueryObj.searchTerms[3];
            }
            set
            {
                QueryObj.searchTerms[3] = value;
                search.RaiseCanExecuteChanged();
            }
        }

        public string SearchTag4
        {
            get
            {
                return QueryObj.searchTerms[4];
            }
            set
            {
                QueryObj.searchTerms[4] = value;
                search.RaiseCanExecuteChanged();
            }
        }

        public string SearchDir
        {
            get
            {
                return QueryObj.downloadDirectory;
            }
            set
            {
                QueryObj.downloadDirectory = value;
                search.RaiseCanExecuteChanged();


            }
        }

        public bool isSearch { get; set; }

        public ObservableCollection<string> ResultList { get; set; } = new ObservableCollection<string>();

        public DelegateCommand search { get; private set; }
        public DelegateCommand save { get; private set; }
        public DelegateCommand load { get; private set; }
        public DelegateCommand cancel { get; private set; }
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
            QueryObj.searchName = "E621";
            QueryObj.searchTerms[0] = "Male";
            QueryObj.searchTerms[1] = "";
            QueryObj.searchTerms[2] = "";
            QueryObj.searchTerms[3] = "";
            QueryObj.searchTerms[4] = "";
            QueryObj.downloadDirectory = "";
            search = new DelegateCommand(async () => await Search(), CanSearch);
            save = new DelegateCommand(SaveQuery, CanSaveQuery);
            load = new DelegateCommand(LoadQuery, CanLoadQuery);
            cancel = new DelegateCommand(CancelSearch);
            OpenFolder = new DelegateCommand(LoadDirectory);
            processor = new ImgBrdProcessor();

            _tokensource = new CancellationTokenSource();
            _token = _tokensource.Token;

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

            try
            {
                var tasking = await Task.Run(() => processor.E621Search(QueryObj.searchTerms.ToList()));
                Console.WriteLine($"Moved on at {DateTime.UtcNow.ToLongTimeString()}");
                Progress<DownloadProgress> progress = new Progress<DownloadProgress>();
                progress.ProgressChanged += ReportProgress;

                await Task.Run(() => download(tasking, progress, _token));

                Console.WriteLine($"We completed the search! we found {tasking.Count}");

            }
            catch (ArgumentException e)
            {

                MessageBox.Show(e.Message, "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (System.Net.Http.HttpRequestException e)
            {
                MessageBox.Show(e.Message, "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }



        }

        /// <summary>
        /// Parses through the query and begins downloading the images
        /// </summary>
        /// <param name="queryResults"> the list of items to download</param>
        private void download(IEnumerable<IFile> queryResults, IProgress<DownloadProgress> progress, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }
            DownloadProgress report = new DownloadProgress();
            report.TotalToDownload = queryResults.Count();
            if (!Directory.Exists(QueryObj.downloadDirectory))
            {
                Console.WriteLine("Directory missing, making it now");
                Directory.CreateDirectory(QueryObj.downloadDirectory);
            }
            if (queryResults.Any())
            {
             
                foreach (var file in queryResults)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        return;
                    }
                    WebClient wc = new WebClient();
                    wc.DownloadFile(new Uri(file.File_url), $@"{QueryObj.downloadDirectory}\{file.filename}");
                    report.FilesDownloaded.Add(file.filename);
                    progress.Report(report);
                    wc.Dispose();

                }

                Console.WriteLine("We're done!");
            }
            else
            { Console.WriteLine("We didnt get anything back!"); }

            Console.WriteLine($"Completed at {DateTime.UtcNow.ToLongTimeString()}");
            QueryObj.lastExecute = DateTime.UtcNow;

        }

        /// <summary>
        /// Report function that commicates updates to UI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReportProgress(object sender, DownloadProgress e)
        {
            Progress = e.PercentComplete;
            ResultList = new ObservableCollection<string>(e.FilesDownloaded);
        }

        /// <summary>
        /// Validates the state of the query for execution
        /// </summary>
        /// <returns>True = proceed with execution</returns>
        private bool CanSearch()
        {
            if(true)
            return true;
            //return query.isValid();
            //return query.isValid();
        }

        private void CancelSearch()
        {
            _tokensource.Cancel();
        }

        /// <summary>
        /// Writes out the query to file
        /// </summary>
        private void SaveQuery()
        {
            if(String.IsNullOrWhiteSpace(SearchDir))
            {
                using (FolderBrowserDialog diag = new FolderBrowserDialog())
                {
                    diag.Description = "Choose a Destination";
                    diag.RootFolder = Environment.SpecialFolder.MyComputer;
                    if (diag.ShowDialog() == DialogResult.OK)
                    {
                        if (!Directory.Exists(diag.SelectedPath))
                            Directory.CreateDirectory(diag.SelectedPath);
                        SearchDir = diag.SelectedPath;
                    }
                }

            }
            QuerySerilizer.SaveQuery(QueryObj);
        }

        /// <summary>
        /// Validates the state of the query for saving
        /// </summary>
        /// <returns>True: Proceed with writing</returns>
        private bool CanSaveQuery()
        {
            return QueryObj.isValid();

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
                    QueryObj = QuerySerilizer.LoadQuery(diag.FileName);
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
                diag.Description = "Choose a Destination";
                diag.RootFolder = Environment.SpecialFolder.MyComputer;
                if (diag.ShowDialog() == DialogResult.OK)
                {
                    if (!Directory.Exists(diag.SelectedPath))
                        Directory.CreateDirectory(diag.SelectedPath);
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
