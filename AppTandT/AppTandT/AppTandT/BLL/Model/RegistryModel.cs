using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTandT.BLL.Model
{
    public class RegistryModel
    {
        public string Login { get; set; }
        public string Pas { get; set; }
        public string Email { get; set; }
        public string ConfirmPas { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }


        /// <summary>
        /// Перевіряє модель на валідність
        /// </summary>
        public bool isValid()
        {
            if (Login == null || Email == null || Pas == null || ConfirmPas == null || FirstName == null || LastName == null)
                throw new Help.ServiceException("All fields must be filled.", okText: "Ok");

            if (Login.Length == 0)
                throw new Help.ServiceException("Login can't be empty!", okText: "Ok");
            if (Email.Length == 0)
                throw new Help.ServiceException("Email can't be empty!", okText: "Ok");
            if (Pas != ConfirmPas)
                throw new Help.ServiceException("Passwords are different!", okText: "Ok");
            if (Pas.Length == 0)
                throw new Help.ServiceException("Password can't be empty!", okText: "Ok");
            if (FirstName.Length == 0)
                throw new Help.ServiceException("First name can't be empty!", okText: "Ok");
            if (LastName.Length == 0)
                throw new Help.ServiceException("Last name can't be empty!", okText: "Ok");


            Login = Login.Trim();
            //Pas = Pas.Trim();
            //ConfirmPas = ConfirmPas.Trim();
            Email = Email.Trim();
            FirstName = FirstName.Trim();
            LastName = LastName.Trim();

            if (Login.Length == 0)
                throw new Help.ServiceException("Login can't be empty!", okText: "Ok");
            if (Email.Length == 0)
                throw new Help.ServiceException("Email can't be empty!", okText: "Ok");
            if (Pas != ConfirmPas)
                throw new Help.ServiceException("Passwords are different!", okText: "Ok");
            if (Pas.Length == 0)
                throw new Help.ServiceException("Password can't be empty!", okText: "Ok");
            if (FirstName.Length == 0)
                throw new Help.ServiceException("First name can't be empty!", okText: "Ok");
            if (LastName.Length == 0)
                throw new Help.ServiceException("Last name can't be empty!", okText: "Ok");

            bool valid_login = Login.Length > 0;

            bool valid_pass = Pas.Trim().Length > 0 && Pas.Trim().Length == ConfirmPas.Trim().Length;

            bool valid_mail = Email.Length > 0;
            string email_pattern = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*@((([\w]+([-\w]*[\w]+)*\.)+[a-zA-Z]+)"
                                 + @"|((([01]?[0-9]{1,2}|2[0-4][0-9]|25[0-5]).){3}[01]?[0-9]{1,2}|2[0-4][0-9]|25[0-5]))\z";
            valid_mail &= System.Text.RegularExpressions.Regex.IsMatch(Email, email_pattern);

            bool valid_fname = FirstName.Length > 0;
            bool valid_lname = LastName.Length > 0;///

            if (!valid_login)
                throw new Help.ServiceException("Login is invalid!", okText: "Ok");
            if (!valid_mail)
                throw new Help.ServiceException("Email is invalid!", okText: "Ok");
            if (!valid_pass)
                throw new Help.ServiceException("Passwords are invalid!", okText: "Ok");
            if (!valid_fname)
                throw new Help.ServiceException("First name is invalid!", okText: "Ok");
            if (!valid_lname)
                throw new Help.ServiceException("Last name is invalid!", okText: "Ok");

            return true;// valid_login && valid_pass && valid_mail && valid_fname && valid_lname;
        }
    }
}
