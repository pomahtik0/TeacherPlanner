using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace TeacherPlanner.Classes.Converters
{
    public class DatesToStringOneWay : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dates = value as ObservableCollection<DateTime>;
            if (dates == null || dates.Count == 0)
            {
                return "Оберіть дату";
            }
            else if (dates.Count == 1) 
            {
                return dates[0].ToString("dd/MM");
            }
            else
            {
                return $"{dates[0].ToString("dd/MM")}-{dates.Last().ToString("dd/MM")}";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
