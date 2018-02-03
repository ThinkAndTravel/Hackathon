using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTandT.BLL
{
    /// <summary>
    /// Сесія певного користувача складається з емайла, ід, ключа(шифровка для JVT)
    ///  Вона зберігаться в налаштуваннях
    /// </summary>


    public class Sesion
    {
        public static string Email
        {
            get => AppSettings.GetValueOrDefault(nameof(Email), string.Empty);

            set => AppSettings.AddOrUpdateValue(nameof(Email), value);

        }

        public static string _id
        {
            get => AppSettings.GetValueOrDefault(nameof(_id), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(_id), value);

        }

        public static string key
        {
            get => AppSettings.GetValueOrDefault(nameof(key), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(key), value);
        }

        public static void CloseSession()
        {
            NewSession("", "", "");
        }

        public static void NewSession(string _id, string Email, string key)
        {
            CrossSettings.Current.AddOrUpdateValue(nameof(_id), _id);
            CrossSettings.Current.AddOrUpdateValue(nameof(Email), Email);
            CrossSettings.Current.AddOrUpdateValue(nameof(key), key);
        }

        private static ISettings AppSettings
        {
            get
            {
                if (CrossSettings.IsSupported)
                    return CrossSettings.Current;

                return null; // or your custom implementation 
            }
        }

    }
}

