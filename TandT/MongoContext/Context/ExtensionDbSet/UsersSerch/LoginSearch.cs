using MongoDB.Driver.Linq;
using MongoDB.Driver;
using MongoManager.CollectionModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MongoManager.Context
{
    public static class LoginSearch
    {
        /// <summary>
        /// Повертає хеш пароля за заданим id
        /// </summary>
        public static async Task<string> GetUserPas(this DbSet<User> dbSet, string id)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));
            return await (from cur in DbSet<User>.collections[db_name]
                   .AsQueryable<User>()
                        where cur._id == id
                        select cur.Main.HashPass)
                   .FirstAsync();
        }

        /// <summary>
        /// Перевіряє чи цей юзер активований
        /// </summary>
        /// <param name="dbSet"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async Task<bool> IsActiveted(this DbSet<User> dbSet, string id)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));
            return await (from cur in DbSet<User>.collections[db_name]
                   .AsQueryable<User>()
                          where cur._id == id
                          select cur.Activated)
                   .FirstAsync();
        }


        /// <summary>
        /// Перевіряє чи унікальний логін 
        /// </summary>
        public static async Task<bool> CheckLoginAsync(this DbSet<User> dbSet, string Login)
        {
            foreach (var collection in DbSet<User>.collections)
            {
                var c = await collection.Value
                    .Find(x => x._id == collection.Key + ":" + Login)
                    .FirstOrDefaultAsync();
                if (c != null)
                    return false;         
            }
            return true;
        }

        public static async Task<string> GetEmailAsync(this DbSet<User> dbSet, string id)
        {
            var db_name = id.Substring(0, id.IndexOf(':'));
            return await (from cur in DbSet<User>.collections[db_name]
                    .AsQueryable<User>()
                          where cur._id == id
                          select cur.Main.Email)
                    .FirstAsync();

        }
    }
}
