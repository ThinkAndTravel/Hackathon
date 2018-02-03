using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoManager.CollectionModels
{
    public class User: BsonModel
    {
        #region VAR

        public MainUser Main { get; set; }

        /// <summary>
        /// Дата реєстрації юзера
        /// </summary>
        public DateTime? DateCreated = null;

        /// <summary>
        /// Остання сесія
        /// </summary>
        [BsonIgnoreIfNull]
        public DateTime? LastLoginTime = null;

        /// <summary>
        /// Для перевірки підтвердження реєстрації
        /// </summary>
        public bool Activated ;

        /// <summary>
        /// Всі підписки на зовнішні сервіси
        /// </summary>
        [BsonIgnoreIfNull]
        public Confirm Confirm { get; set; }

        [BsonIgnoreIfNull]
        public double Money { get; set; }

        public List<Notice> Notices { get; set; } = new List<Notice>();

        /// <summary>
        /// Підписки юзера
        /// </summary>
        [BsonIgnoreIfNull]
        public List<string> Subscriptions { get; set; } = new List<string>();

        /// <summary>
        /// Список друзів
        /// </summary>
        [BsonIgnoreIfNull]
        public List<string> Friends { get; set; } = new List<string>();

        /// <summary>
        /// Завдання доступні до виконання
        /// </summary>
        [BsonIgnoreIfNull]
        public List<string> ActiveTask { get; set; } = new List<string>();

        /// <summary>
        /// Виконані(або просрочені) завдання
        /// </summary>
        [BsonIgnoreIfNull]
        public List<string> CompletedTasks { get; set; } = new List<string>();

        /// <summary>
        /// Список id постів
        /// </summary>
        [BsonIgnoreIfNull]
        public List<string> Posts { get; set; } = new List<string>();

        /// <summary>
        /// Список id на яких зображений користувач
        /// </summary>
        [BsonIgnoreIfNull]
        public List<string> Photos { get; set; } = new List<string>();
        public List<Plan> Plans { get; set; } = new List<Plan>();
    
        #endregion

        
    }
}
