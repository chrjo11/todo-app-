using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace TodoApp.MVVM.Converter
{
    class DateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // 1. Expliziter Cast vs. "as" Operator
            // 2. "is" Operator zum prüfen des Typs
            if (value is DateTime) //überprüft, ob der übergebene Par. value vom Typ DateTime ist: wenn true -> if-Anweisung wird ausgeführt
            {
                DateTime date = (DateTime)value; //Expliziter Cast: date bekommt value zugewiesen
                
                 if (Equals(parameter, "1")) 
                 {
                     return date.ToString("dd.MM.yyyy");
                 }

                 if (Equals(parameter, "2")) 
                 {
                     return date.ToString("H:mm:ss");
                 }
            }
            return "-";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
             return null;
        }
        
    }
}