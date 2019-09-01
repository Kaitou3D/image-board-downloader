using System;
using System.Collections.Generic;
using System.Text;

namespace ImageBoardProcessor.Models
{
    /// <summary>
    /// Conatins current download information
    /// </summary>
    public class DownloadProgress
    {
        /// <value>
        /// Percatge of download complete
        /// </value>
        public int PercentComplete
        {
            get
            {
                int value = 0;
                if(FilesDownloaded.Count > 0 &&  TotalToDownload != 0)
                    value = (FilesDownloaded.Count * 100) / TotalToDownload;
                return value;
            }
        }
        
        /// <value>
        /// List of currently downloaded files
        /// </value>
        public List<string> FilesDownloaded { get; set; } = new List<string>();

        /// <value>
        /// Total Number of files we are going to download
        /// </value>
        public int TotalToDownload { get; set; }
        
    }
}
