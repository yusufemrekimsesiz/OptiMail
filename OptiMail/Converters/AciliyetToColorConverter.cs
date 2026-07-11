using System.Globalization;

namespace OptiMail.Converters
{
    // "aciliyet" alanındaki metni (Düşük/Orta/Yüksek) rozet rengine çevirir
    public class AciliyetToColorConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return (value as string) switch
            {
                "Yüksek" => Color.FromArgb("#E5484D"),
                "Orta" => Color.FromArgb("#F5A623"),
                "Düşük" => Color.FromArgb("#3DA35D"),
                _ => Color.FromArgb("#8E8E93")
            };
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
