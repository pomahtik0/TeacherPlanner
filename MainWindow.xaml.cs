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
using TeacherPlanner.Classes;

namespace TeacherPlanner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MyGroup? group;
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
            var control = sender as Control;
            var parent = control?.Parent as Expander;
            if (parent != null)
            {
                parent.GetBindingExpression(Expander.HeaderProperty).UpdateTarget();
                parent.IsExpanded = false;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            group = this.TryFindResource("group") as MyGroup;
        }

        private void Submit_Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}