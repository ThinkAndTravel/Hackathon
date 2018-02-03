using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using MongoManager;
using MongoManager.CollectionModels;
using MongoManager.Context;


namespace BLL.userLogic.extension
{
    public static class NoticeLogic
    {
        /// <summary>
        /// розіслання сповіщеннь друзям
        /// </summary>
        /// <param name="notice"></param>
        public static void AddNoteToFriend(this UserLogic UL,Notice notice)
        {
            var friends =  DataManager.Users.ReadFull(notice.User).Friends;
            List<Notice> not = new List<Notice>();
            not.Add(notice);
            foreach (var o in friends)
            {
                DataManager.Users.AddNotices(o, not.ToArray());
            }
        }

        /// <summary>
        /// Видалення прочитаних сповіщеннь
        /// </summary>
        /// <param name="id"></param>
        public static void ReadNote(this UserLogic UL, string id)
        {
            List<Notice> notice = DataManager.Users.ReadFull(id).Notices;
            DataManager.Users.DeleteNotices(id, notice.ToArray());
        }
        /// <summary>
        /// Кількість непрочитаних сповіщень
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int CheckNote(this UserLogic UL, string id)
        {
            List<Notice> notice = DataManager.Users.ReadFull(id).Notices;
           
            return notice.Count;
        }

        /// <summary>
        /// Повертає список моделей усіх непрочитаних сповіщень користувача
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static  List<NoticeView> GetNotes(this UserLogic UL, string id)
        {
            List<NoticeView> list = new List<NoticeView>();
            foreach (var o in DataManager.Users.ReadFull(id).Notices)
            {
                NoticeView notice = new NoticeView();
                notice.id = o.User;
                MainUser user = DataManager.Users.ReadMain(id);
                notice.Name = user.FirstName + user.LastName;
                notice.Avatar = user.Avatar;
                notice.DateCreated = o.DateMessage.Value;
                notice.plan = o.Plan;
                list.Add(notice);
            }
            return list;
        }
    }
    
}
