using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkingWithFiles.Shared.Model
{
    public class FileData
    {
        public byte[]? Data { get; set; }
        public string? FileName { get; set; }
        public string? ContentType { get; set; }
    }
}
