using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeacherPlanner.Classes.Interfaces;

namespace TeacherPlanner.Classes
{
    public class MyGroup : ConvertToPDF
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
                    while (value > NumberOfThemes)
                    {
                        Themes.Add(new Theme());
                    }
                }
            }
        }
        
        public int NumberOfStudents { get; set; }

        private List<string> studentNameList = []; // May be not full list, or oversized
        
        public int GeneralNumberOfLessons { get; set; }

        private List<string> lesonNameList = []; // May be not full list, or oversized

        public DateTime startDate { get; set; }

        public DateTime EndDate { get; set; }

        public ObservableCollection<IEnumerable<DateTime>> listOfHollidays { get; set; } = [];

        public bool[] daysOfStudy { get; } = new bool[6]; // Mon. Tue. Wed. Thu. Fri. Sat.

        public void UploadListOfStudentNames_exel(string fileName)
        {
            // checks for file format
            // loading names to a list
            // setting NumberOfStudents to list count
            // source was updated event
            throw new NotImplementedException();
        }

        public void UploadListOfLesonNames_exel(string fileName)
        {
            // checks for file format
            // loading names to a list
            // setting GeneralNumberOfLessons to list count
            // source was updated event
            throw new NotImplementedException();
        }

        public void SaveDatesToLessons()
        { 
            // calculate dates
            // apply dates for each lesson
            // throw if not enough dates
            // throw if not enough lessons
            throw new NotImplementedException();
        }

        public void MakeCalculations()
        {
            // assighn all lesson names to Lessons in themes
            // apply dates to lessons
            // throw if calculations fail
            throw new NotImplementedException();
        }

        public void ConvertToPDF(string filename)
        {
            // final checks, throw if failed
            // for each theme generate 1-2 pages, with tables
            // insert text boxes to be able to redact lesson names
            throw new NotImplementedException();
        }
    }
}
