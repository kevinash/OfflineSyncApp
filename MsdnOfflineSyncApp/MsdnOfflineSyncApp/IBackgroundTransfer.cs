using System;
using System.Collections.Generic;
using System.Text;

namespace MsdnOfflineSyncApp
{
    // this interface is to demonstrate transferring
    // blobs using native services on each platform
    public interface IBackgroundTransfer
    {
        void Init(string sessionId, string url, TransferTaskMode mode);
        void Start();
    }

    public enum TransferTaskMode
    {
        Download,
        Upload
    }
}
