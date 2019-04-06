using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;



namespace ImageBoardProcessor.Models
{
    
    public class E621Model : IFile
    {
        public int Id { get; set; }
        public string Tags { get; set; }
        public int File_size { get; set; }
        public string File_url { get; set; }
        public string File_ext { get; set; }
        public List<string> Artist { get; set; }
        public char Rating { get; set; }

        public string filename
        {
            get
            {
                return $"{Id}.{File_ext}";
            }
        }

        public int CompareTo(E621Model other)
        {
            return Id.CompareTo(other.Id);
        }
        public List<string> GetTagsList()
        {
            List<string> result = Tags.Split(' ').ToList();
            return result;
        }
        public override string ToString()
        {
            return $"{Id} - {Artist.FirstOrDefault()}";
        }
    }
}
