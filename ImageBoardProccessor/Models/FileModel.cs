using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageBoardProcessor.Models
{
    public class FileModel
    {
        public string fileName { get; set; }
        public StringCollection fileTags { get; set; }
        public Uri fileURL { get; set; }
        public bool isDownloaded { get; set; }
    }
}
