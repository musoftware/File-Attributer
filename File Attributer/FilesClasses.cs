using System;
using System.Collections.Generic;
using System.Text;

namespace File_Attributer
{
    class FilesClasses
    {
        public List<string> files = new List<string>();

        //public HashSet<string> folders = new HashSet<string>();


        public System.IO.FileAttributes AttributeSelected { get; set; }
    }
}
