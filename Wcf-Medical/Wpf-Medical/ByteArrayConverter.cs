using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.IO;
using System.Windows.Media.Imaging;

namespace Wpf_Medical
{
    /// <summary>
    /// Cette classe nous permet de coder/decoder les images envoyees et recues
    /// </summary>
    class ByteArrayConverter : IValueConverter
    {
        /// <summary>
        /// Pour afficher les byte[] recuperees des webservices en Image
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null) {
                byte[] byteArrayValue = value as byte[];
                MemoryStream memoryStream = new MemoryStream(byteArrayValue);

                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = memoryStream;
                image.EndInit();

                return image;
            }
            return null;
        }

        /// <summary>
        /// Permet de convertir les images uploadees depuis le client en byte[]
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //Stream stream = imageSource.StreamSource;
            //stream.Position = 0;
            //Byte[] buffer = null;
            //if (stream != null && stream.Length > 0)
            //{
            //    using (BinaryReader br = new BinaryReader(stream))
            //    {
            //        buffer = br.ReadBytes((Int32)stream.Length);
            //    }
            //}

            //return buffer;

            throw new NotImplementedException();
        }
    }
}
