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
        public static async Task performBlobOperation(Stream stream)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=tandtblob;AccountKey=021iSbqy1gcloCHLvV+kPT+FsemAmc/3Nq6m1ZJcRtRqw6REk1zr3+qHKNB6JiFcRMevF+d2fsC0wYlxjSIFlg==;EndpointSuffix=core.windows.net");


            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("photos");
            
            await container.CreateIfNotExistsAsync();
            string name = "photka" + DateTime.Now.Ticks + ".jpg";
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(name);

            await blockBlob.UploadFromStreamAsync(stream);
            Post post = new Post()
            {
                About = "New post",
                Main = new Model.CollectionModels.MainCollectionModels.MainPost()
                {
                    DatePost = DateTime.Now,
                    UserId = Sesion._id,
                    Photos = new List<string>() { BLL.Services.PostService.AddPhotoAsync(
                        new Photo()
                        {
                            _id = "",
                            URL = @"https://tandtblob.blob.core.windows.net/photos/" + name,
                        }
                        
                        ).Result},
                },
            };

            await BLL.Services.PostService.AddPostAsync(post);
        }
    }
}
