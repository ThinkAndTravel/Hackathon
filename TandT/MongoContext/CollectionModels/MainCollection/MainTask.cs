using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoManager.CollectionModels
{
   public class MainTask
    {
        #region VAR
        [BsonIgnoreIfNull]
        public int? TypeTask = null;
        [BsonIgnoreIfNull]
        public Location Location { get; set; }
        /// <summary>
        /// Список запитань для тестів
        /// </summary>
        [BsonIgnoreIfNull]
        public List<Question> Questions { get; set; } = new List<Question>();
        [BsonIgnoreIfNull]
        public List<string> Tags { get; set; }
        [BsonIgnoreIfNull]
        public string About { get; set; }
        [BsonIgnoreIfNull]
        public string Logo { get; set; }
        [BsonIgnoreIfNull]
        public List<string> PicturesAbout { get; set; } = new List<string>();
        [BsonIgnoreIfNull]
        public string Title { get; set; }
        #endregion

    }
}
