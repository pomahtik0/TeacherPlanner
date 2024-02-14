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
using QuestPDF;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Windows.Documents;

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
            foreach (var dateRange in listOfHollidays) // forming list of excluded dates
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

            var dateEnum = dates.GetEnumerator();

            foreach (var theme in Themes)
            {
                foreach (var lesson in theme.Lessons)
                {
                    if (!dateEnum.MoveNext())
                        throw new IndexOutOfRangeException($"Недостатньо дат ({dates.Count}), щоб покрити всі уроки ({GeneralNumberOfLessons}).");
                    lesson.Date = dateEnum.Current;
                }
            }
            if (dateEnum.MoveNext())
                throw new IndexOutOfRangeException($"Забагато дат, кількість дат {dates.Count}, а кількість уроків {GeneralNumberOfLessons}");
            dateEnum.Dispose();
        }

        public void MakeCalculations()
        {
            int numberOfLessonsInThemes = 0;
            foreach (var theme in Themes)
            {
                numberOfLessonsInThemes += theme.NumberOfLessons;
            }
            if (numberOfLessonsInThemes != GeneralNumberOfLessons)
            {
                throw new InvalidDataException($"Загальна кількість уроків {GeneralNumberOfLessons}, тоді як кількість уроків у всіх темах {numberOfLessonsInThemes}");
            }

            SaveDatesToLessons();

            if (lessonNameList.Count > 0)
            {
                var nameEnum = lessonNameList.GetEnumerator();
                foreach (var theme in Themes)
                {
                    foreach (var lesson in theme.Lessons)
                    {
                        if (!nameEnum.MoveNext()) break;
                        lesson.Name = nameEnum.Current;
                    }
                }
                if (lessonNameList.Count != GeneralNumberOfLessons)
                {
                    throw new Exception($"Попередження! Кількість завантажених уроків ({lessonNameList.Count}), і вказана кількість уроків ({GeneralNumberOfLessons}) не співпадають!");
                }
                nameEnum.Dispose();
            }
        }

        public void ConvertToPDF(string? filename = null)
        {
            if (string.IsNullOrWhiteSpace(GroupName)) throw new Exception("Введіть ім'я групи");
            if (NumberOfThemes <= 0) throw new Exception("Введіть кількість тем");
            if (GeneralNumberOfLessons <= 0) throw new Exception("Кількість уроків не може дорівнювати 0");
            if (NumberOfStudents <= 0) throw new Exception("Кількість учнів не може дорівнювати 0");
            foreach (var theme in Themes)
            {
                if (theme.NumberOfLessons <= 0) throw new Exception("Кількість уроків в темі не може бути 0");
            }

            try
            {
                MakeCalculations(); // recalculating dates and names
            }
            catch (Exception e)
            {
                if (e is IndexOutOfRangeException || e is InvalidDataException) { throw; } // rethrowing if exception is critical
            }

            filename ??= $"{GroupName}.pdf";

            Document.Create(container =>
            {
                int lessonIndex = 1;
                foreach (var theme in Themes)
                {
                    container.Page(page =>
                    {
                        page.DefaultTextStyle(x => x.FontFamily("Times New Roman"));
                        page.Size(PageSizes.A4);
                        page.MarginLeft(20);
                        page.MarginRight(15);
                        page.MarginTop(20);
                        page.MarginBottom(20);
                        page.Content().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(20);
                                columns.ConstantColumn(100);
                                for (int i = 0; i <= theme.NumberOfLessons; i++)
                                {
                                    columns.RelativeColumn();
                                }
                                if(theme.IsThemeControll)
                                    columns.RelativeColumn();
                            });

                            table.Header(header =>
                            {
                                header.Cell().Element(Block).Text("№");
                                header.Cell().ColumnSpan(2).Element(Block).Text("Ім'я");
                                foreach (var lesson in theme.Lessons)
                                {
                                    header.Cell().Element(Block).Text(lesson.Date.ToString("dd/MM"));
                                }
                                if (theme.IsThemeControll)
                                    header.Cell().Element(Block).Text("T");
                            });

                            for (int i = 1; i <= NumberOfStudents; i++)
                            {
                                table.Cell().Element(Block).Text((i).ToString());
                                if (i <= studentNameList.Count)
                                    table.Cell().ColumnSpan(2).Element(TextBlock).Text(studentNameList[i - 1]);
                                else table.Cell().ColumnSpan(2).Element(Block);
                                foreach (var lesson in theme.Lessons)
                                {
                                    table.Cell().Element(Block);
                                }
                                if (theme.IsThemeControll)
                                    table.Cell().Element(Block);
                            }

                            uint collspan = (uint)theme.NumberOfLessons + 1;
                            if (theme.IsThemeControll) collspan++;
                            for (int i = 0; i < 4; i++)
                            {
                                table.Cell().ColumnSpan(collspan + 2).Text("");
                            }


                            table.Cell().Element(Block).Text("№");
                            table.Cell().Element(Block).Text("Дата");
                            table.Cell().ColumnSpan(collspan).Element(TextBlock).PaddingLeft(15).Text(theme.Name).Bold();

                            foreach(var lesson in theme.Lessons)
                            {
                                table.Cell().Element(Block).Text(lessonIndex++.ToString());
                                table.Cell().Element(Block).Text(lesson.Date.ToString("dd/MM"));
                                table.Cell().ColumnSpan(collspan).Element(TextBlock).Text(lesson.Name);
                            }

                            static QuestPDF.Infrastructure.IContainer Block(QuestPDF.Infrastructure.IContainer container)
                            {
                                return container
                                    .Border(1)
                                    .ShowOnce()
                                    .AlignCenter()
                                    .AlignMiddle();
                            }

                            static QuestPDF.Infrastructure.IContainer TextBlock(QuestPDF.Infrastructure.IContainer container)
                            {
                                return container
                                    .Border(1)
                                    .ShowOnce()
                                    .AlignLeft()
                                    .PaddingLeft(5)
                                    .AlignMiddle();
                            }
                        });
                    });
                }
            }).GeneratePdf(filename);
        }
    }
}
