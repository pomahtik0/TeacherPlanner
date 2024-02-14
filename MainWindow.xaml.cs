using Microsoft.Win32;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TeacherPlanner.Classes;

namespace TeacherPlanner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MyGroup? group;
        public PdfSettings? pdfSettings;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void AddNewHolliday_Button_Click(object sender, RoutedEventArgs e)
        {
            group?.listOfHollidays.Add(new List<DateTime>());
        }

        private void hollidayDates_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            var calendar = sender as Calendar;
            var parent = calendar?.Parent as Expander;
            
            if (group != null && calendar != null)
            {
                group.listOfHollidays[listOfHollidays.SelectedIndex] = calendar.SelectedDates; // updating range of dates
            }

            if (parent != null)
            {
                parent.GetBindingExpression(Expander.HeaderProperty).UpdateTarget(); // updating header and closing calendar
                parent.IsExpanded = false;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            group = this.TryFindResource("group") as MyGroup;
            pdfSettings = this.TryFindResource("pdfSettings") as PdfSettings;
        }

        private void Submit_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                group?.MakeCalculations();
            }
            catch (IndexOutOfRangeException ex)
            {
                MessageBox.Show(ex.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch (InvalidDataException ex)
            {
                MessageBox.Show(ex.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Попередження", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            MessageBox.Show("Всі числа співпадають!", "Успіх", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        private void window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Return || e.Key == Key.Enter)
            {
                Keyboard.Focus(mainGrid);
            }
        }

        private string? GetExcelDocument()
        {
            var fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Текстовий Файл|*.txt";
            var result = fileDialog.ShowDialog();
            if(result == true)
            {
                return fileDialog.FileName;
            }
            else
            {
                return null;
            }
        }

        private void UploadListOfStudets_Button_Click(object sender, RoutedEventArgs e)
        {
            var filename = GetExcelDocument();
            if (filename == null)
                return;
            group?.UploadListOfStudentNames_txt(filename);
        }

        private void UploadListOfLessons_Button_Click(object sender, RoutedEventArgs e)
        {
            var filename = GetExcelDocument();
            if (filename == null)
                return;
            group?.UploadListOfLesonNames_txt(filename);

        }

        private void CreatePdf_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                group?.ConvertToPDF(pdfSettings);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            MessageBox.Show("Файл створено успішно", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            (sender as TextBox)?.SelectAll();
        }

        private void SelectPdf_Button_Click(object sender, RoutedEventArgs e)
        {
            var fileDialog = new SaveFileDialog();
            fileDialog.AddExtension = true;
            fileDialog.CheckFileExists = false;
            fileDialog.CheckPathExists = false;
            fileDialog.Filter = ".pdf|*.pdf";
            var result = fileDialog.ShowDialog();
            if(result == true)
            {
                fullPathPdf_TextBox.Text = fileDialog.FileName;
                fullPathPdf_TextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            }
        }
    }
}