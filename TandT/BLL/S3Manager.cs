using System;
using System.Collections.Generic;
using System.Text;
using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using System.Linq;
using Amazon.Runtime;

namespace BLL
{
    public class S3Manager
    {
        public bool sendMyFileToS3( string fileNameInS3)
        {
            // input explained :
            // localFilePath = the full local file path e.g. "c:\mydir\mysubdir\myfilename.zip"
            // bucketName : the name of the bucket in S3 ,the bucket should be alreadt created
            // fileNameInS3 = the file name in the 
            string bucketName = "tandtphoto";
            AWSCredentials credentials = new BasicAWSCredentials("AKIAI24CSDII5HVHZOBA", "QF7tB8sUvNGgfh3cIhoQPKYC6BFBMfAHGdS1hiuu");
            AmazonS3Config config = new AmazonS3Config();
            config.ServiceURL = "s3.amazonaws.com";
            config.RegionEndpoint = Amazon.RegionEndpoint.EUCentral1;
            var client = new AmazonS3Client(credentials, config);
            TransferUtility utility = new TransferUtility(client);
            TransferUtilityUploadRequest request = new TransferUtilityUploadRequest();
            request.BucketName = bucketName;         
            request.Key = fileNameInS3; //file name up in S3
           // request.InputStream();
            request.CannedACL = S3CannedACL.PublicRead;
            utility.Upload(request); //commensing the transfer
            
            return true; //indicate that the file was sent
        }


    }
}
