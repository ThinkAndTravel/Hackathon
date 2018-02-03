using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTandT.BLL.Model.CollectionModels.MainCollectionModels
{
    public class MainUser
    {
        #region VAR

        public string Email { get; set; }
        public string HashPass { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        /// <summary>
        /// ID аватару на Cloudinary 
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// Кількість зароблених балів
        /// </summary>       
        public string Country { get; set; }

        public string City { get; set; }

        #endregion

        public string Name { get { return FirstName + ' ' + LastName; } }

    }
}
