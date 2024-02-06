using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeacherPlanner.Classes
{
    public class Theme
    {
        public ObservableCollection<Lesson> Lessons { get; set; } = [];
    }
}
