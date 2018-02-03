using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoManager.CollectionModels
{

   /// <summary>
   /// Визначає географічні координати
   /// </summary>
   public class Location
    {
        #region VAR
        [BsonIgnoreIfNull]
        public string Country { get; set; }

        /// <summary>
        /// Населений пункт
        /// </summary>
        [BsonIgnoreIfNull]
        public string City { get; set; }

        /// <summary>
        /// Довгота
        /// </summary>
        [BsonIgnoreIfNull]
        public double GeoLong; //{ get; set; } 

        /// <summary>
        /// Широта
        /// </summary>
        [BsonIgnoreIfNull]
        public double GeoLat; //{ get; set; }

        #endregion

        
    }
}
