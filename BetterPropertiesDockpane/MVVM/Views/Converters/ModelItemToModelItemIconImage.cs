using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using Autodesk.Navisworks.Api;

namespace BetterPropertiesDockpane.MVVM.Views.Converters
{
    class ModelItemToModelItemIconImage : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            var uriString = @"/BetterPropertiesDockpane;component/Images/";

            var targetIconType = NavisworksDevHelper.ModelItem.CategoriesPropertiesHelper.GetIconType(value as ModelItem);

            switch (targetIconType)
            {
                case NavisworksDevHelper.ModelItem.IconType.Unidentified:
                    goto default;
                case NavisworksDevHelper.ModelItem.IconType.File:
                    uriString += "GUID-2D8532F2-122E-4218-9E22-44C4BC834F7C.png";
                    break;
                case NavisworksDevHelper.ModelItem.IconType.Layer:
                    uriString += "GUID-4BCD09CF-FF0C-4B88-B473-B1025A17C100.png";
                    break;
                case NavisworksDevHelper.ModelItem.IconType.Collection:
                    uriString += "GUID-7AD510FA-7C48-415E-9579-D996820D8BC1.png";
                    break;
                case NavisworksDevHelper.ModelItem.IconType.CompositeObject:
                    uriString += "GUID-197CB0CC-4CBB-4308-A42C-0B7046B05392.png";
                    break;
                case NavisworksDevHelper.ModelItem.IconType.InsertGroup:
                    uriString += "GUID-A12DD8E6-A4BE-401A-BB86-6C80E4C4C1FB.png";
                    break;
                case NavisworksDevHelper.ModelItem.IconType.Geometry:
                    uriString += "GUID-8C08B821-22E1-45BA-9421-D9C5E577D4B0.png";
                    break;
                default:
                    uriString = string.Empty;
                    break;
            }

            var ImageUri = new Uri(uriString, UriKind.Relative);
            return new BitmapImage(ImageUri);
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
