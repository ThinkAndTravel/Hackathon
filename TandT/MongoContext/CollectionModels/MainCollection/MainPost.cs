using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoManager.CollectionModels
{
   public class MainPost
    {
        #region VAR
        /// <summary>
        /// Id автора посту
        /// </summary>
        public string UserId { get; set; }

        [BsonIgnoreIfNull]
        public DateTime? DatePost = null;

        /// <summary>
        /// id photo
        /// </summary>
        [BsonIgnoreIfNull]
        public List<string> Photos { get; set; } = new List<string>();

        public Location Location { get; set; }

        public Task CurTask { get; set; }
        #endregion
       
    }
}
