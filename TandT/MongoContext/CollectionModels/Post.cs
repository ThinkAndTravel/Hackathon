using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoManager.CollectionModels.HelpCollectionModels;

namespace MongoManager.CollectionModels
{
    public class Post:BsonModel
    {
        #region VAR
        
        [BsonIgnoreIfNull]
        public MainPost Main { get; set; }
        /// <summary>
        /// Список людей які є відмічені на фото
        /// </summary>
        [BsonIgnoreIfNull]
        public List<string> UserPost { get; set; } = new List<string>();

        /// <summary>
        /// Опис посту
        /// </summary>
        [BsonIgnoreIfNull]
        public string About { get; set; }


        /// <summary>
        /// UserID
        /// </summary>
        [BsonIgnoreIfNull]
        public List<string> Likes { get; set; } = new List<string>();
        [BsonIgnoreIfNull]
        public List<Comment> PostComments { get; set; } = new List<Comment>();
        #endregion

       
    }
}
