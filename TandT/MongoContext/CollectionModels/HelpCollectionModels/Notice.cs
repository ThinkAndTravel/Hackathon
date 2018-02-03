using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoManager.CollectionModels
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
        [BsonIgnoreIfNull]
        public string User { get; set; }

        /// <summary>
        /// Сам план
        /// </summary>
        [BsonIgnoreIfNull]
        public Plan Plan { get; set; }

        /// <summary>
        /// Дата повідомлення
        /// </summary>
        [BsonIgnoreIfNull]
        public DateTime? DateMessage = null;// { get; set; }

        #endregion      
    }
}
