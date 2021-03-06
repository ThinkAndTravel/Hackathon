﻿using BLL.User.ViewModel;
using MongoDB.Driver;
using MongoManager;
using MongoManager.CollectionModels;
using MongoManager.Context;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.userLogic.extension
{
    public class TaskLogic
    {
        public static async Task<ViewTask> GetTaskById(string id)
        {
            foreach (var collection in DbSet<MongoManager.CollectionModels.Task>.collections)
            {
                var c = await collection.Value
                    .Find(x => x._id == id).CountAsync();
                    
                if(c > 0)
                {
                    var task = await  collection.Value
                    .Find(x => x._id == id).FirstAsync();
                    ViewTask view = new ViewTask();
                    view.About = task.Main.About;
                    view.Comments = task.Comments;
                    view.PicturesAbout = task.Main.PicturesAbout;
                    view.Tags = task.Main.Tags;
                    view._id = task._id;
                    view.Title = task.Main.Title;
                    for (int i = 1; i < 6; i++)
                    {
                        view.Ranks[i] = 5;// task.Ranks[i].Count;
                    }
                    return view;
                }
            }
            return null;
        }

        /// <summary>
        /// 2 - користувач пройшов це завдання
        /// 3 - завдання не з цієї локації
        /// 1 - завдання підходить користувачу
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="TaskId"></param>
        /// <param name="loc"></param>
        /// <returns></returns>
        public static int  CheckTask(string UserId, string TaskId, string City)
        {
            var userTasks = DataManager.Users.ReadFull(UserId).CompletedTasks;
            var taskCity = DataManager.Tasks.ReadFull(TaskId).City;
            if (City != taskCity) return 3;
            if (userTasks.Contains(TaskId) == true) return 2;
            return 1;
        }
        public static List<ViewTask> GetTaskForCity(string Id, string City)
        {
            List<ViewTask> list = new List<ViewTask>();
            foreach (var collection in DbSet<MongoManager.CollectionModels.Task>.collections)
            {
                var c = collection.Value
                    .Find(x => x.Main.Location.City == City)
                    .ToList();
                foreach (var task in c)
                {
                    ViewTask view = new ViewTask();
                    view.About = task.Main.About;
                    view.Comments = task.Comments;
                    view.PicturesAbout = task.Main.PicturesAbout;
                    view.Tags = task.Main.Tags;
                    view._id = task._id;
                    view.Title = task.Main.Title;
                    for (int i = 1; i < 6; i++)
                    {
                        view.Ranks[i] = (5 + (((int)DateTime.Now.Ticks << 32) ^ 11) % 5) % 5 + 1;// task.Ranks[i].Count;
                    }
                    list.Add(view);
                }
            }
            return list;
        }
        public static List<ViewTask> GetTaskForCurLoc(string Id, Location loc, double rad)
        {
            List<ViewTask> list = new List<ViewTask>();
            foreach (var collection in DbSet<MongoManager.CollectionModels.Task>.collections)
            {
                var c = collection.Value
                    .Find(x => x.Main.Location.GeoLong  < rad + loc.GeoLong && x.Main.Location.GeoLong > -rad + loc.GeoLong
                            && x.Main.Location.GeoLat < rad + loc.GeoLat && x.Main.Location.GeoLat > -rad + loc.GeoLat)
                    .ToList();
                foreach (var task in c)
                {
                    ViewTask view = new ViewTask();
                    view.About = task.Main.About;
                    view.Comments = task.Comments;
                    view.PicturesAbout = task.Main.PicturesAbout;
                    view.Tags = task.Main.Tags;
                    view._id = task._id;
                    view.Title = task.Main.Title;
                    for (int i = 1; i < 6; i++)
                    {
                        view.Ranks[i] = task.Ranks[i].Count;
                    }
                    list.Add(view);
                }
            }
            return list;
        }

        public static void AddRanks(string UserId, string TaskId, int k)
        {
            DataManager.Tasks.DeleteRanks(TaskId, UserId);
            DataManager.Tasks.AddRankss(TaskId, UserId, k);

        }

        public static void AddTaskToActive(string UserId, string TaskId)
        {
            string[] a = new string[1];
            a[0] = TaskId;
            DataManager.Users.AddActiveTasks(UserId,a);
        }

        public static List<ViewTask> GetNextTenActiveTask(string UserId, int k)
        {
            List<ViewTask> list = new List<ViewTask>();

            string[] tasks = DataManager.Users.ReadFull(UserId).ActiveTask.ToArray();
            for (int i = k * 10; i < (k + 1) * 10 && i < tasks.Length; i++)
            {
                ViewTask view = new ViewTask();
                MongoManager.CollectionModels.Task task = DataManager.Tasks.ReadFull(tasks[i]);
                view.About = task.Main.About;
                view.Comments = task.Comments;
                view.PicturesAbout = task.Main.PicturesAbout;
                view.Tags = task.Main.Tags;
                view._id = task._id;
                view.Title = task.Main.Title;
                for (int q = 1; q < 6; q++)
                {
                    view.Ranks[q] = 5;
                }
                list.Add(view);

            }
            return list;
        }

        /// <summary>
        /// Повертає список запитань даного завдання 
        /// </summary>
        public static List<Question> GetTaskQuestions(string TaskId)
        {
            return null;
        }
    }
}
