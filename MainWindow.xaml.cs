using System.ComponentModel;
using System.Diagnostics;
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

namespace TeacherPlanner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void AddNewHolliday_Button_Click(object sender, RoutedEventArgs e)
        {
            listOfHollidays.Items.Add(21);
        }

        private void hollidayDates_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            var control = sender as Control;
            var parent = control?.Parent as Expander;
            if (parent != null)
            {
                parent.GetBindingExpression(Expander.HeaderProperty).UpdateTarget();
                parent.IsExpanded = false;
            }
        }
    }
}