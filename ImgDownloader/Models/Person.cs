using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgDownloader.Models
{
    public class Person
    {

        public int Age { get; set; }
        public string Name { get; set; }
        public Person()
        {
            Age = 18;
            Name = "James Seward";

        }
    }
}
