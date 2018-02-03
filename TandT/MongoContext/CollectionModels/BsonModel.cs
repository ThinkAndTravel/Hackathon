using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoManager.CollectionModels
{
    public class BsonModel
    {
        [BsonId]
        public string _id { get; set; }
    }
}
