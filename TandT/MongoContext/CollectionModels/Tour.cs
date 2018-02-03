using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoManager.CollectionModels.HelpCollectionModels;

namespace MongoManager.CollectionModels
{
    public class Tour : BsonModel
    {
        #region VAR

        [BsonIgnoreIfNull]
        public MainTour Main { get; set; }
        [BsonIgnoreIfNull]
        public List<Comment> Comments { get; set; } = new List<Comment>();
        [BsonIgnoreIfNull]
        public List<string> Tourists { get; set; } = new List<string>();
        [BsonIgnoreIfNull]
        public List<Condition> Conditions { get; set; } = new List<Condition>();

        #endregion      
    }
}
