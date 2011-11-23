using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.IO;

namespace Wpf_Medical
{
    class ByteArrayToImageListConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null) {
                List<BitmapImage> result = new List<BitmapImage>();
                byte[][] imagesArray = value as byte[][];

                for (int i = 0; i < imagesArray.Length; i++) {
                    MemoryStream memoryStream = new MemoryStream(imagesArray[i]);
                    BitmapImage decodedImage = new BitmapImage();
                    decodedImage.BeginInit();
                    decodedImage.StreamSource = memoryStream;
                    decodedImage.EndInit();
                    result.Add(decodedImage);
                }
                return result;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
