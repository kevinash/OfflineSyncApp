using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using System.Threading.Tasks;


namespace MsdnOfflineSyncApp
{
    public class SensorModel
    {
        const string applicationURL = @"<YOUR APP URL>";
        const string applicationKey = @"<YOUR APP KEY>";
        const string syncStorePath = "sync.db";
        MobileServiceClient client;
        IMobileServiceSyncTable<SensorDataItem> sensorDataTable;
     
        public SensorModel()
        {
            // initialize mobile services and SQLLite store
            Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init();
#if __IOS__
            SQLitePCL.CurrentPlatform.Init();
#endif

            // initialize the client with your app url and key
            client = new MobileServiceClient(applicationURL, applicationKey);
            // create sync table instance
            sensorDataTable = client.GetSyncTable<SensorDataItem>();
        }

        public async Task InitStoreAsync()
        {
            if (!client.SyncContext.IsInitialized)
            {
                var store = new MobileServiceSQLiteStore(syncStorePath);
                store.DefineTable<SensorDataItem>();
                await client.SyncContext.InitializeAsync(store, new MobileServiceSyncHandler());
            }
        }

        public async Task PushAsync()
        {
            try
            {
                await this.client.SyncContext.PushAsync();
           }
            catch (MobileServiceInvalidOperationException e)
            {
                Console.Error.WriteLine(@"Sync Failed: {0}", e.Message);
            }
        }

        public async Task PullAsync()
        {
            try
            {
                var query = sensorDataTable.Where(s => s.speed > 0);
                await sensorDataTable.PullAsync(query);
            }
            catch (MobileServiceInvalidOperationException e)
            {
                Console.Error.WriteLine(@"Sync Failed: {0}", e.Message);
            }
        }

        public async Task PurgeAsync()
        {
            try
            {
                var query = sensorDataTable.Where(s => s.speed > 0);
                await sensorDataTable.PurgeAsync(query);
            }
            catch (MobileServiceInvalidOperationException e)
            {
                Console.Error.WriteLine(@"Purge Failed: {0}", e.Message);
            }
        }

     
        public async Task SaveAsync(SensorDataItem item)
        {
            try
            {
                // insert a new TodoItem into the database
                await this.sensorDataTable.InsertAsync(item);
            }
            catch (MobileServiceInvalidOperationException e)
            {
                Console.Error.WriteLine(@"Insert Failed: {0}", e.Message);
            }
        }

    }
}
