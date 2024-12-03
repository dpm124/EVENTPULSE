using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace EVENTPULSE
{
    public class EstadoToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string estado = value as string;

            if (estado == "Confirmado")
                return Brushes.Green;
            else if (estado == "Por Confirmar")
                return Brushes.Goldenrod;
            else if (estado == "Cancelado")
                return Brushes.Red;
            else
                return Brushes.Gray;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
