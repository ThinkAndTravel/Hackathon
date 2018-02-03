using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoManager.CollectionModels.HelpCollectionModels;

namespace MongoManager.CollectionModels
{
    public class Photo:BsonModel
    {
        [BsonIgnoreIfNull]
        public string URL { get; set; }

        [BsonIgnoreIfNull]
        public List<string> Likes { get; set; } = new List<string>();

        [BsonIgnoreIfNull]
        public List<Comment> Comments { get; set; } = new List<Comment>();     
    }
}
