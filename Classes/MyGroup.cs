using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeacherPlanner.Classes.Interfaces;
using System.IO;

namespace TeacherPlanner.Classes
{
    public class MyGroup : ConvertToPDF, INotifyPropertyChanged
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
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Themes)));   
            }
        }
        
        public int NumberOfStudents { get; set; }

        private List<string> studentNameList = []; // May be not full list, or oversized
        
        public int GeneralNumberOfLessons { get; set; }

        private List<string> lessonNameList = []; // May be not full list, or oversized

        public event PropertyChangedEventHandler? PropertyChanged;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public ObservableCollection<IEnumerable<DateTime>> listOfHollidays { get; set; } = [];

        public bool[] daysOfStudy { get; } = new bool[6]; // Mon. Tue. Wed. Thu. Fri. Sat.

        private List<string> GetStringList(string fileName)
        {
            var list = new List<string>();
            foreach (var line in File.ReadLines(fileName))
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    list.Add(line);
                }
            }
            return list;
        }
        public void UploadListOfStudentNames_txt(string fileName)
        {
            studentNameList = GetStringList(fileName);
            NumberOfStudents = studentNameList.Count;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NumberOfStudents)));
        }

        public void UploadListOfLesonNames_txt(string fileName)
        {
            lessonNameList = GetStringList(fileName);
            GeneralNumberOfLessons = lessonNameList.Count;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(GeneralNumberOfLessons)));
        }

        public void SaveDatesToLessons()
        { 
            var dates = new List<DateTime>();
            var hollidays = new List<DateTime>();
            foreach(var dateRange in listOfHollidays) // forming list of excluded dates
            {
                hollidays.AddRange(dateRange);
            }

            var currentDate = StartDate;
            while (currentDate <= EndDate)
            {
                if (currentDate.DayOfWeek != DayOfWeek.Sunday && !hollidays.Contains(currentDate))
                {
                    switch (currentDate.DayOfWeek)
                    {
                        case DayOfWeek.Monday:
                            if (daysOfStudy[0]) dates.Add(currentDate);
                            break;
                        case DayOfWeek.Tuesday:
                            if (daysOfStudy[1]) dates.Add(currentDate);
                            break;
                        case DayOfWeek.Wednesday:
                            if (daysOfStudy[2]) dates.Add(currentDate);
                            break;
                        case DayOfWeek.Thursday:
                            if (daysOfStudy[3]) dates.Add(currentDate);
                            break;
                        case DayOfWeek.Friday:
                            if (daysOfStudy[4]) dates.Add(currentDate);
                            break;
                        case DayOfWeek.Saturday:
                            if (daysOfStudy[5]) dates.Add(currentDate);
                            break;
                        default:
                            throw new Exception("unknown day of week");
                    }
                }
                currentDate = currentDate.AddDays(1);
            }
            // apply dates for each lesson
            // throw if not enough dates
            // throw if not enough lessons
            throw new NotImplementedException();
        }

        public void MakeCalculations()
        {
            int numberOfLessonsInThemes = 0;
            foreach(var theme in Themes)
            {
                numberOfLessonsInThemes += theme.NumberOfLessons;
            }
            if (numberOfLessonsInThemes != GeneralNumberOfLessons)
            {
                throw new InvalidDataException($"Загальна кількість уроків {GeneralNumberOfLessons}, тоді як кількість уроків у всіх темах {numberOfLessonsInThemes}");
            }
            SaveDatesToLessons();
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
