using MongoDB.Bson;
using MongoManager.CollectionModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace MongoManager
{
    /// <summary>
    /// Об'єкт через який відбувається обмін даними з БД
    /// </summary>
    public class DataManager
    {
        public static DbSet<User> Users { get; set; }
        public static DbSet<Post> Posts { get; set; }
        public static DbSet<Task> Tasks { get; set; }
        public static DbSet<Photo> Photos { get; set; }
        public static DbSet<Tour> Tours { get; set; }

        public DataManager()
        {
            Users = new DbSet<User>("Users");
            Posts = new DbSet<Post>("Posts");
            Tasks = new DbSet<Task>("Tasks");
            Photos = new DbSet<Photo>("Photos");
            Tours = new DbSet<Tour>("Tours");
        }
    }
}
