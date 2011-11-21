using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Wpf_Medical
{
    /// <summary>
    /// Cette classe nous permet d'afficher de maniere comprehensible le statut de connexion d'un membre du personnel
    /// </summary>
    class BoolConverter : IValueConverter
    {
        /// <summary>
        /// Quand le webservice nous fournit la liste nous convertissons le true/false en string
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null) {
                bool boolValue = System.Convert.ToBoolean(value);

                if (boolValue)
                {
                    return "Oui";
                }
                else {
                    return "Non";
                }
            }

            return "Non";
        }

        /// <summary>
        /// Nous n'avons pas besoin de convertir dans le sens string -> bool
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
