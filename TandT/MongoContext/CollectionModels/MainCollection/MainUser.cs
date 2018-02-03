using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoManager.CollectionModels
{
    public class MainUser
    {
        #region VAR

        public string Email { get; set; }
        public string HashPass { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        /// <summary>
        /// ID аватару на Cloudinary 
        /// </summary>
        [BsonIgnoreIfNull]
        public string Avatar { get; set; }

        /// <summary>
        /// Кількість зароблених балів
        /// </summary>       
        [BsonIgnoreIfNull]
        public string Country { get; set; }
        [BsonIgnoreIfNull]
        public string City { get; set; }

        #endregion

        public string Name { get { return FirstName + ' ' + LastName; } }
        
    }
}
