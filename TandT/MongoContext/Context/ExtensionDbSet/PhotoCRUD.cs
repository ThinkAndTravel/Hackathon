using MongoManager.CollectionModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoManager.CollectionModels.HelpCollectionModels;

namespace MongoManager.Context
{
    public static class PhotoCRUD
    {

        #region CREATE

        /// <summary>
        /// Ім'я ДБ в яку ми записуємо всіх, хто щойно додався
        /// </summary>
        /// <remarks>
        /// Повинно бути різним для кожного сервера 
        /// </remarks>
        public static string CurrentDB { get; set; } = "f1";

        /// <summary>
        /// Додає новий документ в БД 
        /// </summary>
        public static bool Create(this DbSet<Photo> dbSet, Photo photo)
        {
            ///!!!
            /// Як синхронізувати роботу всіх серваків?!
            ///Нам потрібна поточна БД в яку можна додавати док => CurentDB
            /// (0)яким чином визначати що дана БД не занята іншим сервером?
            ///припустимо (0) ми якось вирішили,далі  алгоритм буде такий:
            ///           
            /// 1) Додаємо в БД новий докж
            /// 2) Перевіряємо чи він унікальний(валідний)
            ///   +(True) => return true;
            ///  +(False) видаляємо, який щойно додали => return false;  
            photo._id = CurrentDB + ":" + photo._id;
            DbSet<Photo>.collections[CurrentDB].InsertOne(photo);
            // DbSet<User>.collections[CurrentDB].
            return true;
        }

        #endregion

