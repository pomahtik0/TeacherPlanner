using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeacherPlanner.Classes.Interfaces;

namespace TeacherPlanner.Classes
{
    internal class MyGroup : ConvertToPDF
    {
        public string GroupName { get; set; } = "";
        public ObservableCollection<Theme> Themes { get; set; } = [];
        public int NumberOfThemes
        {
            get => Themes.Count;
            set
            {
                if (value < NumberOfThemes)
                {
                    Themes = new ObservableCollection<Theme>(Themes.Take(value));
                }
                else
                {
                    while (value++ > NumberOfThemes)
                    {
                        Themes.Add(new Theme());
                    }
                }
            }
        }

        public void ConvertToPDF(string filename)
        {
            throw new NotImplementedException();
        }
    }
}
