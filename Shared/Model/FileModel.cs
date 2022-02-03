using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkingWithFiles.Shared.Model
{
    public class FileModel
    {
        public string? FileName { get; set; }
        public string? FullName { get; set; }
        public FileModel(string filename, string fullname)
        {
            FileName = filename;
            FullName = fullname;
        }
    }
}
