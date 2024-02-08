using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Microsoft.Office.Interop.Excel;
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

        private List<string> lesonNameList = []; // May be not full list, or oversized

        public event PropertyChangedEventHandler? PropertyChanged;

        public DateTime startDate { get; set; }

        public DateTime EndDate { get; set; }

        public ObservableCollection<IEnumerable<DateTime>> listOfHollidays { get; set; } = [];

        public bool[] daysOfStudy { get; } = new bool[6]; // Mon. Tue. Wed. Thu. Fri. Sat.

        private List<string> GetStringList(string fileName)
        {
            Application xlApp = new Application();
            Workbook xlWorkBook = null;
            Worksheet dataSheet = null;
            Microsoft.Office.Interop.Excel.Range dataRange = null;
            List<string> namesList = new List<string>();

            try
            {
                // Open the Excel file
                xlWorkBook = xlApp.Workbooks.Open(fileName);

                // Get the first data sheet (you can modify this based on your needs)
                dataSheet = (Worksheet?)xlWorkBook.Worksheets[1];

                // Get the used range (all data in the sheet)
                dataRange = dataSheet.UsedRange;

                // Read data into an object array
                object[,] valueArray = (object[,])dataRange.Value;

                // Iterate through the rows (assuming names are in a column)
                for (int row = 1; row <= valueArray.GetLength(0); row++)
                {
                    string name = valueArray[row, 1]?.ToString(); // Assuming names are in the first column
                    if (!string.IsNullOrEmpty(name))
                    {
                        namesList.Add(name);
                    }
                }

                // Now 'namesList' contains the names as strings.
            }
            catch
            {
                throw;
            }
            finally
            {
                // Clean up resources
                xlWorkBook?.Close(false);
                xlApp?.Quit();
            }
        }
        public void UploadListOfStudentNames_excel(string fileName)
        {
            // checks for file format
            // loading names to a list
            // setting NumberOfStudents to list count
            // source was updated event
            throw new NotImplementedException();
        }

        public void UploadListOfLesonNames_excel(string fileName)
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
