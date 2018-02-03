using System;
using System.Collections.Generic;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.GridFS;
using MongoManager.CollectionModels;

namespace MongoManager
{
    /// <summary>
    /// Реаалізує курування колекцією, яка знаходиться в багатьох БД
    /// </summary>
    /// <typeparam name="T"> Модель колекції </typeparam>
    public class DbSet<T>// where T:IBsonModel
    {

        /// <summary>
        /// Словник який зберігає об'єкт доступу до колокції кожної БД
        /// </summary>
       //  public static Dictionary<string, IMongoCollection<BsonDocument>> collections
       //     = new Dictionary<string, IMongoCollection<BsonDocument>>();


        /// <summary>
        /// Словник який зберігає об'єкти для LINQ запитів
        /// </summary>
        public static Dictionary<string, IMongoCollection<T>> collections
            = new Dictionary<string, IMongoCollection<T>>();

        /// <summary>
        /// Створює DbSet який зчитує з maindb, які бд потрібно підключити і підключає
        /// </summary>
        /// <remarks> 
        /// Використовує GetDictionary()
        /// </remarks>
        public DbSet(string colName)
        {
            var client = new MongoClient("mongodb://admin:121314qw@ds036617.mlab.com:36617/maindb");
            var db = client.GetDatabase("maindb");
            var col = db.GetCollection<BsonDocument>("DB");
            var filter = new BsonDocument("CollectionName",colName);
            var collection = col.Find(filter).ToList();
            List<string> ConnString = new List<string>();
            List<string> dbName = new List<string>();
            foreach (var o in collection)
            {
                ConnString.Add(o["ConnectingString"].AsString);
                dbName.Add(o["dbName"].AsString);

            }
            GetDictionary(ConnString.ToArray(),dbName.ToArray(), colName);
        }

        /// <summary>
        /// Підключає всі БД і заповнює словник колекцій
        /// </summary>
        /// <param name="connectionStrings"></param>
        /// <param name="dbNames"></param>
        /// <param name="collectionName"></param>
        public void GetDictionary(string[] connectionStrings, string[] dbNames, string collectionName)
        {
          if (connectionStrings.Length == dbNames.Length)
            for(int i = 0; i < dbNames.Length; i++)
            {
                var client = new MongoClient(connectionStrings[i]);
                var db = client.GetDatabase(dbNames[i]);
               // collections.Add(dbNames[i], db.GetCollection<BsonDocument>(collectionName));
                collections.Add(dbNames[i], db.GetCollection<T>(collectionName));
                
            }
        }

    }
}
