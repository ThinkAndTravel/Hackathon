using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTandT.BLL.Models.CollectionModels.HelpCollectionModels
{
    /// <summary>
    /// Представляє всі зовнішні підписки на інші сервіси
    /// </summary>
    public class Confirm
    {
        #region VAR

        /// <summary>
        /// Код підтвердження
        /// </summary>
        public string ConfirmString { get; set; }

        /// <summary>
        /// Зберігає id чату в якому залогінився юзер
        /// </summary>
        public string ChatId { get; set; }

        #endregion
    }
}