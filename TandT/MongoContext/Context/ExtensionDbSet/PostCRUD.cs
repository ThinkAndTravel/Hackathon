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
    public static class PostCRUD
    {


        #region CREATE

        /// <summary>
        /// Ім'я ДБ в яку ми записуємо всіх, хто щойно додався
        /// </summary>
        /// <remarks>
        /// Повинно бути різним для кожного сервера 
        /// </remarks>
        public static string CurrentDB { get; set; } = "p1";

        /// <summary>
        /// Додає новий документ в БД 
        /// </summary>
        public static bool Create(this DbSet<Post> dbSet, ref Post post)
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
            post._id = CurrentDB + ":" + post._id;
            DbSet<Post>.collections[CurrentDB].InsertOne(post);
            // DbSet<User>.collections[CurrentDB].
            return true;
        }

        #endregion

        #region READ
        /// <summary>
        /// Returns all posts
        /// </summary>
        /// <param name="dbSet"></param>
        /// <returns></returns>
        public static async Task<List<Post>> ReadAllPostsAsync(this DbSet<Post> dbSet)
        {
            List<String> dbs = new List<string> {"p1", "p2", "p3", "p4" };
            var filter = new BsonDocument();
            List<Post> total = new List<Post>();
            foreach (var db in dbs)
            {
                var list = await DbSet<Post>.collections[db].Find(filter).ToListAsync();
                total.AddRange(list);
            }
            return total;
        }

        /// <summary>
        /// Зчитує з словника колекцій лише головну частину документа за таким id 
        /// </summary>
        public static async Task<MainPost> ReadMainAsync(this DbSet<Post> dbSet, string id)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<Post>.collections.ContainsKey(db_name) == false)
            {
                return null;
            }
            var filter = new BsonDocument("_id", id);
            var people = await DbSet<Post>.collections[db_name].Find(filter).FirstOrDefaultAsync();
            return people.Main;
        }
        public static async Task<Post> ReadFullAsync(this DbSet<Post> dbSet, string id)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<Post>.collections.ContainsKey(db_name) == false)
            {
                return null;
            }
            var filter = new BsonDocument("_id", id);
            var post = await DbSet<Post>.collections[db_name].Find(filter).FirstOrDefaultAsync();
            return post;

        }

        /// <summary>
        /// Зчитує з словника колекцій лише 
        /// вказані в масиві атрибути документу за таким id 
        /// </summary>
        /// <param name="attributes"> 
        /// Масив імен атрибутів
        ///     <example>["Posts","Friends"]</example>
        /// </param>
        public static async Task<BsonDocument> ReadAsync(this DbSet<Post> dbSet,
            string id, string[] attributes)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<Post>.collections.ContainsKey(db_name) == false)
            {
                return null;
            }
            var doc = new BsonDocument();
            var filter = new BsonDocument("_id", id);
            var post = await DbSet<Post>.collections[db_name].Find(filter).FirstOrDefaultAsync();
            foreach (var o in attributes)
            {
                doc.Add(o, post.ToBsonDocument()[o].ToBsonDocument());
            }
            return doc;
        }
        public static MainPost ReadMain(this DbSet<Post> dbSet, string id)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<Post>.collections.ContainsKey(db_name) == false)
            {
                return null;
            }
            var filter = new BsonDocument("_id", id);
            var people = DbSet<Post>.collections[db_name].Find(filter).FirstOrDefault();
            return people.Main;
        }
        public static Post ReadFull(this DbSet<Post> dbSet, string id)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<Post>.collections.ContainsKey(db_name) == false)
            {
                return null;
            }
            var filter = new BsonDocument("_id", id);
            var post = DbSet<Post>.collections[db_name].Find(filter).FirstOrDefault();
            return post;

        }

        /// <summary>
        /// Зчитує з словника колекцій лише 
        /// вказані в масиві атрибути документу за таким id 
        /// </summary>
        /// <param name="attributes"> 
        /// Масив імен атрибутів
        ///     <example>["Posts","Friends"]</example>
        /// </param>
        public static BsonDocument Read(this DbSet<Post> dbSet,
            string id, string[] attributes)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<Post>.collections.ContainsKey(db_name) == false)
            {
                return null;
            }
            var doc = new BsonDocument();
            var filter = new BsonDocument("_id", id);
            var post = DbSet<Post>.collections[db_name].Find(filter).FirstOrDefault();
            foreach (var o in attributes)
            {
                doc.Add(o, post.ToBsonDocument()[o]);
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

        public static async Task<bool> UpdateMainAsync(this DbSet<Post> dbSet, string id,
                                                  MainPost post)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<Post>.collections.ContainsKey(db_name) == false)
            {
                return false;
            }
            var filter = new BsonDocument("_id", id);
            var doc = await DbSet<Post>.collections[db_name].Find(filter).FirstOrDefaultAsync();
            if (doc.Main == null)
            {
                doc.Main = post;
                await DbSet<Post>.collections[db_name].ReplaceOneAsync(filter, doc);
                return true;

            }
            doc.Main = post;
            var result = await DbSet<Post>.collections[db_name].ReplaceOneAsync(filter, doc);

            return result.MatchedCount == 1;
        }

        /// <summary>
        /// Заміняє значення main які задані в масиві на відповідні value
        /// </summary>
        /// <param name="atributes"> атрибути Main</param>
        /// <param name="values"> значення атрибутів(відповідно)</param>
        public static async Task<bool> UpdateAsync(this DbSet<Post> dbSet, string id,
                                                       Post post)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<Post>.collections.ContainsKey(db_name) == false)
            {
                return false;
            }
            var filter = new BsonDocument("_id",id);
            var result = await DbSet<Post>.collections[db_name].ReplaceOneAsync(filter, post);
            return result.MatchedCount == 1;

        }
        public static bool UpdateMain(this DbSet<Post> dbSet, string id,
                                                  MainPost post)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<Post>.collections.ContainsKey(db_name) == false)
            {
                return false;
            }
            var filter = new BsonDocument("_id", id);
            var doc = DbSet<Post>.collections[db_name].Find(filter).FirstOrDefault();
            if (doc.Main == null)
            {
                doc.Main = post;
                DbSet<Post>.collections[db_name].ReplaceOne(filter, doc);
                return true;
            }
            doc.Main = post;
            var result = DbSet<Post>.collections[db_name].ReplaceOne(filter, doc);

            return result.MatchedCount == 1;
        }

        /// <summary>
        /// Заміняє значення main які задані в масиві на відповідні value
        /// </summary>
        /// <param name="atributes"> атрибути Main</param>
        /// <param name="values"> значення атрибутів(відповідно)</param>
        public static bool Update(this DbSet<Post> dbSet, string id,
                                                       Post post)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<Post>.collections.ContainsKey(db_name) == false)
            {
                return false;
            }
            var filter = new BsonDocument("_id", id);
            var result = DbSet<Post>.collections[db_name].ReplaceOne(filter, post);
            return result.MatchedCount == 1;

        }
        #region UserPost
        /// <summary>
        /// Додаємо елементи в наш масив Друзів
        ///
        /// </summary>
        public static async Task<bool> AddUserPostAsync(this DbSet<Post> dbSet, string id,
                                                         string[] elements)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<Post>.collections.ContainsKey(db_name) == false)
            {


                return false;
            }
            var filter = new BsonDocument("_id", id);
            var doc = await DbSet<Post>.collections[db_name].Find(filter).FirstOrDefaultAsync();
            if (doc.UserPost == null)
            {
                SortedSet<string> q = new SortedSet<string>(elements);
                List<string> y = new List<string>(q);
                //var ad = new BsonDocument("UserPost", new BsonArray(y.ToArray()));
                doc.UserPost = y;
                await DbSet<Post>.collections[db_name].ReplaceOneAsync(filter, doc);
                return true;
            }
            var main = doc.UserPost;
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
            doc.UserPost = b;
            var result = await DbSet<Post>.collections[db_name].ReplaceOneAsync(filter, doc);

            return result.MatchedCount == 1;
        }

        public static bool AddUserPost(this DbSet<Post> dbSet, string id,
                                                      string[] elements)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<Post>.collections.ContainsKey(db_name) == false)
            {


                return false;
            }
            var filter = new BsonDocument("_id", id);
            var doc = DbSet<Post>.collections[db_name].Find(filter).FirstOrDefault();
            if (doc.UserPost == null)
            {
                SortedSet<string> q = new SortedSet<string>(elements);
                List<string> y = new List<string>(q);
                //var ad = new BsonDocument("UserPost", new BsonArray(y.ToArray()));
                doc.UserPost = y;
                DbSet<Post>.collections[db_name].ReplaceOne(filter, doc);
                return true;
            }
            var main = doc.UserPost;
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
            doc.UserPost = b;
            var result = DbSet<Post>.collections[db_name].ReplaceOne(filter, doc);

            return result.MatchedCount == 1;
        }
        public static async Task<bool> DeleteUserPostAsync(this DbSet<Post> dbSet, string id,
                                                       string[] elements)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<Post>.collections.ContainsKey(db_name) == false)
            {


                return false;
            }
            var filter = new BsonDocument("_id", id);
            var doc = await DbSet<Post>.collections[db_name].Find(filter).FirstOrDefaultAsync();
            if (doc.UserPost == null)
            {
                SortedSet<string> q = new SortedSet<string>(elements);
                List<string> y = new List<string>(q);
                //var ad = new BsonDocument("UserPost", new BsonArray(y.ToArray()));
                doc.UserPost = y;
                await DbSet<Post>.collections[db_name].ReplaceOneAsync(filter, doc);
                return true;
            }
            var main = doc.UserPost;
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
            doc.UserPost = b;
            var result = await DbSet<Post>.collections[db_name].ReplaceOneAsync(filter, doc);

            return result.MatchedCount == 1;
        }
        public static bool DeleteUserPost(this DbSet<Post> dbSet, string id,
                                                      string[] elements)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<Post>.collections.ContainsKey(db_name) == false)
            {


                return false;
            }
            var filter = new BsonDocument("_id", id);
            var doc = DbSet<Post>.collections[db_name].Find(filter).FirstOrDefault();
            if (doc.UserPost == null)
            {
                SortedSet<string> q = new SortedSet<string>(elements);
                List<string> y = new List<string>(q);
                //var ad = new BsonDocument("UserPost", new BsonArray(y.ToArray()));
                doc.UserPost = y;
                DbSet<Post>.collections[db_name].ReplaceOne(filter, doc);
                return true;
            }
            var main = doc.UserPost;
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
            doc.UserPost = b;
            var result = DbSet<Post>.collections[db_name].ReplaceOne(filter, doc);

            return result.MatchedCount == 1;
        }
        #endregion
        #region Likes
        /// <summary>
        /// Додаємо елементи в наш масив Друзів
        ///
        /// </summary>
        public static async Task<bool> AddLikesAsync(this DbSet<Post> dbSet, string id,
                                                       string[] elements)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<Post>.collections.ContainsKey(db_name) == false)
            {


                return false;
            }
            var filter = new BsonDocument("_id", id);
            var doc = await DbSet<Post>.collections[db_name].Find(filter).FirstOrDefaultAsync();
            if (doc.Likes == null)
            {
                SortedSet<string> q = new SortedSet<string>(elements);
                List<string> y = new List<string>(q);
                //var ad = new BsonDocument("Likes", new BsonArray(y.ToArray()));
                doc.Likes=y;
                await DbSet<Post>.collections[db_name].ReplaceOneAsync(filter, doc);
                return true;
            }
            var main = doc.Likes;
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
            doc.Likes = b;
            var result = await DbSet<Post>.collections[db_name].ReplaceOneAsync(filter, doc);

            return result.MatchedCount == 1;
        }

        public static bool AddLikes(this DbSet<Post> dbSet, string id,
                                                      string[] elements)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<Post>.collections.ContainsKey(db_name) == false)
            {


                return false;
            }
            var filter = new BsonDocument("_id", id);
            var doc = DbSet<Post>.collections[db_name].Find(filter).FirstOrDefault();
            if (doc.Likes == null)
            {
                SortedSet<string> q = new SortedSet<string>(elements);
                List<string> y = new List<string>(q);
                //var ad = new BsonDocument("Likes", new BsonArray(y.ToArray()));
                doc.Likes=y;
                DbSet<Post>.collections[db_name].ReplaceOne(filter, doc);
                return true;
            }
            var main = doc.Likes;
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
            doc.Likes = b;
            var result = DbSet<Post>.collections[db_name].ReplaceOne(filter, doc);

            return result.MatchedCount == 1;
        }
        public static async Task<bool> DeleteLikesAsync(this DbSet<Post> dbSet, string id,
                                                       string[] elements)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<Post>.collections.ContainsKey(db_name) == false)
            {


                return false;
            }
            var filter = new BsonDocument("_id", id);
            var doc = await DbSet<Post>.collections[db_name].Find(filter).FirstOrDefaultAsync();
            if (doc.Likes == null)
            {
                SortedSet<string> q = new SortedSet<string>(elements);
                List<string> y = new List<string>(q);
                //var ad = new BsonDocument("Likes", new BsonArray(y.ToArray()));
                doc.Likes = y;
                await DbSet<Post>.collections[db_name].ReplaceOneAsync(filter, doc);
                return true;
            }
            var main = doc.Likes;
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
            doc.Likes = b;
            var result = await DbSet<Post>.collections[db_name].ReplaceOneAsync(filter, doc);

            return result.MatchedCount == 1;
        }
        public static bool DeleteLikes(this DbSet<Post> dbSet, string id,
                                                      string[] elements)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<Post>.collections.ContainsKey(db_name) == false)
            {


                return false;
            }
            var filter = new BsonDocument("_id", id);
            var doc = DbSet<Post>.collections[db_name].Find(filter).FirstOrDefault();
            if (doc.Likes == null)
            {
                SortedSet<string> q = new SortedSet<string>(elements);
                List<string> y = new List<string>(q);
                //var ad = new BsonDocument("Likes", new BsonArray(y.ToArray()));
                doc.Likes = y;
                DbSet<Post>.collections[db_name].ReplaceOne(filter, doc);
                return true;
            }
            var main = doc.Likes;
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
            doc.Likes = b;
            var result = DbSet<Post>.collections[db_name].ReplaceOne(filter, doc);

            return result.MatchedCount == 1;
        }
        #endregion

        #region Comments
        public static async Task<bool> AddPostCommentsAsync(this DbSet<Post> dbSet, string id,
                                                      Comment[] elements)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<Post>.collections.ContainsKey(db_name) == false)
            {
                return false;
            }

            var filter = new BsonDocument("_id", id);
            var doc = await DbSet<Post>.collections[db_name].Find(filter).FirstOrDefaultAsync();
            if (doc.PostComments == null)
            {
                SortedSet<Comment> q = new SortedSet<Comment>(elements);
                List<Comment> y = new List<Comment>(q);
                //var ad = new BsonDocument("PostComments", new BsonArray(y.ToArray()));
                doc.PostComments = y;
                await DbSet<Post>.collections[db_name].ReplaceOneAsync(filter, doc);
                return true;
            }
            var main = doc.PostComments;
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
            doc.PostComments = b;
            var result = await DbSet<Post>.collections[db_name].ReplaceOneAsync(filter, doc);

            return result.MatchedCount == 1;
        }

        public static bool AddPostComments(this DbSet<Post> dbSet, string id,
                                                     Comment[] elements)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<Post>.collections.ContainsKey(db_name) == false)
            {
                return false;
            }

            var filter = new BsonDocument("_id", id);
            var doc = DbSet<Post>.collections[db_name].Find(filter).FirstOrDefault();
            if (doc.PostComments == null)
            {
                SortedSet<Comment> q = new SortedSet<Comment>(elements);
                List<Comment> y = new List<Comment>(q);
                doc.PostComments=y;
                DbSet<Post>.collections[db_name].ReplaceOne(filter, doc);
                return true;
            }
            var main = doc.PostComments;
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
            doc.PostComments = b;
            var result = DbSet<Post>.collections[db_name].ReplaceOne(filter, doc);

            return result.MatchedCount == 1;
        }
        public static async Task<bool> DeletePostCommentsAsync(this DbSet<Post> dbSet, string id,
                                                      Comment[] elements)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<Post>.collections.ContainsKey(db_name) == false)
            {
                return false;
            }

            var filter = new BsonDocument("_id", id);
            var doc = await DbSet<Post>.collections[db_name].Find(filter).FirstOrDefaultAsync();
            if (doc.PostComments == null)
            {
                SortedSet<Comment> q = new SortedSet<Comment>(elements);
                List<Comment> y = new List<Comment>(q);
                //var ad = new BsonDocument("PostComments", new BsonArray(y.ToArray()));
                doc.PostComments = y;
                await DbSet<Post>.collections[db_name].ReplaceOneAsync(filter, doc);
                return true;
            }
            var main = doc.PostComments;
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
            doc.PostComments = b;
            var result = await DbSet<Post>.collections[db_name].ReplaceOneAsync(filter, doc);

            return result.MatchedCount == 1;
        }
        public static bool DeletePostComments(this DbSet<Post> dbSet, string id,
                                                       Comment[] elements)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<Post>.collections.ContainsKey(db_name) == false)
            {
                return false;
            }

            var filter = new BsonDocument("_id", id);
            var doc = DbSet<Post>.collections[db_name].Find(filter).FirstOrDefault();
            if (doc.PostComments == null)
            {
                SortedSet<Comment> q = new SortedSet<Comment>(elements);
                List<Comment> y = new List<Comment>(q);
                doc.PostComments = y;
                DbSet<Post>.collections[db_name].ReplaceOne(filter, doc);
                return true;
            }
            var main = doc.PostComments;
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
            doc.PostComments = b;
            var result = DbSet<Post>.collections[db_name].ReplaceOne(filter, doc);

            return result.MatchedCount == 1;
        }
        #endregion

        #endregion

        #region DELETE

        /// <summary>
        ///   Видаляє документ з таким Id
        /// </summary>
        public static async void DeleteAsync(this DbSet<Post> dbSet, string id)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<Post>.collections.ContainsKey(db_name) == true)
            {

                var filter = new BsonDocument("_id", id);
                await DbSet<Post>.collections[db_name].DeleteOneAsync(filter);
            }
        }
        public static void Delete(this DbSet<Post> dbSet, string id)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));

            if (DbSet<Post>.collections.ContainsKey(db_name) == true)
            {

                var filter = new BsonDocument("_id", id);
                DbSet<Post>.collections[db_name].DeleteOne(filter);
            }
        }
        #endregion

    }
}

