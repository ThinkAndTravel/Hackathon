using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamvvm;

namespace AppTandT.BLL.Models
{
    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }

        /// <summary>
        /// Перевіряє модель на валідність
        /// </summary>
        /// <returns></returns>
        public bool isValid()
        {
            return true;
        }
    }
}
