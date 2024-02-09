using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeacherPlanner.Classes.Interfaces
{
    public interface ConvertToPDF
    {
        public void ConvertToPDF(string? filename = null);
    }
}
