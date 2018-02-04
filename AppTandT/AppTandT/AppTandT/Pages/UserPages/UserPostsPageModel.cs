using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamvvm;
using AppTandT.BLL.Model.ViewModels;

namespace AppTandT.Pages.UserPages
{
    
        public class UserPostsPageModel : BasePageModel
        {

            public UserPostsPageModel()
            {
                PostSelectedCommand = new BaseCommand<SelectedItemChangedEventArgs>((arg) =>
                {
                    SelectedPost = null;
                });
                Reload();
            }
            public PostItem SelectedPost { get; set; }
            public ICommand PostSelectedCommand { get; set; }

            public ObservableCollection<PostItem> Items { get; set; }

            public async Task Reload()
            {
                if (Items == null)
                    Items = new ObservableCollection<PostItem>();
                // var list = await PostService.GetPostsPageAsync(Sesion._id);
                var t = 0;
                var Post1 = new PostItem()
                {
                    MainImageUrl = "https://scontent-waw1-1.xx.fbcdn.net/v/t1.0-9/21761765_180870455791802_8497190410053769062_n.jpg?oh=dddc78e9d198285ba10b7a83b127a165&oe=5A9923E1",
                    AvatarUrl = "https://scontent-waw1-1.xx.fbcdn.net/v/t1.0-9/21761765_180870455791802_8497190410053769062_n.jpg?oh=dddc78e9d198285ba10b7a83b127a165&oe=5A9923E1",
                    Login = "Slawik",
                    About = "Про пост",
                    h = 0,
                    HeartCommand = new BaseCommand((arg) =>
                    {
                        System.Diagnostics.Debug.WriteLine("Like TAPPED");

                    })
                };
                var Post2 = new PostItem()
                {
                    MainImageUrl = "https://scontent-frt3-2.xx.fbcdn.net/v/t1.0-9/23755153_1554254364668430_6513016014356578767_n.jpg?oh=ca6cc762d53fcb6fc734d221b6eeb8d7&oe=5AA82104",
                    AvatarUrl = "https://scontent-frt3-2.xx.fbcdn.net/v/t1.0-9/23755153_1554254364668430_6513016014356578767_n.jpg?oh=ca6cc762d53fcb6fc734d221b6eeb8d7&oe=5AA82104",
                    Login = "Povodka",
                    About = "Про пост",
                };
                Items.Add(Post1);
                Items.Add(Post2);
                Items.Add(Post1);
                Items.Add(Post2);
                Items.Add(Post1);
                Items.Add(Post2);
                Items.Add(Post1);
                Items.Add(Post2);

                // foreach (var cur in list)
                //     Items.Add(new PostItem (cur));
            }
        }

        public class PostItem : BaseModel
        {
            public PostItem()
            {

            }

            public PostItem(PostViewModel m)
            {
                AvatarUrl = m.UserAvatar;
                id = m.id;
                Photo = m.Photo;
                //MainImageUrl =  (Photo == null ? "" : Photo[0] ?? "");
                MainImageUrl = m.URLphoto == null ? "" : (m.URLphoto[0] ?? "");
                About = "";
                UserId = m.UserId;
                LikeSum = m.CountLikes;
                Comment = m.Comment;
                DateCreated = m.DateCreated;
            }

            public string id { get; set; }
            public string UserId { get; set; }

            public bool Comment { get; set; }

            public DateTime DateCreated { get; set; }

            public string AvatarUrl { get; set; }

            public string About { get; set; }

            public int LikeSum { get; set; }

            public string Login { get; set; }

            public string MainImageUrl { get; set; }

            public List<string> Photo { get; set; }
            public int h = 0;
            public ICommand Command { get; set; }
            public ICommand ChatCommand { get; set; } = new BaseCommand((arg) =>
            {
            });
            public ICommand SentCommand { get; set; } = new BaseCommand((arg) =>
            {
            });
            public ICommand SaveCommand { get; set; } = new BaseCommand((arg) =>
            {
            });
            public ICommand HeartCommand { get; set; } = new BaseCommand((arg) =>
            {
            });

        }

    }

