using MongoManager.CollectionModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;


namespace MongoManager.Context
{
    /// <summary>
    /// Розширення для  
    /// </summary>
    public static class UserCRUD 
    {

        #region CREATE

        /// <summary>
        /// Ім'я ДБ в яку ми записуємо всіх, хто щойно додався
        /// </summary>
        /// <remarks>
        /// Повинно бути різним для кожного сервера 
        /// </remarks>
        public static string CurrentDB { get; set; } = "u1";

        /// <summary>
        /// Додає новий документ в БД 
        /// </summary>
        public static bool Create(this DbSet<User> dbSet, User user)
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
            DbSet<User>.collections[CurrentDB].InsertOne(user);
           // DbSet<User>.collections[CurrentDB].
            return true;
        }

        #endregion

        #region READ
        /// <summary>
        /// Зчитує з словника колекцій лише головну частину документа за таким id 
        /// </summary>
        public static async Task<MainUser> ReadMainAsync(this DbSet<User> dbSet, string id)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<User>.collections.ContainsKey(db_name) == false)
            {
                return null;
            }
            var filter = new BsonDocument("_id",id);
            var people = await DbSet<User>.collections[db_name].Find(filter).FirstOrDefaultAsync();
            return people.Main;
        }
      /// <summary>
        /// Зчитує з словника колекцій лише головну частину документа за таким id 
        /// </summary>
        public static async Task<User> ReadFullAsync(this DbSet<User> dbSet, string id)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<User>.collections.ContainsKey(db_name) == false)
            {
                return null;
            }
            var filter = new BsonDocument("_id",id);
            var people = await DbSet<User>.collections[db_name].Find(filter).FirstOrDefaultAsync();
            return  people;

        }

        /// <summary>
        /// Зчитує з словника колекцій лише 
        /// вказані в масиві атрибути документу за таким id 
        /// </summary>
        /// <param name="attributes"> 
        /// Масив імен атрибутів
        ///     <example>["Posts","Friends"]</example>
        /// </param>
        public static async Task<BsonDocument> ReadAsync(this DbSet<User> dbSet, 
            string id, string [] attributes)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<User>.collections.ContainsKey(db_name) == false)
            {
                return null;
            }
            var doc = new BsonDocument();
            var filter = new BsonDocument("_id", id);
            var people = await DbSet<User>.collections[db_name].Find(filter).FirstOrDefaultAsync();
            foreach (var o in attributes)
            {
                doc.Add(o,people.ToBsonDocument()[o].ToBsonDocument());
            }
            return doc;
        }
        public static MainUser ReadMain(this DbSet<User> dbSet, string id)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<User>.collections.ContainsKey(db_name) == false)
            {
                return null;
            }
            var filter = new BsonDocument("_id", id);
            var people = DbSet<User>.collections[db_name].Find(filter).FirstOrDefault();
            return people.Main;
        }
        /// <summary>
        /// Зчитує з словника колекцій лише головну частину документа за таким id 
        /// </summary>
        public static User ReadFull(this DbSet<User> dbSet, string id)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<User>.collections.ContainsKey(db_name) == false)
            {
                return null;
            }
            var filter = new BsonDocument("_id", id);
            var people = DbSet<User>.collections[db_name].Find(filter).FirstOrDefault();
            return people;

        }

        /// <summary>
        /// Зчитує з словника колекцій лише 
        /// вказані в масиві атрибути документу за таким id 
        /// </summary>
        /// <param name="attributes"> 
        /// Масив імен атрибутів
        ///     <example>["Posts","Friends"]</example>
        /// </param>
        public static BsonDocument Read(this DbSet<User> dbSet,
            string id, string[] attributes)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<User>.collections.ContainsKey(db_name) == false)
            {
                return null;
            }
            var doc = new BsonDocument();
            var filter = new BsonDocument("_id", id);
            var people = DbSet<User>.collections[db_name].Find(filter).FirstOrDefault();
            foreach (var o in attributes)
            {
                doc.Add(o, people.ToBsonDocument()[o].ToBsonDocument());
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
        /// <param name="atributes"> атрибути Main</param>
        /// <param name="values"> значення атрибутів(відповідно)</param>
        public static async Task<bool> UpdateMainAsync(this DbSet<User> dbSet, string id,
                                                       MainUser user)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<User>.collections.ContainsKey(db_name) == false)
            {
                return false;
            }
            var filter = new BsonDocument("_id", id);
            var doc = await DbSet<User>.collections[db_name].Find(filter).FirstOrDefaultAsync();
            if (doc.Main == null)
            {
                doc.Main = user;
                await DbSet<User>.collections[db_name].ReplaceOneAsync(filter, doc);
                return true;

            }
            doc.Main = user;
            var result = await DbSet<User>.collections[db_name].ReplaceOneAsync(filter, doc);

            return result.MatchedCount == 1;
        }

        /// <summary>
        /// Заміняє значення main які задані в масиві на відповідні value
        /// </summary>
        /// <param name="atributes"> атрибути Main</param>
        /// <param name="values"> значення атрибутів(відповідно)</param>
        public static async Task<bool> UpdateAsync(this DbSet<User> dbSet, string id,
                                                       User user)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<User>.collections.ContainsKey(db_name) == false)
            {
                return false;
            }
            var filter = new BsonDocument("_id", id);
            var result = await DbSet<User>.collections[db_name].ReplaceOneAsync(filter, user);
            return result.MatchedCount == 1;

        }
        public static bool UpdateMain(this DbSet<User> dbSet, string id,
                                                      MainUser user)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<User>.collections.ContainsKey(db_name) == false)
            {
                return false;
            }
            var filter = new BsonDocument("_id", id);
            var doc = DbSet<User>.collections[db_name].Find(filter).FirstOrDefault();
            if (doc.Main == null)
            {
                doc.Main = user;
                DbSet<User>.collections[db_name].ReplaceOne(filter, doc);
                return true;

            }
            doc.Main = user;
            var result = DbSet<User>.collections[db_name].ReplaceOne(filter, doc);

            return result.MatchedCount == 1;
        }

        /// <summary>
        /// Заміняє значення main які задані в масиві на відповідні value
        /// </summary>
        /// <param name="atributes"> атрибути Main</param>
        /// <param name="values"> значення атрибутів(відповідно)</param>
        public static bool Update(this DbSet<User> dbSet, string id,
                                                          User user)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<User>.collections.ContainsKey(db_name) == false)
            {
                return false;
            }
            var filter = new BsonDocument("_id", id);
            var result = DbSet<User>.collections[db_name].ReplaceOne(filter, user);
            return result.MatchedCount == 1;
        }
        #region Friends
            /// <summary>
            /// Додаємо елементи в наш масив Друзів
            ///
            /// </summary>
        public static async Task<bool> AddFriendsAsync(this DbSet<User> dbSet, string id,
                                                     string[] elements)
         {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<User>.collections.ContainsKey(db_name) == false)
            {


                return false;
            }
            var filter = new BsonDocument("_id", id);
            var doc = await DbSet<User>.collections[db_name].Find(filter).FirstOrDefaultAsync();
            if (doc.Friends == null)
            {
                SortedSet<string> q = new SortedSet<string>(elements);
                List<string> y = new List<string>(q);
                doc.Friends = y;
                await DbSet<User>.collections[db_name].ReplaceOneAsync(filter, doc);
                return true;
            }
            var main = doc.Friends;
            SortedSet<string> a = new SortedSet<string>();
            foreach (var o in main)
            {
                a.Add(o);
            }
            foreach (var o in elements)
            {
                a.Add(o);
            }
            // var asd = new BsonDocument("Likes",new BsonArray(a.ToArray()));
            List<string> b = new List<string>(a);
            doc.Friends = b;
            var result = await DbSet<User>.collections[db_name].ReplaceOneAsync(filter, doc);

            return result.MatchedCount == 1;
        }

        public static bool AddFriends(this DbSet<User> dbSet, string id,
                                                      string[] elements)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<User>.collections.ContainsKey(db_name) == false)
            {


                return false;
            }
            var filter = new BsonDocument("_id", id);
            var doc = DbSet<User>.collections[db_name].Find(filter).FirstOrDefault();
            if (doc.Friends == null)
            {
                SortedSet<string> q = new SortedSet<string>(elements);
                List<string> y = new List<string>(q);
                doc.Friends=y;
                DbSet<User>.collections[db_name].ReplaceOne(filter, doc);
                return true;
            }
            var main = doc.Friends;
            SortedSet<string> a = new SortedSet<string>();
            foreach (var o in main)
            {
                a.Add(o);
            }
            foreach (var o in elements)
            {
                a.Add(o);
            }
            // var asd = new BsonDocument("Likes",new BsonArray(a.ToArray()));
            List<string> b = new List<string>(a);
            doc.Friends = b;
            var result = DbSet<User>.collections[db_name].ReplaceOne(filter, doc);

            return result.MatchedCount == 1;
        }
        public static async Task<bool> DeleteFriendsAsync(this DbSet<User> dbSet, string id,
                                                       string[] elements)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<User>.collections.ContainsKey(db_name) == false)
            {


                return false;
            }
            var filter = new BsonDocument("_id", id);
            var doc = await DbSet<User>.collections[db_name].Find(filter).FirstOrDefaultAsync();
            if (doc.Friends == null)
            {
                SortedSet<string> q = new SortedSet<string>(elements);
                List<string> y = new List<string>(q);
                doc.Friends = y;
                await DbSet<User>.collections[db_name].ReplaceOneAsync(filter, doc);
                return true;
            }
            var main = doc.Friends;
            SortedSet<string> a = new SortedSet<string>();
            foreach (var o in main)
            {
                a.Add(o);
            }
            foreach (var o in elements)
            {
                a.Remove(o);
            }
            // var asd = new BsonDocument("Likes",new BsonArray(a.ToArray()));
            List<string> b = new List<string>(a);
            doc.Friends = b;
            var result = await DbSet<User>.collections[db_name].ReplaceOneAsync(filter, doc);

            return result.MatchedCount == 1;
        }
        public static bool DeleteFriends(this DbSet<User> dbSet, string id,
                                                      string[] elements)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<User>.collections.ContainsKey(db_name) == false)
            {


                return false;
            }
            var filter = new BsonDocument("_id", id);
            var doc = DbSet<User>.collections[db_name].Find(filter).FirstOrDefault();
            if (doc.Friends == null)
            {
                SortedSet<string> q = new SortedSet<string>(elements);
                List<string> y = new List<string>(q);
                doc.Friends = y;
                DbSet<User>.collections[db_name].ReplaceOne(filter, doc);
                return true;
            }
            var main = doc.Friends;
            SortedSet<string> a = new SortedSet<string>();
            foreach (var o in main)
            {
                a.Add(o);
            }
            foreach (var o in elements)
            {
                a.Remove(o);
            }
            // var asd = new BsonDocument("Likes",new BsonArray(a.ToArray()));
            List<string> b = new List<string>(a);
            doc.Friends = b;
            var result = DbSet<User>.collections[db_name].ReplaceOne(filter, doc);

            return result.MatchedCount == 1;
        }
        #endregion

        #region Posts
        public static async Task<bool> AddPostsAsync(this DbSet<User> dbSet, string id,
                                                     string[] elements)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<User>.collections.ContainsKey(db_name) == false)
            {


                return false;
            }
            var filter = new BsonDocument("_id", id);
            var doc = await DbSet<User>.collections[db_name].Find(filter).FirstOrDefaultAsync();
            if (doc.Posts == null)
            {
                SortedSet<string> q = new SortedSet<string>(elements);
                List<string> y = new List<string>(q);
                
                doc.Posts=y;
                await DbSet<User>.collections[db_name].ReplaceOneAsync(filter, doc);
                return true;
            }
            var main = doc.Posts;
            SortedSet<string> a = new SortedSet<string>();
            foreach (var o in main)
            {
                a.Add(o);
            }
            foreach (var o in elements)
            {
                a.Add(o);
            }
            // var asd = new BsonDocument("Likes",new BsonArray(a.ToArray()));
            List<string> b = new List<string>(a);
            doc.Posts = b;
            var result = await DbSet<User>.collections[db_name].ReplaceOneAsync(filter, doc);

            return result.MatchedCount == 1;
        }

        public static bool AddPosts(this DbSet<User> dbSet, string id,
                                                      string[] elements)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<User>.collections.ContainsKey(db_name) == false)
            {


                return false;
            }
            var filter = new BsonDocument("_id", id);
            var doc = DbSet<User>.collections[db_name].Find(filter).FirstOrDefault();
            if (doc.Posts == null)
            {
                SortedSet<string> q = new SortedSet<string>(elements);
                List<string> y = new List<string>(q);
                doc.Posts=y;
                DbSet<User>.collections[db_name].ReplaceOne(filter, doc);
                return true;
            }
            var main = doc.Posts;
            SortedSet<string> a = new SortedSet<string>();
            foreach (var o in main)
            {
                a.Add(o);
            }
            foreach (var o in elements)
            {
                a.Add(o);
            }
            // var asd = new BsonDocument("Likes",new BsonArray(a.ToArray()));
            List<string> b = new List<string>(a);
            doc.Posts = b;
            var result = DbSet<User>.collections[db_name].ReplaceOne(filter, doc);

            return result.MatchedCount == 1;
        }
        public static async Task<bool> DeletePostsAsync(this DbSet<User> dbSet, string id,
                                                       string[] elements)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<User>.collections.ContainsKey(db_name) == false)
            {


                return false;
            }
            var filter = new BsonDocument("_id", id);
            var doc = await DbSet<User>.collections[db_name].Find(filter).FirstOrDefaultAsync();
            if (doc.Posts == null)
            {
                SortedSet<string> q = new SortedSet<string>(elements);
                List<string> y = new List<string>(q);

                doc.Posts = y;
                await DbSet<User>.collections[db_name].ReplaceOneAsync(filter, doc);
                return true;
            }
            var main = doc.Posts;
            SortedSet<string> a = new SortedSet<string>();
            foreach (var o in main)
            {
                a.Add(o);
            }
            foreach (var o in elements)
            {
                a.Remove(o);
            }
            // var asd = new BsonDocument("Likes",new BsonArray(a.ToArray()));
            List<string> b = new List<string>(a);
            doc.Posts = b;
            var result = await DbSet<User>.collections[db_name].ReplaceOneAsync(filter, doc);

            return result.MatchedCount == 1;
        }
        public static bool DeletePosts(this DbSet<User> dbSet, string id,
                                                      string[] elements)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<User>.collections.ContainsKey(db_name) == false)
            {


                return false;
            }
            var filter = new BsonDocument("_id", id);
            var doc = DbSet<User>.collections[db_name].Find(filter).FirstOrDefault();
            if (doc.Posts == null)
            {
                SortedSet<string> q = new SortedSet<string>(elements);
                List<string> y = new List<string>(q);
                doc.Posts = y;
                DbSet<User>.collections[db_name].ReplaceOne(filter, doc);
                return true;
            }
            var main = doc.Posts;
            SortedSet<string> a = new SortedSet<string>();
            foreach (var o in main)
            {
                a.Add(o);
            }
            foreach (var o in elements)
            {
                a.Remove(o);
            }
            // var asd = new BsonDocument("Likes",new BsonArray(a.ToArray()));
            List<string> b = new List<string>(a);
            doc.Posts = b;
            var result = DbSet<User>.collections[db_name].ReplaceOne(filter, doc);

            return result.MatchedCount == 1;
        }
        #endregion

        #region Photo
        public static async Task<bool> AddPhotoAsync(this DbSet<User> dbSet, string id,
                                                     string[] elements)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<User>.collections.ContainsKey(db_name) == false)
            {


                return false;
            }
            var filter = new BsonDocument("_id", id);
            var doc = await DbSet<User>.collections[db_name].Find(filter).FirstOrDefaultAsync();
            if (doc.Photos == null)
            {
                SortedSet<string> q = new SortedSet<string>(elements);
                List<string> y = new List<string>(q);
            
                doc.Photos=y;
                await DbSet<User>.collections[db_name].ReplaceOneAsync(filter, doc);
                return true;
            }
            var main = doc.Photos;
            SortedSet<string> a = new SortedSet<string>();
            foreach (var o in main)
            {
                a.Add(o);
            }
            foreach (var o in elements)
            {
                a.Add(o);
            }
            // var asd = new BsonDocument("Likes",new BsonArray(a.ToArray()));
            List<string> b = new List<string>(a);
            doc.Photos = b;
            var result = await DbSet<User>.collections[db_name].ReplaceOneAsync(filter, doc);

            return result.MatchedCount == 1;
        }

        public static bool AddPhoto(this DbSet<User> dbSet, string id,
                                                      string[] elements)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<User>.collections.ContainsKey(db_name) == false)
            {


                return false;
            }
            var filter = new BsonDocument("_id", id);
            var doc = DbSet<User>.collections[db_name].Find(filter).FirstOrDefault();
            if (doc.Photos == null)
            {
                SortedSet<string> q = new SortedSet<string>(elements);
                List<string> y = new List<string>(q);
                doc.Photos=y;
                DbSet<User>.collections[db_name].ReplaceOne(filter, doc);
                return true;
            }
            var main = doc.Photos;
            SortedSet<string> a = new SortedSet<string>();
            foreach (var o in main)
            {
                a.Add(o);
            }
            foreach (var o in elements)
            {
                a.Add(o);
            }
            // var asd = new BsonDocument("Likes",new BsonArray(a.ToArray()));
            List<string> b = new List<string>(a);
            doc.Photos = b;
            var result = DbSet<User>.collections[db_name].ReplaceOne(filter, doc);

            return result.MatchedCount == 1;
        }
        public static async Task<bool> DeletePhotoAsync(this DbSet<User> dbSet, string id,
                                                       string[] elements)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<User>.collections.ContainsKey(db_name) == false)
            {


                return false;
            }
            var filter = new BsonDocument("_id", id);
            var doc = await DbSet<User>.collections[db_name].Find(filter).FirstOrDefaultAsync();
            if (doc.Photos == null)
            {
                SortedSet<string> q = new SortedSet<string>(elements);
                List<string> y = new List<string>(q);

                doc.Photos = y;
                await DbSet<User>.collections[db_name].ReplaceOneAsync(filter, doc);
                return true;
            }
            var main = doc.Photos;
            SortedSet<string> a = new SortedSet<string>();
            foreach (var o in main)
            {
                a.Add(o);
            }
            foreach (var o in elements)
            {
                a.Remove(o);
            }
            // var asd = new BsonDocument("Likes",new BsonArray(a.ToArray()));
            List<string> b = new List<string>(a);
            doc.Photos = b;
            var result = await DbSet<User>.collections[db_name].ReplaceOneAsync(filter, doc);

            return result.MatchedCount == 1;
        }
        public static bool DeletePhoto(this DbSet<User> dbSet, string id,
                                                      string[] elements)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<User>.collections.ContainsKey(db_name) == false)
            {


                return false;
            }
            var filter = new BsonDocument("_id", id);
            var doc = DbSet<User>.collections[db_name].Find(filter).FirstOrDefault();
            if (doc.Photos == null)
            {
                SortedSet<string> q = new SortedSet<string>(elements);
                List<string> y = new List<string>(q);
                doc.Photos = y;
                DbSet<User>.collections[db_name].ReplaceOne(filter, doc);
                return true;
            }
            var main = doc.Photos;
            SortedSet<string> a = new SortedSet<string>();
            foreach (var o in main)
            {
                a.Add(o);
            }
            foreach (var o in elements)
            {
                a.Remove(o);
            }
            // var asd = new BsonDocument("Likes",new BsonArray(a.ToArray()));
            List<string> b = new List<string>(a);
            doc.Photos = b;
            var result = DbSet<User>.collections[db_name].ReplaceOne(filter, doc);

            return result.MatchedCount == 1;
        }
        #endregion

        #region ActiveTasks
        public static async Task<bool> AddActiveTasksAsync(this DbSet<User> dbSet, string id,
                                                     string[] elements)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<User>.collections.ContainsKey(db_name) == false)
            {


                return false;
            }
            var filter = new BsonDocument("_id", id);
            var doc = await DbSet<User>.collections[db_name].Find(filter).FirstOrDefaultAsync();
            if (doc.ActiveTask == null)
            {
                SortedSet<string> q = new SortedSet<string>(elements);
                List<string> y = new List<string>(q);
                doc.ActiveTask = y;
                await DbSet<User>.collections[db_name].ReplaceOneAsync(filter, doc);
                return true;
            }
            var main = doc.ActiveTask;
            SortedSet<string> a = new SortedSet<string>();
            foreach (var o in main)
            {
                a.Add(o);
            }
            foreach (var o in elements)
            {
                a.Add(o);
            }
            // var asd = new BsonDocument("Likes",new BsonArray(a.ToArray()));
            List<string> b = new List<string>(a);
            doc.ActiveTask = b;
            var result = await DbSet<User>.collections[db_name].ReplaceOneAsync(filter, doc);

            return result.MatchedCount == 1;
        }

        public static bool AddActiveTasks(this DbSet<User> dbSet, string id,
                                                      string[] elements)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<User>.collections.ContainsKey(db_name) == false)
            {


                return false;
            }
            var filter = new BsonDocument("_id", id);
            var doc = DbSet<User>.collections[db_name].Find(filter).FirstOrDefault();
            if (doc.ActiveTask == null)
            {
                SortedSet<string> q = new SortedSet<string>(elements);
                List<string> y = new List<string>(q);
                doc.ActiveTask = y;
                DbSet<User>.collections[db_name].ReplaceOne(filter, doc);
                return true;
            }
            var main = doc.ActiveTask;
            SortedSet<string> a = new SortedSet<string>();
            foreach (var o in main)
            {
                a.Add(o);
            }
            foreach (var o in elements)
            {
                a.Add(o);
            }
            // var asd = new BsonDocument("Likes",new BsonArray(a.ToArray()));
            List<string> b = new List<string>(a);
            doc.ActiveTask = b;
            var result = DbSet<User>.collections[db_name].ReplaceOne(filter, doc);

            return result.MatchedCount == 1;
        }
        public static async Task<bool> DeleteActiveTasksAsync(this DbSet<User> dbSet, string id,
                                                       string[] elements)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<User>.collections.ContainsKey(db_name) == false)
            {


                return false;
            }
            var filter = new BsonDocument("_id", id);
            var doc = await DbSet<User>.collections[db_name].Find(filter).FirstOrDefaultAsync();
            if (doc.ActiveTask == null)
            {
                SortedSet<string> q = new SortedSet<string>(elements);
                List<string> y = new List<string>(q);
                doc.ActiveTask = y;
                await DbSet<User>.collections[db_name].ReplaceOneAsync(filter, doc);
                return true;
            }
            var main = doc.ActiveTask;
            SortedSet<string> a = new SortedSet<string>();
            foreach (var o in main)
            {
                a.Add(o);
            }
            foreach (var o in elements)
            {
                a.Remove(o);
            }
            // var asd = new BsonDocument("Likes",new BsonArray(a.ToArray()));
            List<string> b = new List<string>(a);
            doc.ActiveTask = b;
            var result = await DbSet<User>.collections[db_name].ReplaceOneAsync(filter, doc);

            return result.MatchedCount == 1;
        }
        public static bool DeleteActiveTasks(this DbSet<User> dbSet, string id,
                                                      string[] elements)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<User>.collections.ContainsKey(db_name) == false)
            {


                return false;
            }
            var filter = new BsonDocument("_id", id);
            var doc = DbSet<User>.collections[db_name].Find(filter).FirstOrDefault();
            if (doc.ActiveTask == null)
            {
                SortedSet<string> q = new SortedSet<string>(elements);
                List<string> y = new List<string>(q);
                doc.ActiveTask = y;
                DbSet<User>.collections[db_name].ReplaceOne(filter, doc);
                return true;
            }
            var main = doc.ActiveTask;
            SortedSet<string> a = new SortedSet<string>();
            foreach (var o in main)
            {
                a.Add(o);
            }
            foreach (var o in elements)
            {
                a.Remove(o);
            }
            // var asd = new BsonDocument("Likes",new BsonArray(a.ToArray()));
            List<string> b = new List<string>(a);
            doc.ActiveTask = b;
            var result = DbSet<User>.collections[db_name].ReplaceOne(filter, doc);

            return result.MatchedCount == 1;
        }
        #endregion

        #region CompletedTasks

        public static async Task<bool> AddCompletedTasksAsync(this DbSet<User> dbSet, string id,
                                                      string[] elements)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<User>.collections.ContainsKey(db_name) == false)
            {


                return false;
            }
            var filter = new BsonDocument("_id", id);
            var doc = await DbSet<User>.collections[db_name].Find(filter).FirstOrDefaultAsync();
            if (doc.CompletedTasks == null)
            {
                SortedSet<string> q = new SortedSet<string>(elements);
                List<string> y = new List<string>(q);
                doc.CompletedTasks=y;
                await DbSet<User>.collections[db_name].ReplaceOneAsync(filter, doc);
                return true;
            }
            var main = doc.CompletedTasks;
            SortedSet<string> a = new SortedSet<string>();
            foreach (var o in main)
            {
                a.Add(o);
            }
            foreach (var o in elements)
            {
                a.Add(o);
            }
            // var asd = new BsonDocument("Likes",new BsonArray(a.ToArray()));
            List<string> b = new List<string>(a);
            doc.CompletedTasks = b;
            var result = await DbSet<User>.collections[db_name].ReplaceOneAsync(filter, doc);

            return result.MatchedCount == 1;
        }

        public static bool AddCompletedTasks(this DbSet<User> dbSet, string id,
                                                      string[] elements)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<User>.collections.ContainsKey(db_name) == false)
            {


                return false;
            }
            var filter = new BsonDocument("_id", id);
            var doc = DbSet<User>.collections[db_name].Find(filter).FirstOrDefault();
            if (doc.CompletedTasks == null)
            {
                SortedSet<string> q = new SortedSet<string>(elements);
                List<string> y = new List<string>(q);
                doc.CompletedTasks = y;
                DbSet<User>.collections[db_name].ReplaceOne(filter, doc);
                return true;
            }
            var main = doc.CompletedTasks;
            SortedSet<string> a = new SortedSet<string>();
            foreach (var o in main)
            {
                a.Add(o);
            }
            foreach (var o in elements)
            {
                a.Add(o);
            }
            // var asd = new BsonDocument("Likes",new BsonArray(a.ToArray()));
            List<string> b = new List<string>(a);
            doc.CompletedTasks = b;
            var result = DbSet<User>.collections[db_name].ReplaceOne(filter, doc);

            return result.MatchedCount == 1;
        }
        public static async Task<bool> DeleteCompletedTasksAsync(this DbSet<User> dbSet, string id,
                                                       string[] elements)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<User>.collections.ContainsKey(db_name) == false)
            {


                return false;
            }
            var filter = new BsonDocument("_id", id);
            var doc = await DbSet<User>.collections[db_name].Find(filter).FirstOrDefaultAsync();
            if (doc.CompletedTasks == null)
            {
                SortedSet<string> q = new SortedSet<string>(elements);
                List<string> y = new List<string>(q);
                doc.CompletedTasks = y;
                await DbSet<User>.collections[db_name].ReplaceOneAsync(filter, doc);
                return true;
            }
            var main = doc.CompletedTasks;
            SortedSet<string> a = new SortedSet<string>();
            foreach (var o in main)
            {
                a.Add(o);
            }
            foreach (var o in elements)
            {
                a.Remove(o);
            }
            // var asd = new BsonDocument("Likes",new BsonArray(a.ToArray()));
            List<string> b = new List<string>(a);
            doc.CompletedTasks = b;
            var result = await DbSet<User>.collections[db_name].ReplaceOneAsync(filter, doc);

            return result.MatchedCount == 1;
        }
        public static bool DeleteCompletedTasks(this DbSet<User> dbSet, string id,
                                                      string[] elements)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<User>.collections.ContainsKey(db_name) == false)
            {


                return false;
            }
            var filter = new BsonDocument("_id", id);
            var doc = DbSet<User>.collections[db_name].Find(filter).FirstOrDefault();
            if (doc.CompletedTasks == null)
            {
                SortedSet<string> q = new SortedSet<string>(elements);
                List<string> y = new List<string>(q);
                doc.CompletedTasks = y;
                DbSet<User>.collections[db_name].ReplaceOne(filter, doc);
                return true;
            }
            var main = doc.CompletedTasks;
            SortedSet<string> a = new SortedSet<string>();
            foreach (var o in main)
            {
                a.Add(o);
            }
            foreach (var o in elements)
            {
                a.Remove(o);
            }
            // var asd = new BsonDocument("Likes",new BsonArray(a.ToArray()));
            List<string> b = new List<string>(a);
            doc.CompletedTasks = b;
            var result = DbSet<User>.collections[db_name].ReplaceOne(filter, doc);

            return result.MatchedCount == 1;
        }
        #endregion

        #region Subscriptions
        public static async Task<bool> AddSubscriptionsAsync(this DbSet<User> dbSet, string id,
                                                       string[] elements)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<User>.collections.ContainsKey(db_name) == false)
            {


                return false;
            }
            var filter = new BsonDocument("_id", id);
            var doc = await DbSet<User>.collections[db_name].Find(filter).FirstOrDefaultAsync();
            if (doc.Subscriptions == null)
            {
                SortedSet<string> q = new SortedSet<string>(elements);
                List<string> y = new List<string>(q);
                doc.Subscriptions = y;
                await DbSet<User>.collections[db_name].ReplaceOneAsync(filter, doc);
                return true;
            }
            var main = doc.Subscriptions;
            SortedSet<string> a = new SortedSet<string>();
            foreach (var o in main)
            {
                a.Add(o);
            }
            foreach (var o in elements)
            {
                a.Add(o);
            }
            // var asd = new BsonDocument("Likes",new BsonArray(a.ToArray()));
            List<string> b = new List<string>(a);
            doc.Subscriptions = b;
            var result = await DbSet<User>.collections[db_name].ReplaceOneAsync(filter, doc);

            return result.MatchedCount == 1;
        }

        public static bool AddSubscriptions(this DbSet<User> dbSet, string id,
                                                      string[] elements)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<User>.collections.ContainsKey(db_name) == false)
            {


                return false;
            }
            var filter = new BsonDocument("_id", id);
            var doc = DbSet<User>.collections[db_name].Find(filter).FirstOrDefault();
            if (doc.Subscriptions == null)
            {
                SortedSet<string> q = new SortedSet<string>(elements);
                List<string> y = new List<string>(q);
                doc.Subscriptions = y;
                DbSet<User>.collections[db_name].ReplaceOne(filter, doc);
                return true;
            }
            var main = doc.Subscriptions;
            SortedSet<string> a = new SortedSet<string>();
            foreach (var o in main)
            {
                a.Add(o);
            }
            foreach (var o in elements)
            {
                a.Add(o);
            }
            // var asd = new BsonDocument("Likes",new BsonArray(a.ToArray()));
            List<string> b = new List<string>(a);
            doc.Subscriptions = b;
            var result = DbSet<User>.collections[db_name].ReplaceOne(filter, doc);

            return result.MatchedCount == 1;
        }
        public static async Task<bool> DeleteSubscriptionsAsync(this DbSet<User> dbSet, string id,
                                                       string[] elements)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<User>.collections.ContainsKey(db_name) == false)
            {


                return false;
            }
            var filter = new BsonDocument("_id", id);
            var doc = await DbSet<User>.collections[db_name].Find(filter).FirstOrDefaultAsync();
            if (doc.Subscriptions == null)
            {
                SortedSet<string> q = new SortedSet<string>(elements);
                List<string> y = new List<string>(q);
                doc.Subscriptions = y;
                await DbSet<User>.collections[db_name].ReplaceOneAsync(filter, doc);
                return true;
            }
            var main = doc.Subscriptions;
            SortedSet<string> a = new SortedSet<string>();
            foreach (var o in main)
            {
                a.Add(o);
            }
            foreach (var o in elements)
            {
                a.Remove(o);
            }
            // var asd = new BsonDocument("Likes",new BsonArray(a.ToArray()));
            List<string> b = new List<string>(a);
            doc.Subscriptions = b;
            var result = await DbSet<User>.collections[db_name].ReplaceOneAsync(filter, doc);

            return result.MatchedCount == 1;
        }
        public static bool DeleteSubscriptions(this DbSet<User> dbSet, string id,
                                                      string[] elements)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<User>.collections.ContainsKey(db_name) == false)
            {


                return false;
            }
            var filter = new BsonDocument("_id", id);
            var doc = DbSet<User>.collections[db_name].Find(filter).FirstOrDefault();
            if (doc.Subscriptions == null)
            {
                SortedSet<string> q = new SortedSet<string>(elements);
                List<string> y = new List<string>(q);
                doc.Subscriptions = y;
                DbSet<User>.collections[db_name].ReplaceOne(filter, doc);
                return true;
            }
            var main = doc.Subscriptions;
            SortedSet<string> a = new SortedSet<string>();
            foreach (var o in main)
            {
                a.Add(o);
            }
            foreach (var o in elements)
            {
                a.Remove(o);
            }
            // var asd = new BsonDocument("Likes",new BsonArray(a.ToArray()));
            List<string> b = new List<string>(a);
            doc.Subscriptions = b;
            var result = DbSet<User>.collections[db_name].ReplaceOne(filter, doc);

            return result.MatchedCount == 1;
        }
        #endregion

        #region Plans
        public static async Task<bool> AddPlansAsync(this DbSet<User> dbSet, string id,
                                                      Plan[] elements)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<User>.collections.ContainsKey(db_name) == false)
            {


                return false;
            }
            var filter = new BsonDocument("_id", id);
            var doc = await DbSet<User>.collections[db_name].Find(filter).FirstOrDefaultAsync();
            if (doc.Plans == null)
            {
                SortedSet<Plan> q = new SortedSet<Plan>(elements);
                List<Plan> y = new List<Plan>(q);
                doc.Plans = y;
                await DbSet<User>.collections[db_name].ReplaceOneAsync(filter, doc);
                return true;
            }
            var main = doc.Plans;
            SortedSet<Plan> a = new SortedSet<Plan>();
            foreach (var o in main)
            {
                a.Add(o);
            }
            foreach (var o in elements)
            {
                a.Add(o);
            }
            // var asd = new BsonDocument("Likes",new BsonArray(a.ToArray()));
            List<Plan> b = new List<Plan>(a);
            doc.Plans = b;
            var result = await DbSet<User>.collections[db_name].ReplaceOneAsync(filter, doc);

            return result.MatchedCount == 1;
        }

        public static bool AddPlans(this DbSet<User> dbSet, string id,
                                                      Plan[] elements)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<User>.collections.ContainsKey(db_name) == false)
            {


                return false;
            }
            var filter = new BsonDocument("_id", id);
            var doc = DbSet<User>.collections[db_name].Find(filter).FirstOrDefault();
            if (doc.Plans == null)
            {
                SortedSet<Plan> q = new SortedSet<Plan>(elements);
                List<Plan> y = new List<Plan>(q);
                doc.Plans = y;
                DbSet<User>.collections[db_name].ReplaceOne(filter, doc);
                return true;
            }
            var main = doc.Plans;
            SortedSet<Plan> a = new SortedSet<Plan>();
            foreach (var o in main)
            {
                a.Add(o);
            }
            foreach (var o in elements)
            {
                a.Add(o);
            }
            // var asd = new BsonDocument("Likes",new BsonArray(a.ToArray()));
            List<Plan> b = new List<Plan>(a);
            doc.Plans = b;
            var result = DbSet<User>.collections[db_name].ReplaceOne(filter, doc);

            return result.MatchedCount == 1;
        }
        public static async Task<bool> DeletePlansAsync(this DbSet<User> dbSet, string id,
                                                       Plan[] elements)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<User>.collections.ContainsKey(db_name) == false)
            {


                return false;
            }
            var filter = new BsonDocument("_id", id);
            var doc = await DbSet<User>.collections[db_name].Find(filter).FirstOrDefaultAsync();
            if (doc.Plans == null)
            {
                SortedSet<Plan> q = new SortedSet<Plan>(elements);
                List<Plan> y = new List<Plan>(q);
                doc.Plans = y;
                await DbSet<User>.collections[db_name].ReplaceOneAsync(filter, doc);
                return true;
            }
            var main = doc.Plans;
            SortedSet<Plan> a = new SortedSet<Plan>();
            foreach (var o in main)
            {
                a.Add(o);
            }
            foreach (var o in elements)
            {
                a.Remove(o);
            }
            // var asd = new BsonDocument("Likes",new BsonArray(a.ToArray()));
            List<Plan> b = new List<Plan>(a);
            doc.Plans = b;
            var result = await DbSet<User>.collections[db_name].ReplaceOneAsync(filter, doc);

            return result.MatchedCount == 1;
        }
        public static bool DeletePlans(this DbSet<User> dbSet, string id,
                                                      Plan[] elements)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<User>.collections.ContainsKey(db_name) == false)
            {


                return false;
            }
            var filter = new BsonDocument("_id", id);
            var doc = DbSet<User>.collections[db_name].Find(filter).FirstOrDefault();
            if (doc.Plans == null)
            {
                SortedSet<Plan> q = new SortedSet<Plan>(elements);
                List<Plan> y = new List<Plan>(q);
                doc.Plans = y;
                DbSet<User>.collections[db_name].ReplaceOne(filter, doc);
                return true;
            }
            var main = doc.Plans;
            SortedSet<Plan> a = new SortedSet<Plan>();
            foreach (var o in main)
            {
                a.Add(o);
            }
            foreach (var o in elements)
            {
                a.Remove(o);
            }
            // var asd = new BsonDocument("Likes",new BsonArray(a.ToArray()));
            List<Plan> b = new List<Plan>(a);
            doc.Plans = b;
            var result = DbSet<User>.collections[db_name].ReplaceOne(filter, doc);
            
            return result.MatchedCount == 1;
        }
        #endregion
        #region Notices
        public static async Task<bool> AddNoticesAsync(this DbSet<User> dbSet, string id,
                                                      Notice[] elements)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<User>.collections.ContainsKey(db_name) == false)
            {


                return false;
            }
            var filter = new BsonDocument("_id", id);
            var doc = await DbSet<User>.collections[db_name].Find(filter).FirstOrDefaultAsync();
            if (doc.Notices == null)
            {
                SortedSet<Notice> q = new SortedSet<Notice>(elements);
                List<Notice> y = new List<Notice>(q);
                doc.Notices = y;
                await DbSet<User>.collections[db_name].ReplaceOneAsync(filter, doc);
                return true;
            }
            var main = doc.Notices;
            SortedSet<Notice> a = new SortedSet<Notice>();
            foreach (var o in main)
            {
                a.Add(o);
            }
            foreach (var o in elements)
            {
                a.Add(o);
            }
            // var asd = new BsonDocument("Likes",new BsonArray(a.ToArray()));
            List<Notice> b = new List<Notice>(a);
            doc.Notices = b;
            var result = await DbSet<User>.collections[db_name].ReplaceOneAsync(filter, doc);

            return result.MatchedCount == 1;
        }

        public static bool AddNotices(this DbSet<User> dbSet, string id,
                                                      Notice[] elements)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<User>.collections.ContainsKey(db_name) == false)
            {


                return false;
            }
            var filter = new BsonDocument("_id", id);
            var doc = DbSet<User>.collections[db_name].Find(filter).FirstOrDefault();
            if (doc.Notices == null)
            {
                SortedSet<Notice> q = new SortedSet<Notice>(elements);
                List<Notice> y = new List<Notice>(q);
                doc.Notices = y;
                DbSet<User>.collections[db_name].ReplaceOne(filter, doc);
                return true;
            }
            var main = doc.Notices;
            SortedSet<Notice> a = new SortedSet<Notice>();
            foreach (var o in main)
            {
                a.Add(o);
            }
            foreach (var o in elements)
            {
                a.Add(o);
            }
            // var asd = new BsonDocument("Likes",new BsonArray(a.ToArray()));
            List<Notice> b = new List<Notice>(a);
            doc.Notices = b;
            var result = DbSet<User>.collections[db_name].ReplaceOne(filter, doc);

            return result.MatchedCount == 1;
        }
        public static async Task<bool> DeleteNoticesAsync(this DbSet<User> dbSet, string id,
                                                       Notice[] elements)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<User>.collections.ContainsKey(db_name) == false)
            {


                return false;
            }
            var filter = new BsonDocument("_id", id);
            var doc = await DbSet<User>.collections[db_name].Find(filter).FirstOrDefaultAsync();
            if (doc.Notices == null)
            {
                SortedSet<Notice> q = new SortedSet<Notice>(elements);
                List<Notice> y = new List<Notice>(q);
                doc.Notices = y;
                await DbSet<User>.collections[db_name].ReplaceOneAsync(filter, doc);
                return true;
            }
            var main = doc.Notices;
            SortedSet<Notice> a = new SortedSet<Notice>();
            foreach (var o in main)
            {
                a.Add(o);
            }
            foreach (var o in elements)
            {
                a.Remove(o);
            }
            // var asd = new BsonDocument("Likes",new BsonArray(a.ToArray()));
            List<Notice> b = new List<Notice>(a);
            doc.Notices = b;
            var result = await DbSet<User>.collections[db_name].ReplaceOneAsync(filter, doc);

            return result.MatchedCount == 1;
        }
        public static bool DeleteNotices(this DbSet<User> dbSet, string id,
                                                      Notice[] elements)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<User>.collections.ContainsKey(db_name) == false)
            {


                return false;
            }
            var filter = new BsonDocument("_id", id);
            var doc = DbSet<User>.collections[db_name].Find(filter).FirstOrDefault();
            if (doc.Notices == null)
            {
                SortedSet<Notice> q = new SortedSet<Notice>(elements);
                List<Notice> y = new List<Notice>(q);
                doc.Notices = y;
                DbSet<User>.collections[db_name].ReplaceOne(filter, doc);
                return true;
            }
            var main = doc.Notices;
            SortedSet<Notice> a = new SortedSet<Notice>();
            foreach (var o in main)
            {
                a.Add(o);
            }
            foreach (var o in elements)
            {
                a.Remove(o);
            }
            // var asd = new BsonDocument("Likes",new BsonArray(a.ToArray()));
            List<Notice> b = new List<Notice>(a);
            doc.Notices = b;
            var result = DbSet<User>.collections[db_name].ReplaceOne(filter, doc);

            return result.MatchedCount == 1;
        }
        #endregion
        #endregion

        #region DELETE

        /// <summary>
        ///   Видаляє документ з таким Id
        /// </summary>
        public static async void DeleteAsync(this DbSet<User> dbSet, string id)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<Photo>.collections.ContainsKey(db_name) == true)
            {

                var filter = new BsonDocument("_id", id);
                await DbSet<User>.collections[db_name].DeleteOneAsync(filter);
            }
        }
        public static void Delete(this DbSet<User> dbSet, string id)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<Photo>.collections.ContainsKey(db_name) == true)
            {

                var filter = new BsonDocument("_id", id);
                DbSet<User>.collections[db_name].DeleteOne(filter);
            }
        }
        #endregion

    }
}
