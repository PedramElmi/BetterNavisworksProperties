using Autodesk.Navisworks.Api;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace BetterPropertiesDockpane.MVVM.Views.Converters
{
    public class PropertyValueToCorrectedValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return NavisworksDevHelper.ModelItem.CategoriesPropertiesHelper.GetCleanedVariantData(value as VariantData, true);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
