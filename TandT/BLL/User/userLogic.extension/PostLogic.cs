using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using MongoManager;
using MongoManager.CollectionModels;
using MongoManager.Context;
using BLL.User.ViewModel;
using MongoManager.CollectionModels.HelpCollectionModels;
using System.Threading.Tasks;

namespace BLL.userLogic.extension
{
    public static class PostLogic
    {



        public static void AddPost(string id, Post post)
        {
            
            DataManager.Posts.Create(ref post);
            string[] arr = new string[1];
            arr[0] = post._id;
            DataManager.Users.AddPosts(post.Main.UserId, arr); 
        }
        public static void DeletePost(string id)
        {
            List<string> list = new List<string>();
            list.Add(id);
            DataManager.Users.DeletePosts(DataManager.Posts.ReadMain(id).UserId, list.ToArray());
            DataManager.Posts.Delete(id);
        }

        public static async Task<List<ViewPost>> GetAllPosts()
        {
            List<ViewPost> list = new List<ViewPost>();
            var e = await DataManager.Posts.ReadAllPostsAsync();
            foreach(var post in e)
            {
                ViewPost view = new ViewPost();
                view.UserId = post.Main.UserId;
                view.id = post._id;
                if (post.PostComments.Count == 0) view.Comment = false; else view.Comment = true;
                view.CountLikes = post.Likes.Count;
                view.DateCreated = post.Main.DatePost.Value;
                view.About = post.About;
                List<string> url = new List<string>();
                foreach (var c in post.Main.Photos)
                {
                    url.Add(DataManager.Photos.ReadFull(c).URL);
                }
                view.URLPhoto = url;
                view.UserAvatar = DataManager.Users.ReadMain(view.UserId).Avatar;
                list.Add(view);
            }

            return list;
        }

        public static List<ViewPost> GetNextTenPost(string UserId, int k)
        {
            List<ViewPost> list = new List<ViewPost>();

            string[] posts = DataManager.Users.ReadFull(UserId).Posts.ToArray();
            for (int i = k * 10; i < (k + 1) * 10 && i < posts.Length ; i++)
            {
                ViewPost view = new ViewPost();
                Post post = DataManager.Posts.ReadFull(posts[i]);
                view.UserId = UserId;
                view.id = post._id;
                if (post.PostComments.Count == 0) view.Comment = false; else view.Comment = true;
                view.CountLikes = post.Likes.Count;
                view.DateCreated = post.Main.DatePost.Value;
                view.About = post.About;
                List<string> url = new List<string>();
                foreach (var c in post.Main.Photos)
                {
                    url.Add(DataManager.Photos.ReadFull(c).URL);
                }
                view.URLPhoto = url;
                view.UserAvatar = DataManager.Users.ReadMain(UserId).Avatar;
                list.Add(view);
            }
            return list;
        }
        public static void ClickLikePost(string UserId,string PostId)
        {
            List<string> list = new List<string>();
            list.Add(UserId);
            var a = DataManager.Posts.ReadFull(PostId).Likes;
            if (!a.Contains(UserId)) DataManager.Posts.AddLikes(PostId, list.ToArray());
            else DataManager.Posts.DeleteLikes(PostId, list.ToArray());
        }
        public static void AddPostComment(string PostId, Comment comment)
        {
            List<Comment> list = new List<Comment>();
            list.Add(comment);
            DataManager.Posts.AddPostComments(PostId, list.ToArray());
        }
        public static void AddPostPhotoComments(string PhotoId, Comment comment)
        {
            List<Comment> list = new List<Comment>();
            list.Add(comment);
            DataManager.Posts.AddPostComments(PhotoId, list.ToArray());

        }
    
        public static void DeletePostComment(string PostId, Comment comment)
        {
            List<Comment> list = new List<Comment>();
            list.Add(comment);
            DataManager.Posts.DeletePostComments(PostId, list.ToArray());
        }
        public static void LikePostComment(string PostId, string UserId, Comment comment)
        {
            List<Comment> list = new List<Comment>();
            list.Add(comment);
            DataManager.Posts.DeletePostComments(PostId, list.ToArray());
            list.Clear();
            comment.Likes.Add(UserId);
            list.Add(comment);
            DataManager.Posts.AddPostComments(PostId, list.ToArray());
        }
        public static void UnLikePostComment(string PostId, string UserId, Comment comment)
        {
            List<Comment> list = new List<Comment>();
            list.Add(comment);
            DataManager.Posts.DeletePostComments(PostId, list.ToArray());
            list.Clear();
            comment.Likes.Remove(UserId);
            list.Add(comment);
            DataManager.Posts.AddPostComments(PostId, list.ToArray());
        }
        public static void LikePostPhoto(string UserId, string PhotoId)
        {
            List<string> list = new List<string>();
            list.Add(UserId);
            DataManager.Photos.AddLikes(PhotoId, list.ToArray());
        }
        public static void UnLikePostPhoto(string UserId, string PhotoId)
        {
            List<string> list = new List<string>();
            list.Add(UserId);
            DataManager.Photos.DeleteLikes(PhotoId, list.ToArray());
        }
        public static void LikePhotoComment(string PhotoId, string UserId, Comment comment)
        {
            List<Comment> list = new List<Comment>();
            list.Add(comment);
            DataManager.Photos.DeleteComments(PhotoId, list.ToArray());
            list.Clear();
            comment.Likes.Add(UserId);
            list.Add(comment);
            DataManager.Photos.AddComments(PhotoId, list.ToArray());
        }
        public static void UnLikePhotoComment(string PhotoId, string UserId, Comment comment)
        {
            List<Comment> list = new List<Comment>();
            list.Add(comment);
            DataManager.Photos.DeleteComments(PhotoId, list.ToArray());
            list.Clear();
            comment.Likes.Remove(UserId);
            list.Add(comment);
            DataManager.Photos.AddComments(PhotoId, list.ToArray());
        }
        public static List<ViewComment> GetNext30PostComment(string PostId, int k)
        {
            List<ViewComment> list = new List<ViewComment>();
            Comment[] comment = DataManager.Posts.ReadFull(PostId).PostComments.ToArray();
            for (int i = 30 * k; i < (k + 1) * 30; i++)
            {
                ViewComment view = new ViewComment();
                view.CountLike = comment[i].Likes.Count;
                view.DateCreated = comment[i].DateComent.Value;
                view.UserAvatar = DataManager.Users.ReadMain(comment[i].User).Avatar;
                view.UserId = comment[i].User;
                view.UserName = DataManager.Users.ReadMain(comment[i].User).Name;
                view.Value = comment[i].Value;
                list.Add(view);
            }
            return list;
        }
    }
}
