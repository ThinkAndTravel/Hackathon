using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoManager.CollectionModels
{
    public class Plan 
    {
        #region VAR
        [BsonIgnoreIfNull]
        public string PlanTask { get; set; }

        /// <summary>
        /// Дата ймовірного початку виконання завдання
        /// </summary>
        [BsonIgnoreIfNull]
        public DateTime? DateStart = null; //{ get; set; }

        /// <summary>
        /// Дата ймовірного закінчення виконання завдання 
        /// </summary>
        [BsonIgnoreIfNull]
        public DateTime? DateClose = null; // { get; set; }

        /// <summary>
        /// Якийсь опис юзера
        /// </summary>
        [BsonIgnoreIfNull]
        public string About { get; set; }


        #endregion

       
    }
}
