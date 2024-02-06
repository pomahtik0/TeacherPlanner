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
        public string Name { get; set; } = "";
        public List<Lesson> Lessons { get; set; } = [];
        public int NumberOfLessons
        {
            get => Lessons.Count;
            set
            {
                if (value < NumberOfLessons)
                {
                    Lessons = new ObservableCollection<Lesson>(Lessons.Take(value));
                }
                else
                {
                    while (value++ > NumberOfLessons)
                    {
                        Lessons.Add(new Lesson());
                    }
                }
            }
        }
    }
}
