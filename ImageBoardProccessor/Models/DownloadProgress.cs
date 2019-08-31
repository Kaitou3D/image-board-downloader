using System;
using System.Collections.Generic;
using System.Text;

namespace ImageBoardProcessor.Models
{
    public class DownloadProgress
    {

        public int PercentCompete
        {
            get; set;
        }

        public List<string> FilesDownloaded { get; set; } = new List<string>();

        public int TotalToDownload { get; set; }

        public int TotalDownloaded { get; set; }

        
    }
}
