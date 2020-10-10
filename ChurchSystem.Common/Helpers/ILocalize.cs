using System.Globalization;

namespace ChurchSystem.Common.Helpers
{
    public interface ILocalize
    {
        CultureInfo GetCurrentCultureInfo();

        void SetLocale(CultureInfo ci);

    }
}
