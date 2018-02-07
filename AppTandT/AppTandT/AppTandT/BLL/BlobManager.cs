using AppTandT.BLL.Model.CollectionModels;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppTandT.BLL
{
    public class BlobManager
    {
        public static async Task<int> performBlobOperation(Stream stream)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=tandtblob;AccountKey=021iSbqy1gcloCHLvV+kPT+FsemAmc/3Nq6m1ZJcRtRqw6REk1zr3+qHKNB6JiFcRMevF+d2fsC0wYlxjSIFlg==;EndpointSuffix=core.windows.net");


            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("photos");
            
            await container.CreateIfNotExistsAsync();
            string name = "photka" + DateTime.Now.Ticks + ".jpg";
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(name);

            await blockBlob.UploadFromStreamAsync(stream);
            string photoid = await BLL.Services.PostService.AddPhotoAsync(
                        new Photo()
                        {
                            _id = "",
                            URL = @"https://tandtblob.blob.core.windows.net/photos/" + name,
                        }

                        );
            photoid = photoid.Substring(1, photoid.Length - 2);

            Post post = new Post()
            {
                _id = Sesion._id+DateTime.Now.ToString(),
                About = "New post",
                Main = new Model.CollectionModels.MainCollectionModels.MainPost()
                {
                    DatePost = DateTime.Now,
                    UserId = Sesion._id,
                    Photos = new List<string>() { photoid },
                    CurTask = new Model.CollectionModels.Task()
                    {
                        _id = "fdsfdsfsd",
                        IsPersonal = false,
                        Main =  new Model.CollectionModels.MainCollectionModels.MainTask()
                        {
                            TypeTask = 0,
                            Location = new Model.CollectionModels.HelpCollectionModels.Location() {
                                GeoLong = 0.5212,
                                GeoLat = 1.2132,
                                Country =  "Chechnya",
                                City =  "Grozny"
                            },
                            Questions = new List<Model.CollectionModels.HelpCollectionModels.Question>(),
                            Tags = new List<string>(),
                            About = "about"
                        },
                        Country = "Chechnya",
                        City =  "Grozny",
                        Comments = new List<Model.CollectionModels.HelpCollectionModels.Comment>()
                    }
                },
            };
           
            await BLL.Services.PostService.AddPostAsync(post);
            return 0;
        }
    }
}
