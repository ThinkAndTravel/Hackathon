using System;
using MongoDB.Bson;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoManager.CollectionModels
{
    /// <summary>
    /// Зберігає питання і відповіді(до цього питання) для Task
    /// </summary>
    public class Question
    {
        #region VAR

        /// <summary>
        /// Саме питання
        /// </summary>
        [BsonIgnoreIfNull]
        public string Value { get; set; }

        /// <summary>
        /// Неправильні варіанти відповіді
        /// </summary>
        [BsonIgnoreIfNull]
        public List<string> Answers { get; set; }

        /// <summary>
        /// Правильна відповідь
        /// </summary>
        [BsonIgnoreIfNull]
        public string TrueAnswer { get; set; }
        
        #endregion

       
    }
}