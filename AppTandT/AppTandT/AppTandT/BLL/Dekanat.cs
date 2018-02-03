using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTandT.BLL
{
    /// <summary>
    /// (deanery) == деканат
    /// Клас, який відповідає за сесії
    /// </summary>
    public class Dekanat
    {
        /// <summary>
        /// Створення нової сесії
        /// якщо параметри null, null, null то це видалення сесії
        /// </summary>
        /// <param name="_id"></param>
        /// <param name="Email"></param>
        /// <param name="key"></param>
        public static void newSesion(string _id, string Email, string key)
        {
            Sesion.NewSession(_id, Email, key);
        }



        public static bool isLogined()
        {
            /////!!! має перевірятись відповідність ключа
            if (Sesion._id.Length != 0) return true;
            return false;
        }

    }
}
