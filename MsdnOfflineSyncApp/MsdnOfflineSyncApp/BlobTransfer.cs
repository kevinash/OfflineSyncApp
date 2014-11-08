using PCLStorage;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MsdnOfflineSyncApp
{
    public class BlobTransfer 
    {
        public static async Task<bool> DownloadFileAsync(IFolder folder, string url, string fileName)
        {
            using (var client = new HttpClient())
            using (var response = await client.GetAsync(url))
            {
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Stream temp = await response.Content.ReadAsStreamAsync();
                    IFile file = await folder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
                    using (var fs = await file.OpenAsync(PCLStorage.FileAccess.ReadAndWrite))
                    {
                        await temp.CopyToAsync(fs);
                        fs.Close();
                        return true;
                    }
                }
                else
                {
                    Debug.WriteLine("NOT FOUND " + url);
                    return false;
                }
            }
        }

        public static async Task UploadFileAsync(IFolder folder, string fileName, string fileUrl)
        {
            using (var client = new HttpClient())
            {
                var file = await folder.GetFileAsync(fileName);
                var fileStream = await file.OpenAsync(PCLStorage.FileAccess.Read);
                var content = new StreamContent(fileStream);
                content.Headers.Add("Content-Type", "application/octet-stream");
                content.Headers.Add("x-ms-blob-type", "BlockBlob");
                using (var uploadResponse = await client.PutAsync(new Uri(fileUrl, UriKind.Absolute), content))
                {
                    Debug.WriteLine("CLOUD UPLOADED " + fileName);
                    return;
                }
            }
        }
    }
}
