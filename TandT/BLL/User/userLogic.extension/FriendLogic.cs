using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using MongoManager;
using MongoManager.CollectionModels;
using MongoManager.Context;


namespace BLL.userLogic.extension
{
    public static class FriendLogic
    {
        /// <summary>
        /// Sub = Subscription
        /// SubId - id тіпа який підписується на userId
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="SubId"></param>
        public static bool AddFriend(string userId, string SubId)
        {
            bool flag=DataManager.Users.ReadFull(SubId).Subscriptions.Contains(userId);
            if (flag)
            {
                List<string> list = new List<string>();
                list.Add(SubId);
                DataManager.Users.DeleteSubscriptions(userId, list.ToArray());
                DataManager.Users.AddFriends(userId, list.ToArray());
                list.Clear();
                list.Add(userId);
                DataManager.Users.DeleteSubscriptions(SubId, list.ToArray());
                DataManager.Users.AddFriends(SubId, list.ToArray());
            }
            else
            {
                List<string> list = new List<string>();
                list.Add(userId);
                DataManager.Users.AddSubscriptions(SubId, list.ToArray());

            }
            return !(DataManager.Users.ReadFull(SubId).Subscriptions.Contains(userId) & DataManager.Users.ReadFull(userId).Subscriptions.Contains(SubId));
        }
        /// <summary>
        /// userId видаляє з друзів friendId
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="subId"></param>
        public static void DeleteFriend(string userId, string friendId)
        {
            List<string> list = new List<string>();
            list.Add(friendId);
            DataManager.Users.DeleteFriends(userId, list.ToArray());
            list.Clear();
            list.Add(userId);
            DataManager.Users.DeleteFriends(friendId,list.ToArray());
            DataManager.Users.AddSubscriptions(friendId, list.ToArray());
        }
        /// <summary>
        /// Вертає друзів з 10*к по (к+1)*10 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public static List<UserFriend> GetNextTenFriends(string userId, int k)
        {
            List<UserFriend> list = new List<UserFriend>();
            string[] friends = DataManager.Users.ReadFull(userId).Friends.ToArray();
            for (int i = k * 10; i < (k + 1) * 10; i++)
            {
                UserFriend user = new UserFriend();
                user.Avatar = DataManager.Users.ReadMain(friends[i]).Avatar;
                user.id = friends[i];
                user.Name = DataManager.Users.ReadMain(friends[i]).Name;
                list.Add(user);
            }
            return list;
        }
        /// <summary>
        /// Sub = Subscription
        /// SubId відписується від userId
        /// </summary>
        public static void DeleteSubscription(string userId, string SubId)
        {
            List<string> list = new List<string>();
            list.Add(userId);
            DataManager.Users.DeleteSubscriptions(SubId, list.ToArray());

        }
    }
}
