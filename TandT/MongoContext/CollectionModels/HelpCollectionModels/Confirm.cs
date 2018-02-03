using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoManager.CollectionModels
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
        [BsonIgnoreIfNull]
        public string ConfirmString { get; set; }

        /// <summary>
        /// Зберігає id чату в якому залогінився юзер
        /// </summary>
        [BsonIgnoreIfNull]
        public string ChatId { get; set; }

        #endregion

       
    }
}
