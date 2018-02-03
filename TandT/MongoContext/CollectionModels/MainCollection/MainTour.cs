using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoManager.CollectionModels
{
    public class MainTour:BsonModel
    {
        [BsonIgnoreIfNull]
        public string Country { get; set; }
        [BsonIgnoreIfNull]
        public string Name { get; set; }
        [BsonIgnoreIfNull]
        public string City { get; set; }
        [BsonIgnoreIfNull]
        public int? Price = null;
        [BsonIgnoreIfNull]
        public string About { get; set; }
        [BsonIgnoreIfNull]
        public List<string> Tags { get; set; } = new List<string>();
        
    }
}
