using System.Globalization;

namespace OptiMail.Converters
{
    // GroupedEmails.Count == 0 iken true döner; boş durum ekranını göstermek için kullanılır
    public class CountToInverseBoolConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is int count)
                return count == 0;

            return true;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
