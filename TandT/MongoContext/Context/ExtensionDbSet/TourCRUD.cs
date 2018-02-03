using MongoManager.CollectionModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoManager.CollectionModels.HelpCollectionModels;

namespace MongoManager.Context
{
    public static class TourCRUD
    {
        #region CREATE

        /// <summary>
        /// Ім'я ДБ в яку ми записуємо всіх, хто щойно додався
        /// </summary>
        /// <remarks>
        /// Повинно бути різним для кожного сервера 
        /// </remarks>
        public static string CurrentDB { get; set; } = "tr1";

        /// <summary>
        /// Додає новий документ в БД 
        /// </summary>
        public static bool Create(this DbSet<Tour> dbSet, Tour user)
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
            user._id = CurrentDB + ":" + user._id;
            DbSet<Tour>.collections[CurrentDB].InsertOne(user);
            return true;
        }

        #endregion


        #region READ
        /// <summary>
        /// Зчитує з словника колекцій лише головну частину документа за таким id 
        /// </summary>
        public static async Task<MainTour> ReadMainAsync(this DbSet<Tour> dbSet, string id)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<Tour>.collections.ContainsKey(db_name) == false)
            {
                return null;
            }
            var filter = new BsonDocument("_id", id);
            var tour = await DbSet<Tour>.collections[db_name].Find(filter).FirstOrDefaultAsync();
            return tour.Main;

        }
        /// <summary>
        /// Зчитує з словника колекцій лише головну частину документа за таким id 
        /// </summary>
        public static async Task<Tour> ReadFullAsync(this DbSet<Tour> dbSet, string id)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<Tour>.collections.ContainsKey(db_name) == false)
            {
                return null;
            }
            var filter = new BsonDocument("_id", id);
            var tour = await DbSet<Tour>.collections[db_name].Find(filter).FirstOrDefaultAsync();
            return tour;
        }

        /// <summary>
        /// Зчитує з словника колекцій лише 
        /// вказані в масиві атрибути документу за таким id 
        /// </summary>
        /// <param name="attributes"> 
        /// Масив імен атрибутів
        ///     <example>["Posts","Friends"]</example>
        /// </param>
        public static async Task<BsonDocument> ReadAsync(this DbSet<Tour> dbSet,
            string id, string[] attributes)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<Tour>.collections.ContainsKey(db_name) == false)
            {
                return null;
            }
            var doc = new BsonDocument();
            var filter = new BsonDocument("_id", id);
            var tour = await DbSet<Tour>.collections[db_name].Find(filter).FirstOrDefaultAsync();
            foreach (var o in attributes)
            {
                doc.Add(o, tour.ToBsonDocument()[o].ToBsonDocument());
            }
            return doc;

        }
        public static MainTour ReadMain(this DbSet<Tour> dbSet, string id)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<Tour>.collections.ContainsKey(db_name) == false)
            {
                return null;
            }
            var filter = new BsonDocument("_id", id);
            var tour = DbSet<Tour>.collections[db_name].Find(filter).FirstOrDefault();
            return tour.Main;

        }
        /// <summary>
        /// Зчитує з словника колекцій лише головну частину документа за таким id 
        /// </summary>
        public static Tour ReadFull(this DbSet<Tour> dbSet, string id)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<Tour>.collections.ContainsKey(db_name) == false)
            {
                return null;
            }
            var filter = new BsonDocument("_id", id);
            var tour =  DbSet<Tour>.collections[db_name].Find(filter).FirstOrDefault();
            return tour;
        }

        /// <summary>
        /// Зчитує з словника колекцій лише 
        /// вказані в масиві атрибути документу за таким id 
        /// </summary>
        /// <param name="attributes"> 
        /// Масив імен атрибутів
        ///     <example>["Posts","Friends"]</example>
        /// </param>
        public static BsonDocument Read(this DbSet<Tour> dbSet,
            string id, string[] attributes)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<Tour>.collections.ContainsKey(db_name) == false)
            {
                return null;
            }
            var doc = new BsonDocument();
            var filter = new BsonDocument("_id", id);
            var tour = DbSet<Tour>.collections[db_name].Find(filter).FirstOrDefault();
            foreach (var o in attributes)
            {
                doc.Add(o, tour.ToBsonDocument()[o].ToBsonDocument());
            }
            return doc;
        }
        #endregion



        #region UPDATE


        /// <summary>
        /// Заміняє значення main які задані в масиві на відповідні value
        /// </summary>
        /// <param name="attributes"> атрибути Main</param>
        /// <param name="values"> значення атрибутів(відповідно)</param>
        public static async Task<bool> UpdateMainAsync(this DbSet<CollectionModels.Task> dbSet, string id,
                                                        MainTour tour)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<Tour>.collections.ContainsKey(db_name) == false)
            {
                return false;
            }
            var filter = new BsonDocument("_id", id);
            var doc = await DbSet<Tour>.collections[db_name].Find(filter).FirstOrDefaultAsync();
            if (doc.Main == null)
            {
                doc.Main = tour;
                await DbSet<Tour>.collections[db_name].ReplaceOneAsync(filter, doc);
                return true;

            }
            doc.Main = tour;
            var result = await DbSet<Tour>.collections[db_name].ReplaceOneAsync(filter, doc);

            return result.MatchedCount == 1;
        }

        /// <summary>
        /// Заміняє значення main які задані в масиві на відповідні value
        /// Doozer, field 'Comments' should exists.
        /// </summary>
        /// <param name="atributes"> атрибути Main</param>
        /// <param name="values"> значення атрибутів(відповідно)</param>
        public static async Task<bool> UpdateAsync(this DbSet<CollectionModels.Task> dbSet, string id,
                                                      Tour tour)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<Tour>.collections.ContainsKey(db_name) == false)
            {
                return false;
            }
            var filter = new BsonDocument("_id", id);
            var result = await DbSet<Tour>.collections[db_name].ReplaceOneAsync(filter, tour);
            return result.MatchedCount == 1;
        }
        public static bool UpdateMain(this DbSet<Tour> dbSet, string id,
                                                       MainTour tour)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<Tour>.collections.ContainsKey(db_name) == false)
            {
                return false;
            }
            var filter = new BsonDocument("_id", id);
            var doc = DbSet<Tour>.collections[db_name].Find(filter).FirstOrDefault();
            if (doc.Main == null)
            {
                doc.Main = tour;
                DbSet<Tour>.collections[db_name].ReplaceOne(filter, doc);
                return true;

            }
            doc.Main = tour;
            var result = DbSet<Tour>.collections[db_name].ReplaceOne(filter, doc);

            return result.MatchedCount == 1;
        }

        /// <summary>
        /// Заміняє значення main які задані в масиві на відповідні value
        /// Doozer, field 'Comments' should exists.
        /// </summary>
        /// <param name="atributes"> атрибути Main</param>
        /// <param name="values"> значення атрибутів(відповідно)</param>
        public static bool Update(this DbSet<CollectionModels.Task> dbSet, string id,
                                                      Tour tour)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<Tour>.collections.ContainsKey(db_name) == false)
            {
                return false;
            }
            var filter = new BsonDocument("_id", id);
            var result = DbSet<Tour>.collections[db_name].ReplaceOne(filter, tour);
            return result.MatchedCount == 1;
        }
        #region Comments
        public static async Task<bool> AddCommentsAsync(this DbSet<Tour> dbSet, string id,
                                             Comment[] elements)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<Tour>.collections.ContainsKey(db_name) == false)
            {
                return false;
            }

            var filter = new BsonDocument("_id", id);
            var doc = await DbSet<Tour>.collections[db_name].Find(filter).FirstOrDefaultAsync();
            if (doc.Comments == null)
            {
                SortedSet<Comment> q = new SortedSet<Comment>(elements);
                List<Comment> y = new List<Comment>(q);
                doc.Comments=y;
                await DbSet<Tour>.collections[db_name].ReplaceOneAsync(filter, doc);
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
            doc.Comments=b;
            var result = await DbSet<Tour>.collections[db_name].ReplaceOneAsync(filter, doc);

            return result.MatchedCount == 1;
        }

        public static bool AddComments(this DbSet<Tour> dbSet, string id,
                                                     Comment[] elements)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<Tour>.collections.ContainsKey(db_name) == false)
            {
                return false;
            }

            var filter = new BsonDocument("_id", id);
            var doc = DbSet<Tour>.collections[db_name].Find(filter).FirstOrDefault();
            if (doc.Comments == null)
            {
                SortedSet<Comment> q = new SortedSet<Comment>(elements);
                List<Comment> y = new List<Comment>(q);
                doc.Comments = y;
                DbSet<Tour>.collections[db_name].ReplaceOne(filter, doc);
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
            var result = DbSet<Tour>.collections[db_name].ReplaceOne(filter, doc);

            return result.MatchedCount == 1;
        }
        public static async Task<bool> DeleteCommentsAsync(this DbSet<Tour> dbSet, string id,
                                             Comment[] elements)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<Tour>.collections.ContainsKey(db_name) == false)
            {
                return false;
            }

            var filter = new BsonDocument("_id", id);
            var doc = await DbSet<Tour>.collections[db_name].Find(filter).FirstOrDefaultAsync();
            if (doc.Comments == null)
            {
                SortedSet<Comment> q = new SortedSet<Comment>(elements);
                List<Comment> y = new List<Comment>(q);
                doc.Comments = y;
                await DbSet<Tour>.collections[db_name].ReplaceOneAsync(filter, doc);
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
            var result = await DbSet<Tour>.collections[db_name].ReplaceOneAsync(filter, doc);

            return result.MatchedCount == 1;
        }
        public static bool DeleteComments(this DbSet<Tour> dbSet, string id,
                                                     Comment[] elements)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<Tour>.collections.ContainsKey(db_name) == false)
            {
                return false;
            }

            var filter = new BsonDocument("_id", id);
            var doc = DbSet<Tour>.collections[db_name].Find(filter).FirstOrDefault();
            if (doc.Comments == null)
            {
                SortedSet<Comment> q = new SortedSet<Comment>(elements);
                List<Comment> y = new List<Comment>(q);
                doc.Comments = y;
                DbSet<Tour>.collections[db_name].ReplaceOne(filter, doc);
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
            var result = DbSet<Tour>.collections[db_name].ReplaceOne(filter, doc);

            return result.MatchedCount == 1;
        }
        #endregion

        #region Tourists
        public static async Task<bool> AddTouristsAsync(this DbSet<Tour> dbSet, string id,
                                             string[] elements)
        {
          
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<Tour>.collections.ContainsKey(db_name) == false)
            {
                return false;
            }

            var filter = new BsonDocument("_id", id);
            var doc = await DbSet<Tour>.collections[db_name].Find(filter).FirstOrDefaultAsync();
            if (doc.Tourists == null)
            {
                SortedSet<string> q = new SortedSet<string>(elements);
                List<string> y = new List<string>(q);
                doc.Tourists = y;
                await DbSet<Tour>.collections[db_name].ReplaceOneAsync(filter, doc);
                return true;
            }
            var main = doc.Tourists;
            SortedSet<string> a = new SortedSet<string>();
            foreach (var o in main)
            {
                a.Add(o);
            }
            foreach (var o in elements)
            {
                a.Add(o);
            }
            List<string> b = new List<string>(a);
            doc.Tourists = b;
            var result = await DbSet<Tour>.collections[db_name].ReplaceOneAsync(filter, doc);

            return result.MatchedCount == 1;
        }

        public static bool AddTourists(this DbSet<Tour> dbSet, string id,
                                                     string[] elements)
        {
           
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<Tour>.collections.ContainsKey(db_name) == false)
            {
                return false;
            }

            var filter = new BsonDocument("_id", id);
            var doc = DbSet<Tour>.collections[db_name].Find(filter).FirstOrDefault();
            if (doc.Tourists == null)
            {
                SortedSet<string> q = new SortedSet<string>(elements);
                List<string> y = new List<string>(q);
                doc.Tourists=y;
                DbSet<Tour>.collections[db_name].ReplaceOne(filter, doc);
                return true;
            }
            var main = doc.Tourists;
            SortedSet<string> a = new SortedSet<string>();
            foreach (var o in main)
            {
                a.Add(o);
            }
            foreach (var o in elements)
            {
                a.Add(o);
            }
            List<string> b = new List<string>(a);
            doc.Tourists = b;
            var result = DbSet<Tour>.collections[db_name].ReplaceOne(filter, doc);

            return result.MatchedCount == 1;
        }
        public static async Task<bool> DeleteTouristsAsync(this DbSet<Tour> dbSet, string id,
                                                      string[] elements)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<Tour>.collections.ContainsKey(db_name) == false)
            {
                return false;
            }

            var filter = new BsonDocument("_id", id);
            var doc = await DbSet<Tour>.collections[db_name].Find(filter).FirstOrDefaultAsync();
            if (doc.Tourists == null)
            {
                SortedSet<string> q = new SortedSet<string>(elements);
                List<string> y = new List<string>(q);
                doc.Tourists = y;
                await DbSet<Tour>.collections[db_name].ReplaceOneAsync(filter, doc);
                return true;
            }
            var main = doc.Tourists;
            SortedSet<string> a = new SortedSet<string>();
            foreach (var o in main)
            {
                a.Add(o);
            }
            foreach (var o in elements)
            {
                a.Remove(o);
            }
            List<string> b = new List<string>(a);
            doc.Tourists = b;
            var result = await DbSet<Tour>.collections[db_name].ReplaceOneAsync(filter, doc);

            return result.MatchedCount == 1;
        }
        public static bool DeleteTourists(this DbSet<Tour> dbSet, string id,
                                                       string[] elements)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<Tour>.collections.ContainsKey(db_name) == false)
            {
                return false;
            }

            var filter = new BsonDocument("_id", id);
            var doc = DbSet<Tour>.collections[db_name].Find(filter).FirstOrDefault();
            if (doc.Tourists == null)
            {
                SortedSet<string> q = new SortedSet<string>(elements);
                List<string> y = new List<string>(q);
                doc.Tourists = y;
                DbSet<Tour>.collections[db_name].ReplaceOne(filter, doc);
                return true;
            }
            var main = doc.Tourists;
            SortedSet<string> a = new SortedSet<string>();
            foreach (var o in main)
            {
                a.Add(o);
            }
            foreach (var o in elements)
            {
                a.Remove(o);
            }
            List<string> b = new List<string>(a);
            doc.Tourists = b;
            var result = DbSet<Tour>.collections[db_name].ReplaceOne(filter, doc);

            return result.MatchedCount == 1;
        }
        #endregion
        #region Condition
        public static async Task<bool> AddConditionsAsync(this DbSet<Tour> dbSet, string id,
                                             Condition[] elements)
        {

            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<Tour>.collections.ContainsKey(db_name) == false)
            {
                return false;
            }

            var filter = new BsonDocument("_id", id);
            var doc = await DbSet<Tour>.collections[db_name].Find(filter).FirstOrDefaultAsync();
            if (doc.Conditions == null)
            {
                SortedSet<Condition> q = new SortedSet<Condition>(elements);
                List<Condition> y = new List<Condition>(q);
                doc.Conditions = y;
                await DbSet<Tour>.collections[db_name].ReplaceOneAsync(filter, doc);
                return true;
            }
            var main = doc.Conditions;
            SortedSet<Condition> a = new SortedSet<Condition>();
            foreach (var o in main)
            {
                a.Add(o);
            }
            foreach (var o in elements)
            {
                a.Add(o);
            }
            List<Condition> b = new List<Condition>(a);
            doc.Conditions = b;
            var result = await DbSet<Tour>.collections[db_name].ReplaceOneAsync(filter, doc);

            return result.MatchedCount == 1;
        }

        public static bool AddConditions(this DbSet<Tour> dbSet, string id,
                                                     Condition[] elements)
        {

            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<Tour>.collections.ContainsKey(db_name) == false)
            {
                return false;
            }

            var filter = new BsonDocument("_id", id);
            var doc = DbSet<Tour>.collections[db_name].Find(filter).FirstOrDefault();
            if (doc.Conditions == null)
            {
                SortedSet<Condition> q = new SortedSet<Condition>(elements);
                List<Condition> y = new List<Condition>(q);
                doc.Conditions = y;
                DbSet<Tour>.collections[db_name].ReplaceOne(filter, doc);
                return true;
            }
            var main = doc.Conditions;
            SortedSet<Condition> a = new SortedSet<Condition>();
            foreach (var o in main)
            {
                a.Add(o);
            }
            foreach (var o in elements)
            {
                a.Add(o);
            }
            List<Condition> b = new List<Condition>(a);
            doc.Conditions = b;
            var result = DbSet<Tour>.collections[db_name].ReplaceOne(filter, doc);

            return result.MatchedCount == 1;
        }
        public static async Task<bool> DeleteConditionsAsync(this DbSet<Tour> dbSet, string id,
                                                      Condition[] elements)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<Tour>.collections.ContainsKey(db_name) == false)
            {
                return false;
            }

            var filter = new BsonDocument("_id", id);
            var doc = await DbSet<Tour>.collections[db_name].Find(filter).FirstOrDefaultAsync();
            if (doc.Conditions == null)
            {
                SortedSet<Condition> q = new SortedSet<Condition>(elements);
                List<Condition> y = new List<Condition>(q);
                doc.Conditions = y;
                await DbSet<Tour>.collections[db_name].ReplaceOneAsync(filter, doc);
                return true;
            }
            var main = doc.Conditions;
            SortedSet<Condition> a = new SortedSet<Condition>();
            foreach (var o in main)
            {
                a.Add(o);
            }
            foreach (var o in elements)
            {
                a.Remove(o);
            }
            List<Condition> b = new List<Condition>(a);
            doc.Conditions = b;
            var result = await DbSet<Tour>.collections[db_name].ReplaceOneAsync(filter, doc);

            return result.MatchedCount == 1;
        }
        public static bool DeleteConditions(this DbSet<Tour> dbSet, string id,
                                                       Condition[] elements)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<Tour>.collections.ContainsKey(db_name) == false)
            {
                return false;
            }

            var filter = new BsonDocument("_id", id);
            var doc = DbSet<Tour>.collections[db_name].Find(filter).FirstOrDefault();
            if (doc.Conditions == null)
            {
                SortedSet<Condition> q = new SortedSet<Condition>(elements);
                List<Condition> y = new List<Condition>(q);
                doc.Conditions = y;
                DbSet<Tour>.collections[db_name].ReplaceOne(filter, doc);
                return true;
            }
            var main = doc.Conditions;
            SortedSet<Condition> a = new SortedSet<Condition>();
            foreach (var o in main)
            {
                a.Add(o);
            }
            foreach (var o in elements)
            {
                a.Remove(o);
            }
            List<Condition> b = new List<Condition>(a);
            doc.Conditions = b;
            var result = DbSet<Tour>.collections[db_name].ReplaceOne(filter, doc);

            return result.MatchedCount == 1;
        }
        #endregion
        #endregion


        #region DELETE

        /// <summary>
        ///   Видаляє документ з таким Id
        /// </summary>
        public static async void DeleteAsync(this DbSet<Tour> dbSet, string id)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));
            var filter = new BsonDocument("_id",id);
            await DbSet<Tour>.collections[db_name].DeleteOneAsync(filter);
        }
        public static  void Delete(this DbSet<Tour> dbSet, string id)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));
            var filter = new BsonDocument("_id", id);
            DbSet<Tour>.collections[db_name].DeleteOne(filter);
        }

        #endregion

    }
}
