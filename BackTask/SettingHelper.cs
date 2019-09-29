using System.Runtime.CompilerServices;
using Windows.Storage;

namespace BackTask
{
    static class SettingHelper
    {
        static readonly ApplicationDataContainer container = ApplicationData.Current.LocalSettings;

        private static T GetOrSetDefault<T>(T def, [CallerMemberName] string key = null)
        {
            if (container.Values[key] != null)
            {
                return (T)container.Values[key];
            }
            else
            {
                SetValue(def, key);
                return def;
            }
        }

        private static void SetValue<T>(T value, [CallerMemberName] string key = null)
        {
            container.Values[key] = value;
        }

        public static bool DTCT
        {
            get => GetOrSetDefault(true);
            set => SetValue(value);
        }

        public static bool DT
        {
            get => GetOrSetDefault(true);
            set => SetValue(value);
        }

        public static bool FJ
        {
            get => GetOrSetDefault(true);
            set => SetValue(value);
        }

        public static string TsDt
        {
            get => GetOrSetDefault(string.Empty);
            set => SetValue(value);
        }
    }
}
