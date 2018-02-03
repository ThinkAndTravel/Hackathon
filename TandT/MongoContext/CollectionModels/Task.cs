using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoManager.CollectionModels.HelpCollectionModels;

namespace MongoManager.CollectionModels
{
    public class Task:BsonModel
    {
        #region VAR
        
        [BsonIgnoreIfNull]
        public MainTask Main { get; set; }
        [BsonIgnoreIfNull]
        public string  Country { get; set; }
        [BsonIgnoreIfNull]
        public string  City { get; set; }

        [BsonIgnoreIfNull]
        public List<Comment> Comments { get; set; } = new List<Comment>();
        [BsonIgnoreIfNull]
        public bool? IsPersonal = null;
        public List<string>[] Ranks = new List<string>[6];
        #endregion

      
    }
}