        #region READ
               /// <summary>
        /// Зчитує з словника колекцій лише головну частину документа за таким id 
        /// </summary>
        public static async Task<BsonDocument> ReadFullAsync(this DbSet<Photo> dbSet, string id)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<Photo>.collections.ContainsKey(db_name) == false)
            {
                return null;
            }
            var filter = new BsonDocument("_id", id);
            var photo = await DbSet<Photo>.collections[db_name].FindAsync(filter);
            return photo.ToBsonDocument();

        }

        public static Photo ReadFull(this DbSet<Photo> dbSet, string id)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<Photo>.collections.ContainsKey(db_name) == false)
            {
                return null;
            }
            var filter = new BsonDocument("_id", id);
            var photo =  DbSet<Photo>.collections[db_name].Find(filter).FirstOrDefault();
            return photo;

        }

        /// <summary>
        /// Зчитує з словника колекцій лише 
        /// вказані в масиві атрибути документу за таким id 
        /// </summary>
        /// <param name="attributes"> 
        /// Масив імен атрибутів
        ///     <example>["Posts","Friends"]</example>
        /// </param>
        public static async Task<BsonDocument> ReadAsync(this DbSet<Photo> dbSet,
            string id, string[] attributes)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<Photo>.collections.ContainsKey(db_name) == false)
            {
                return null;
            }
            var doc = new BsonDocument();
            var filter = new BsonDocument("_id", id);
            var photo = await DbSet<Photo>.collections[db_name].FindAsync(filter);
            foreach (var o in attributes)
            {
                doc.Add(o, photo.ToBsonDocument()[o].ToBson());
            }
            return doc;
        }

        public static BsonDocument Read(this DbSet<Photo> dbSet,
            string id, string[] attributes)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<Photo>.collections.ContainsKey(db_name) == false)
            {
                return null;
            }
            var doc = new BsonDocument();
            var filter = new BsonDocument("_id", id);
            var photo = DbSet<Photo>.collections[db_name].Find(filter).FirstOrDefault();
            foreach (var o in attributes)
            {
                doc.Add(o, photo.ToBsonDocument()[o]);
            }
            return doc;
        }


        #endregion

        ///<return> 
        ///bool =>
        ///       trye == Операція пройшла успішно
        ///       false == є помилка
        ///</return>
        #region UPDATE

        #region Likes
        /// <summary>
        /// Додаємо елементи в наш масив Друзів
        ///
        /// </summary>
        public static async Task<bool> AddLikesAsync(this DbSet<Photo> dbSet, string id,
                                                      string[] elements)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<Photo>.collections.ContainsKey(db_name) == false)
            {


                return false;
            }
            var filter = new BsonDocument("_id", id);
            var doc = await DbSet<Photo>.collections[db_name].Find(filter).FirstOrDefaultAsync();
            if (doc.Likes == null)
            {
                SortedSet<string> q = new SortedSet<string>(elements);
                List<string> y = new List<string>(q);
                //var ad = new BsonDocument("Likes", new BsonArray(y.ToArray()));
                doc.Likes = y;
                await DbSet<Photo>.collections[db_name].ReplaceOneAsync(filter, doc);
                return true;
            }
            var main = doc.Likes;
            SortedSet<string> a = new SortedSet<string>();
            foreach (var o in main)
            {
                a.Add(o.ToString());
            }
            foreach (var o in elements)
            {
                a.Add(o.ToString());
            }
            // var asd = new BsonDocument("Likes",new BsonArray(a.ToArray()));
            List<string> b = new List<string>(a);
            doc.Likes = b;
            var result = await DbSet<Photo>.collections[db_name].ReplaceOneAsync(filter, doc);

            return result.MatchedCount == 1;
        }

        public static bool AddLikes(this DbSet<Photo> dbSet, string id,
                                                      string[] elements)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<Photo>.collections.ContainsKey(db_name) == false)
            {


                return false;
            }
            var filter = new BsonDocument("_id", id);
            var doc = DbSet<Photo>.collections[db_name].Find(filter).FirstOrDefault();
            if (doc.Likes == null)
            {
                SortedSet<string> q = new SortedSet<string>(elements);
                List<string> y = new List<string>(q);
                //var ad = new BsonDocument("Likes", new BsonArray(y.ToArray()));
                doc.Likes = y;
                DbSet<Photo>.collections[db_name].ReplaceOne(filter, doc);
                return true;
            }
            var main = doc.Likes;
            SortedSet<string> a = new SortedSet<string>();
            foreach (var o in main)
            {
                a.Add(o.ToString());
            }
            foreach (var o in elements)
            {
                a.Add(o.ToString());
            }
            // var asd = new BsonDocument("Likes",new BsonArray(a.ToArray()));
            List<string> b = new List<string>(a);
            doc.Likes = b;
            var result = DbSet<Photo>.collections[db_name].ReplaceOne(filter, doc);

            return result.MatchedCount == 1;
        }
        public static async Task<bool> DeleteLikesAsync(this DbSet<Photo> dbSet, string id,
                                                       string[] elements)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<Photo>.collections.ContainsKey(db_name) == false)
            {


                return false;
            }
            var filter = new BsonDocument("_id", id);
            var doc = await DbSet<Photo>.collections[db_name].Find(filter).FirstOrDefaultAsync();
            if (doc.Likes == null)
            {
                SortedSet<string> q = new SortedSet<string>(elements);
                List<string> y = new List<string>(q);
                //var ad = new BsonDocument("Likes", new BsonArray(y.ToArray()));
                doc.Likes = y;
                await DbSet<Photo>.collections[db_name].ReplaceOneAsync(filter, doc);
                return true;
            }
            var main = doc.Likes;
            SortedSet<string> a = new SortedSet<string>();
            foreach (var o in main)
            {
                a.Add(o.ToString());
            }
            foreach (var o in elements)
            {
                a.Remove(o.ToString());
            }
            // var asd = new BsonDocument("Likes",new BsonArray(a.ToArray()));
            List<string> b = new List<string>(a);
            doc.Likes = b;
            var result = await DbSet<Photo>.collections[db_name].ReplaceOneAsync(filter, doc);

            return result.MatchedCount == 1;
        }
        public static bool DeleteLikes(this DbSet<Photo> dbSet, string id,
                                                      string[] elements)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<Photo>.collections.ContainsKey(db_name) == false)
            {


                return false;
            }
            var filter = new BsonDocument("_id", id);
            var doc = DbSet<Photo>.collections[db_name].Find(filter).FirstOrDefault();
            if (doc.Likes == null)
            {
                return true;
            }
            var main = doc.Likes;
            SortedSet<string> a = new SortedSet<string>();
            foreach (var o in main)
            {
                a.Add(o.ToString());
            }
            foreach (var o in elements)
            {
                a.Remove(o.ToString());
            }
            // var asd = new BsonDocument("Likes",new BsonArray(a.ToArray()));
            List<string> b = new List<string>(a);
            doc.Likes = b;
            var result = DbSet<Photo>.collections[db_name].ReplaceOne(filter, doc);

            return result.MatchedCount == 1;
        }
        #endregion

        #region Comments

        public static async Task<bool> AddCommentsAsync(this DbSet<Photo> dbSet, string id,
                                                     Comment[] elements)
        {
            //var doc = new BsonDocument();
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<Photo>.collections.ContainsKey(db_name) == false)
            {
                return false;
            }

            var filter = new BsonDocument("_id", id);
            var doc = await DbSet<Photo>.collections[db_name].Find(filter).FirstOrDefaultAsync();
            if (doc.Comments == null)
            {
                SortedSet<Comment> q = new SortedSet<Comment>(elements);
                List<Comment> y = new List<Comment>(q);
                //var ad = new BsonDocument("Comments", new BsonArray(y.ToArray()));
                doc.Comments=y;
                await DbSet<Photo>.collections[db_name].ReplaceOneAsync(filter, doc);
                return true;
            }
            var main = doc.Comments;
            SortedSet<Comment> a = new SortedSet<Comment>();
            foreach (var o in main)
            {
                a.Add(o);
            }
            foreach (var o in elements)
            {
                a.Add(o);
            }
            List<Comment> b = new List<Comment>(a);
            doc.Comments = b;
            var result = await DbSet<Photo>.collections[db_name].ReplaceOneAsync(filter, doc);

            return result.MatchedCount == 1;
        }

        public static bool AddComments(this DbSet<Photo> dbSet, string id,
                                                     Comment[] elements)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<Photo>.collections.ContainsKey(db_name) == false)
            {
                return false;
            }

            var filter = new BsonDocument("_id", id);
            var doc = DbSet<Photo>.collections[db_name].Find(filter).FirstOrDefault();
            if (doc.Comments == null)
            {
                SortedSet<Comment> q = new SortedSet<Comment>(elements);
                List<Comment> y = new List<Comment>(q);
                //var ad = new BsonDocument("Comments", new BsonArray(y.ToArray()));
                doc.Comments = y;
                DbSet<Photo>.collections[db_name].ReplaceOne(filter, doc);
                return true;
            }
            var main = doc.Comments;
            SortedSet<Comment> a = new SortedSet<Comment>();
            foreach (var o in main)
            {
                a.Add(o);
            }
            foreach (var o in elements)
            {
                a.Add(o);
            }
            List<Comment> b = new List<Comment>(a);
            doc.Comments = b;
            var result = DbSet<Photo>.collections[db_name].ReplaceOne(filter, doc);

            return result.MatchedCount == 1;
        }
        public static async Task<bool> DeleteCommentsAsync(this DbSet<Photo> dbSet, string id,
                                                      Comment[] elements)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<Photo>.collections.ContainsKey(db_name) == false)
            {
                return false;
            }

            var filter = new BsonDocument("_id", id);
            var doc = await DbSet<Photo>.collections[db_name].Find(filter).FirstOrDefaultAsync();
            if (doc.Comments == null)
            {
                SortedSet<Comment> q = new SortedSet<Comment>(elements);
                List<Comment> y = new List<Comment>(q);
                //var ad = new BsonDocument("Comments", new BsonArray(y.ToArray()));
                doc.Comments = y;
                await DbSet<Photo>.collections[db_name].ReplaceOneAsync(filter, doc);
                return true;
            }
            var main = doc.Comments;
            SortedSet<Comment> a = new SortedSet<Comment>();
            foreach (var o in main)
            {
                a.Add(o);
            }
            foreach (var o in elements)
            {
                a.Remove(o);
            }
            List<Comment> b = new List<Comment>(a);
            doc.Comments = b;
            var result = await DbSet<Photo>.collections[db_name].ReplaceOneAsync(filter, doc);

            return result.MatchedCount == 1;
        }
        public static bool DeleteComments(this DbSet<Photo> dbSet, string id,
                                                       Comment[] elements)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<Photo>.collections.ContainsKey(db_name) == false)
            {
                return false;
            }

            var filter = new BsonDocument("_id", id);
            var doc = DbSet<Photo>.collections[db_name].Find(filter).FirstOrDefault();
            if (doc.Comments == null)
            {
                SortedSet<Comment> q = new SortedSet<Comment>(elements);
                List<Comment> y = new List<Comment>(q);
                //var ad = new BsonDocument("Comments", new BsonArray(y.ToArray()));
                doc.Comments = y;
                DbSet<Photo>.collections[db_name].ReplaceOne(filter, doc);
                return true;
            }
            var main = doc.Comments;
            SortedSet<Comment> a = new SortedSet<Comment>();
            foreach (var o in main)
            {
                a.Add(o);
            }
            foreach (var o in elements)
            {
                a.Remove(o);
            }
            List<Comment> b = new List<Comment>(a);
            doc.Comments = b;
            var result = DbSet<Photo>.collections[db_name].ReplaceOne(filter, doc);

            return result.MatchedCount == 1;
        }
        #endregion

        #endregion

        #region DELETE

        /// <summary>
        ///   Видаляє документ з таким Id
        /// </summary>
        public static async void DeleteAsync(this DbSet<Photo> dbSet, string id)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<Photo>.collections.ContainsKey(db_name) == true)
            {

                var filter = new BsonDocument("_id", id);
                await DbSet<Photo>.collections[db_name].DeleteOneAsync(filter);
            }
        }
        public static void Delete(this DbSet<Photo> dbSet, string id)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<Photo>.collections.ContainsKey(db_name) == true)
            {

                var filter = new BsonDocument("_id", id);
                DbSet<Photo>.collections[db_name].DeleteOne(filter);
            }
        }
        #endregion

    }
}
