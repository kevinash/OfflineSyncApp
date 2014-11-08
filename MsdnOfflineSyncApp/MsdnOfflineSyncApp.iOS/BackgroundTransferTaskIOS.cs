using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace MsdnOfflineSyncApp
{
    public class BackgroundTransferTaskIOS : IBackgroundTransfer
    {
        TransferTaskMode _mode = TransferTaskMode.Download;
        string _sessionId;
        string _url;

        public NSUrlSessionDownloadTask downloadTask;
        public NSUrlSessionUploadTask uploadTask;
        public NSUrlSession session;

        public BackgroundTransferTaskIOS() { }

        public void Init(string sessionId, string url, TransferTaskMode mode)
        {
            using (var configuration = NSUrlSessionConfiguration.BackgroundSessionConfiguration(sessionId))
            {
                _mode = mode;
                _sessionId = sessionId;
                _url = url;
                session = NSUrlSession.FromConfiguration(configuration);
            }
        }

        public void Start()
        {
            if (_mode == TransferTaskMode.Upload)
                StartUpload();
            else
                StartDownload();
        }

        void StartDownload()
        {
            if (downloadTask != null)
                return;

            using (var uri = NSUrl.FromString(_url))
            using (var request = NSUrlRequest.FromUrl(uri))
            {
                downloadTask = session.CreateDownloadTask(request);
                downloadTask.Resume();
            }
            
        }

        void StartUpload()
        {
            if (uploadTask != null)
                return;

            using (var uri = NSUrl.FromString(_url))
            using (var request = NSUrlRequest.FromUrl(uri))
            {
                uploadTask = session.CreateUploadTask(request);
                uploadTask.Resume();
            }
        }

    }

    

}