using System;
using Minio;
using Minio.Exceptions;
using Minio.DataModel;
using System.Threading.Tasks;
using System.Net;

namespace FileUploader
{
    /// <summary>
    /// 
    /// </summary>
    class FileUpload
    {
        static void Main(string[] args)
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
            //| SecurityProtocolType.Tls12
            //| SecurityProtocolType.Tls11
            //| SecurityProtocolType.Tls12;
            var endpoint = "10.221.5.254:9000";
            var accessKey = "XZUYUQ3X7S7631CIS6TI";
            var secretKey = "YuOtOVd5DppLJ+Knf2GhIo6Dhb/SNHA7ktX13DHO";
            try
            {
                var minio = new MinioClient(endpoint, accessKey, secretKey);//.WithSSL();
                FileUpload.Run(minio).Wait();
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine(ex.Message);
            }
            Console.ReadLine();
        }
        //Check if a bucket exists
        private async static Task Run(MinioClient minio)
        {
            // Make a new bucket called mymusic.
            var bucketName = "classifieds"; //<==== change this
            var location = "us-east-1";
            // Upload the zip file
            var objectName = "Lighthouse.jpg";
            var filePath = "C:\\Users\\Public\\Pictures\\Sample Pictures\\Lighthouse.jpg";
            var contentType = "application/zip";

            try
            {
                bool found = await minio.Api.BucketExistsAsync(bucketName);
                if (!found)
                {
                    Console.Out.WriteLine("bucket-name was " + ((found == true) ? "found" : "not found"));
                }
                else
                {
                    await minio.Api.PutObjectAsync(bucketName, objectName, filePath, contentType);
                    Console.Out.WriteLine("Successfully uploaded " + objectName);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("[Bucket]  Exception: {0}", e);
            }
        }
   

    }
}
