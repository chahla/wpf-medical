using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Wpf_Medical
{
    /// <summary>
    ///  Pour afficher convenablement les prescriptions des observations 
    ///  on va transformer le tableau en quelque chose de lisible
    /// </summary>
    class StringArrayConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null) {
                string[] stringArray = value as string[];

                StringBuilder sbuilder = new StringBuilder();
                foreach (String item in stringArray)
                {
                    sbuilder.Append("-" + item + "\n");
                }
                return sbuilder.ToString();
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
