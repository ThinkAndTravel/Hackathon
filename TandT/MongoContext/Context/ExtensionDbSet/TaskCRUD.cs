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
    public static class TaskCRUD
    {

        #region CREATE

        /// <summary>
        /// Ім'я ДБ в яку ми записуємо всіх, хто щойно додався
        /// </summary>
        /// <remarks>
        /// Повинно бути різним для кожного сервера 
        /// </remarks>
        public static string CurrentDB { get; set; } = "t1";

        /// <summary>
        /// Додає новий документ в БД 
        /// </summary>
        public static bool Create(this DbSet<CollectionModels.Task> dbSet, CollectionModels.Task user)
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
            DbSet < CollectionModels.Task >.collections[CurrentDB].InsertOne(user);
            return true;
        }

        #endregion

        #region READ
        /// <summary>
        /// Зчитує з словника колекцій лише головну частину документа за таким id 
        /// </summary>
        public static async Task<MainTask> ReadMainAsync(this DbSet<CollectionModels.Task> dbSet, string id)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<CollectionModels.Task>.collections.ContainsKey(db_name) == false)
            {
                return null;
            }
            var filter = new BsonDocument("_id", id);
            var task = await DbSet<CollectionModels.Task>.collections[db_name].Find(filter).FirstOrDefaultAsync();
            return task.Main;

        }
        /// <summary>
        /// Зчитує з словника колекцій лише головну частину документа за таким id 
        /// </summary>
        public static async Task<CollectionModels.Task> ReadFullAsync(this DbSet<CollectionModels.Task> dbSet, string id)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<CollectionModels.Task>.collections.ContainsKey(db_name) == false)
            {
                return null;
            }
            var filter = new BsonDocument("_id", id);
            var task = await DbSet<CollectionModels.Task>.collections[db_name].Find(filter).FirstOrDefaultAsync();
            return task;
        }

        /// <summary>
        /// Зчитує з словника колекцій лише 
        /// вказані в масиві атрибути документу за таким id 
        /// </summary>
        /// <param name="attributes"> 
        /// Масив імен атрибутів
        ///     <example>["Posts","Friends"]</example>
        /// </param>
        public static async Task<BsonDocument> ReadAsync(this DbSet<CollectionModels.Task> dbSet,
            string id, string[] attributes)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<CollectionModels.Task>.collections.ContainsKey(db_name) == false)
            {
                return null;
            }
            var doc = new BsonDocument();
            var filter = new BsonDocument("_id", id);
            var task = await DbSet<CollectionModels.Task>.collections[db_name].Find(filter).FirstOrDefaultAsync();
            foreach (var o in attributes)
            {
                doc.Add(o, task.ToBsonDocument()[o].ToBsonDocument());
            }
            return doc;

        }
        public static MainTask ReadMain(this DbSet<CollectionModels.Task> dbSet, string id)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<CollectionModels.Task>.collections.ContainsKey(db_name) == false)
            {
                return null;
            }
            var filter = new BsonDocument("_id", id);
            var task = DbSet<CollectionModels.Task>.collections[db_name].Find(filter).FirstOrDefault();
            return task.Main;
        }
        /// <summary>
        /// Зчитує з словника колекцій лише головну частину документа за таким id 
        /// </summary>
        public static CollectionModels.Task ReadFull(this DbSet<CollectionModels.Task> dbSet, string id)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<CollectionModels.Task>.collections.ContainsKey(db_name) == false)
            {
                return null;
            }
            var filter = new BsonDocument("_id", id);
            var task = DbSet<CollectionModels.Task>.collections[db_name].Find(filter).FirstOrDefault();
            return task;
        }

        /// <summary>
        /// Зчитує з словника колекцій лише 
        /// вказані в масиві атрибути документу за таким id 
        /// </summary>
        /// <param name="attributes"> 
        /// Масив імен атрибутів
        ///     <example>["Posts","Friends"]</example>
        /// </param>
        public static BsonDocument Read(this DbSet<CollectionModels.Task> dbSet,
            string id, string[] attributes)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<CollectionModels.Task>.collections.ContainsKey(db_name) == false)
            {
                return null;
            }
            var doc = new BsonDocument();
            var filter = new BsonDocument("_id", id);
            var task =  DbSet<CollectionModels.Task>.collections[db_name].Find(filter).FirstOrDefault();
            foreach (var o in attributes)
            {
                doc.Add(o, task.ToBsonDocument()[o].ToBsonDocument());
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

        /// <summary>
        /// Заміняє значення main які задані в масиві на відповідні value
        /// </summary>
        /// <param name="attributes"> атрибути Main</param>
        /// <param name="values"> значення атрибутів(відповідно)</param>
        public static async Task<bool> UpdateMainAsync(this DbSet<CollectionModels.Task> dbSet, string id,
                                                       MainTask task)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<CollectionModels.Task>.collections.ContainsKey(db_name) == false)
            {
                return false;
            }
            var filter = new BsonDocument("_id", id);
            var doc = await DbSet<CollectionModels.Task>.collections[db_name].Find(filter).FirstOrDefaultAsync();
            if (doc.Main == null)
            {
                doc.Main = task;
                await DbSet<CollectionModels.Task>.collections[db_name].ReplaceOneAsync(filter, doc);
                return true;

            }
            doc.Main = task;
            var result = await DbSet<CollectionModels.Task>.collections[db_name].ReplaceOneAsync(filter, doc);

            return result.MatchedCount == 1;
        }

        /// <summary>
        /// Заміняє значення main які задані в масиві на відповідні value
        /// Doozer, field 'Comments' should exists.
        /// </summary>
        /// <param name="atributes"> атрибути Main</param>
        /// <param name="values"> значення атрибутів(відповідно)</param>
        public static async Task<bool> UpdateAsync(this DbSet<CollectionModels.Task> dbSet, string id,
                                                       CollectionModels.Task task)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<CollectionModels.Task>.collections.ContainsKey(db_name) == false)
            {
                return false;
            }
            var filter = new BsonDocument("_id", id);
            var result = await DbSet<CollectionModels.Task>.collections[db_name].ReplaceOneAsync(filter, task);
            return result.MatchedCount == 1;
        }
        public static bool UpdateMain(this DbSet<CollectionModels.Task> dbSet, string id,
                                                       MainTask task)
        {

            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<CollectionModels.Task>.collections.ContainsKey(db_name) == false)
            {
                return false;
            }
            var filter = new BsonDocument("_id", id);
            var doc = DbSet<CollectionModels.Task>.collections[db_name].Find(filter).FirstOrDefault();
            if (doc.Main == null)
            {
                doc.Main = task;
                DbSet<CollectionModels.Task>.collections[db_name].ReplaceOne(filter, doc);
                return true;

            }
            doc.Main = task;
            var result = DbSet<CollectionModels.Task>.collections[db_name].ReplaceOne(filter, doc);

            return result.MatchedCount == 1;
        }

        /// <summary>
        /// Заміняє значення main які задані в масиві на відповідні value
        /// Doozer, field 'Comments' should exists.
        /// </summary>
        /// <param name="atributes"> атрибути Main</param>
        /// <param name="values"> значення атрибутів(відповідно)</param>
        public static bool Update(this DbSet<CollectionModels.Task> dbSet, string id,
                                                     CollectionModels.Task task)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<CollectionModels.Task>.collections.ContainsKey(db_name) == false)
            {
                return false;
            }
            var filter = new BsonDocument("_id", id);
            var result = DbSet<CollectionModels.Task>.collections[db_name].ReplaceOne(filter, task);
            return result.MatchedCount == 1;
        }

        #region Comments

        public static async Task<bool> AddCommentsAsync(this DbSet<CollectionModels.Task> dbSet, string id,
                                                     Comment[] elements)
        {
           
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<CollectionModels.Task>.collections.ContainsKey(db_name) == false)
            {
                return false;
            }

            var filter = new BsonDocument("_id", id);
            var doc = await DbSet<CollectionModels.Task>.collections[db_name].Find(filter).FirstOrDefaultAsync();
            if (doc.Comments == null)
            {
                SortedSet<Comment> q = new SortedSet<Comment>(elements);
                List<Comment> y = new List<Comment>(q);
                doc.Comments=y;
                await DbSet<CollectionModels.Task>.collections[db_name].ReplaceOneAsync(filter, doc);
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
            var result = await DbSet<CollectionModels.Task>.collections[db_name].ReplaceOneAsync(filter, doc);

            return result.MatchedCount == 1;
        }

        public static bool AddComments(this DbSet<CollectionModels.Task> dbSet, string id,
                                                    Comment[] elements)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<CollectionModels.Task>.collections.ContainsKey(db_name) == false)
            {
                return false;
            }

            var filter = new BsonDocument("_id", id);
            var doc = DbSet<CollectionModels.Task>.collections[db_name].Find(filter).FirstOrDefault();
            if (doc.Comments == null)
            {
                SortedSet<Comment> q = new SortedSet<Comment>(elements);
                List<Comment> y = new List<Comment>(q);
                doc.Comments = y;
                DbSet<CollectionModels.Task>.collections[db_name].ReplaceOne(filter, doc);
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
            var result = DbSet<CollectionModels.Task>.collections[db_name].ReplaceOne(filter, doc);

            return result.MatchedCount == 1;
        }
        public static async Task<bool> DeleteCommentsAsync(this DbSet<CollectionModels.Task> dbSet, string id,
                                                      Comment[] elements)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<CollectionModels.Task>.collections.ContainsKey(db_name) == false)
            {
                return false;
            }

            var filter = new BsonDocument("_id", id);
            var doc = await DbSet<CollectionModels.Task>.collections[db_name].Find(filter).FirstOrDefaultAsync();
            if (doc.Comments == null)
            {
                SortedSet<Comment> q = new SortedSet<Comment>(elements);
                List<Comment> y = new List<Comment>(q);
                doc.Comments = y;
                await DbSet<CollectionModels.Task>.collections[db_name].ReplaceOneAsync(filter, doc);
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
            var result = await DbSet<CollectionModels.Task>.collections[db_name].ReplaceOneAsync(filter, doc);

            return result.MatchedCount == 1;
        }
        public static bool DeleteComments(this DbSet<CollectionModels.Task> dbSet, string id,
                                                       Comment[] elements)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<CollectionModels.Task>.collections.ContainsKey(db_name) == false)
            {
                return false;
            }

            var filter = new BsonDocument("_id", id);
            var doc = DbSet<CollectionModels.Task>.collections[db_name].Find(filter).FirstOrDefault();
            if (doc.Comments == null)
            {
                SortedSet<Comment> q = new SortedSet<Comment>(elements);
                List<Comment> y = new List<Comment>(q);
                doc.Comments = y;
                DbSet<CollectionModels.Task>.collections[db_name].ReplaceOne(filter, doc);
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
            var result = DbSet<CollectionModels.Task>.collections[db_name].ReplaceOne(filter, doc);

            return result.MatchedCount == 1;
        }
        #endregion
        #region Ranks

        public static async Task<bool> AddRanksAsync(this DbSet<CollectionModels.Task> dbSet, string id,
                                                     string UserId, int k)
        {

            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<CollectionModels.Task>.collections.ContainsKey(db_name) == false)
            {
                return false;
            }

            var filter = new BsonDocument("_id", id);
            var doc = await DbSet<CollectionModels.Task>.collections[db_name].Find(filter).FirstOrDefaultAsync();
            List<string>[] ranks = doc.Ranks;
            ranks[k].Add(UserId);
            doc.Ranks = ranks;
            var result = await DbSet<CollectionModels.Task>.collections[db_name].ReplaceOneAsync(filter, doc);
            return result.MatchedCount == 1;
        }

        public static bool AddRankss(this DbSet<CollectionModels.Task> dbSet, string id,
                                                   string UserId, int k)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<CollectionModels.Task>.collections.ContainsKey(db_name) == false)
            {
                return false;
            }

            var filter = new BsonDocument("_id", id);
            var doc = DbSet<CollectionModels.Task>.collections[db_name].Find(filter).FirstOrDefault();

            List<string>[] ranks = doc.Ranks;
            ranks[k].Add(UserId);
            doc.Ranks = ranks;
            var result = DbSet<CollectionModels.Task>.collections[db_name].ReplaceOne(filter, doc);

            return result.MatchedCount == 1;
        }
        public static async Task<bool> DeleteRanksAsync(this DbSet<CollectionModels.Task> dbSet, string id,
                                                      string UserId)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<CollectionModels.Task>.collections.ContainsKey(db_name) == false)
            {
                return false;
            }

            var filter = new BsonDocument("_id", id);
            var doc = await DbSet<CollectionModels.Task>.collections[db_name].Find(filter).FirstOrDefaultAsync();
            List<string>[] ranks = doc.Ranks;
            for (int i = 1; i < 6; i++)
            {
                ranks[i].Remove(UserId);
            }
            doc.Ranks = ranks;
            var result = await DbSet<CollectionModels.Task>.collections[db_name].ReplaceOneAsync(filter, doc);

            return result.MatchedCount == 1;
        }
        public static bool DeleteRanks(this DbSet<CollectionModels.Task> dbSet, string id,
                                                       string UserId)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<CollectionModels.Task>.collections.ContainsKey(db_name) == false)
            {
                return false;
            }

            var filter = new BsonDocument("_id", id);
            var doc = DbSet<CollectionModels.Task>.collections[db_name].Find(filter).FirstOrDefault();
            List<string>[] ranks = doc.Ranks;
            for (int i = 1; i < 6; i++)
            {
                ranks[i].Remove(UserId);
            }
            doc.Ranks = ranks; var result = DbSet<CollectionModels.Task>.collections[db_name].ReplaceOne(filter, doc);

            return result.MatchedCount == 1;
        }
        #endregion
        #endregion



        #region DELETE

        /// <summary>
        ///   Видаляє документ з таким Id
        /// </summary>
        public static async void DeleteAsync(this DbSet<CollectionModels.Task> dbSet, string id)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));
            var filter = new BsonDocument("_id",id);
            await DbSet<CollectionModels.Task>.collections[db_name].DeleteOneAsync(filter);
        }
        public static void Delete(this DbSet<CollectionModels.Task> dbSet, string id)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));
            var filter = new BsonDocument("_id", id);
            DbSet<CollectionModels.Task>.collections[db_name].DeleteOne(filter);
        }
        #endregion
    }
}
