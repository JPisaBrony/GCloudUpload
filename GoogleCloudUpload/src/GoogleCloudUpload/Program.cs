using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Services;
using Google.Apis.Storage.v1;

namespace GoogleCloudUpload
{
    public class Program
    {
        public static void Main(string[] args)
        {
            String bucketName = "jpstorage";
            UploadStream(bucketName);
            ListObjects(bucketName);
            DeleteObject(bucketName);
            ListObjects(bucketName);
            Console.ReadKey();
        }

        public static StorageService CreateStorageClient()
        {
            var credentials = Google.Apis.Auth.OAuth2.GoogleCredential.GetApplicationDefaultAsync().Result;

            if (credentials.IsCreateScopedRequired)
            {
                credentials = credentials.CreateScoped(new[] { StorageService.Scope.DevstorageFullControl });
            }

            var serviceInitializer = new BaseClientService.Initializer()
            {
                ApplicationName = "Storage Sample",
                HttpClientInitializer = credentials
            };

            return new StorageService(serviceInitializer);
        }
        public static void UploadStream(string bucketName)
        {
            StorageService storage = CreateStorageClient();

            var content = "My text object content and stuff";
            var uploadStream = new MemoryStream(Encoding.UTF8.GetBytes(content));

            storage.Objects.Insert(
                bucket: bucketName,
                stream: uploadStream,
                contentType: "text/plain",
                body: new Google.Apis.Storage.v1.Data.Object() { Name = "my-file2.txt" }
            ).Upload();

            Console.WriteLine("Uploaded my-file.txt");
        }

        public static void DeleteObject(string bucketName)
        {
            StorageService storage = CreateStorageClient();

            storage.Objects.Delete(bucketName, "my-file.txt").Execute();

            Console.WriteLine("Deleted my-file.txt");
        }

        public static void ListObjects(string bucketName)
        {
            StorageService storage = CreateStorageClient();

            var objects = storage.Objects.List(bucketName).Execute();

            if (objects.Items != null)
            {
                foreach (var obj in objects.Items)
                {
                    Console.WriteLine($"Object: {obj.Name}");
                }
            }
        }

    }
}
