using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using PCLStorage;

namespace MsdnOfflineSyncApp
{
    public class TransferQueue
    {
        ConcurrentQueue<Job> _jobQueue = new ConcurrentQueue<Job>();

        public ConcurrentQueue<Job> JobQueue
        {
            get { return _jobQueue; }
            set { _jobQueue = value; }
        }

        public TransferQueue()
        {

        }

        public void AddOutProcess(Job job)
        {
            JobQueue.Enqueue(job);
            IBackgroundTransfer transfer = null;
#if __IOS__
           transfer = new BackgroundTransferTaskIOS();
#endif
            transfer.Init("com.test.job" + job.Id, job.Url, TransferTaskMode.Download);
            transfer.Start();
        }

        public async Task<bool> AddInProcessAsync(Job job)
        {
            JobQueue.Enqueue(job);
            var folder = await FileSystem.Current.LocalStorage.CreateFolderAsync("downloads", CreationCollisionOption.OpenIfExists);
            return await BlobTransfer.DownloadFileAsync(folder, job.Url, job.LocalFile);
        }
    }

    public class Job
    {
        public int Id { get; set; }
        public string Url { get; set; }

        public string LocalFile { get; set; }
    }
}
