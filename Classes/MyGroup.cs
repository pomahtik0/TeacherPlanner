﻿using System;
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

        public DateTime startDate { get; set; }

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
