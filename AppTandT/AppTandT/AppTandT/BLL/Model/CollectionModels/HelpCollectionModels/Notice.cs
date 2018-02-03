using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTandT.BLL.Model.CollectionModels.HelpCollectionModels
{
    /// <summary>
    /// Повідомляє про плани друзів
    /// </summary>
    public class Notice
    {
        #region VAR

        /// <summary>
        /// id юзера(конкретно друга), який додав свій план
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// Сам план
        /// </summary>       
        public Plan Plan { get; set; } = new Plan();

        /// <summary>
        /// Дата повідомлення
        /// </summary>    
        public DateTime? DateMessage = null;// { get; set; }

        #endregion      
    }
}