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
        public static async Task performBlobOperation(string nameInBlob, string LocalPath)
        {
            try
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=tandtblob;AccountKey=021iSbqy1gcloCHLvV+kPT+FsemAmc/3Nq6m1ZJcRtRqw6REk1zr3+qHKNB6JiFcRMevF+d2fsC0wYlxjSIFlg==;EndpointSuffix=core.windows.net");


                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("photos");
            
            await container.CreateIfNotExistsAsync();
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(nameInBlob);

            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                throw new Exception("No Camera:(");           
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                SaveToAlbum = true,
                Directory = "TandT",
                Name = nameInBlob,
                PhotoSize = PhotoSize.Medium,
            });

            await blockBlob.UploadFromStreamAsync(file.GetStream());
            }
            catch (Exception e)
            {

            }
        }
    }
}
