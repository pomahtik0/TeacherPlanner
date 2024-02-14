using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeacherPlanner.Classes
{
    internal class PdfSettings
    {
        private string fullPath = string.Empty;

        public string FullPath
        {
            get { return fullPath; }
            set { fullPath = value; }
        }

        public string DocumentName
        {
            get { return Path.GetFileName(fullPath); }
        }

        public int TextSize { get; set; }
    }
}
