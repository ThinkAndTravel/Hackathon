using MongoDB.Bson;
using MongoDB.Driver;
using MongoManager.CollectionModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MongoManager.Context
{
    public static class MailSearch
    {
        /// <summary>
        /// Пошук id юзера за даним 
        /// </summary>
        public static async Task<string> GetMailIdAsync(this DbSet<User> dbSet, string Email)
        {
            string id = null;
            foreach (var collection in DbSet<User>.collections)
            {
                var c = await collection.Value
                    .Find(x => x.Main.Email == Email)
                    .FirstOrDefaultAsync();                                     
                if (c != null)
                {
                    id = c._id.ToString();
                    break;
                }
            }
            return id;
        }

    }
}
