using AppTandT.BLL.Help;
using AppTandT.BLL.Model.CollectionModels;
using AppTandT.BLL.Model.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AppTandT.BLL.Services
{
    public class PostService : ClientService
    {
        public static async Task<List<PostViewModel>> GetNewsAsync()
        {
            var client = GetClient();
            var url = App.ApiUri + "api/post/getnews";
            var response = /*await*/ client.GetAsync(url).Result;

            if (response.StatusCode != HttpStatusCode.OK)
                throw new ServiceException("Can't connect to the server! Try again later.", title: "Ooops");

            var str = await response.Content.ReadAsStringAsync();
            var arr = JsonConvert.DeserializeObject<List<PostViewModel>>(str);
            return arr ;
        }

        public static async Task<List<PostViewModel>> GetPostsPageAsync(string id, int page = 0)
        {
            var client = GetClient();
            var url = App.ApiUri + "api/post/getposts?id=" + id + "&k=" + page;
            var response = await client.GetAsync(url);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new ServiceException("Can't connect to the server! Try again later.", title: "Ooops");

            var str = await response.Content.ReadAsStringAsync();
            var arr = JsonConvert.DeserializeObject<List<PostViewModel>>(str);
            return arr;
        }

        public static async Task<bool> AddPostAsync(Post post)
        {
            var client = GetClient();
            var url = App.ApiUri + "api/post/addpost";
            var response = await client.PostAsync(url, new StringContent(
                    JsonConvert.SerializeObject(post),
                    Encoding.UTF8,
                    "application/json")
            );

            if (response.IsSuccessStatusCode) return true;

            var str = await response.Content.ReadAsStringAsync();
            var obj = JsonConvert.DeserializeObject<JwtModel>(str);
            throw new ServiceException(obj.message);
        }

        ///photo stuff
        public static async Task<string> AddPhotoAsync(Photo photo)
        {
            var client = GetClient();
            var url = App.ApiUri + "api/post/addphoto";
            var response = client.PostAsync(url, new StringContent(
                    JsonConvert.SerializeObject(photo),
                    Encoding.UTF8,
                    "application/json")
            ).Result;
            return await response.Content.ReadAsStringAsync();
        }
    }
}
