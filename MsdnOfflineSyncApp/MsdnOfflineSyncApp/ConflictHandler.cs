using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MsdnOfflineSyncApp
{
    public class ConflictHandler : IMobileServiceSyncHandler
    {
        MobileServiceClient _client;
        ConflictResolutionPolicy _conflictResolutionPolicy = ConflictResolutionPolicy.KeepLocal;

        public ConflictHandler(MobileServiceClient client, ConflictResolutionPolicy conflictResolutionPolicy)
        {
            this._client = client;
            _conflictResolutionPolicy = conflictResolutionPolicy;
        }

        public virtual Task OnPushCompleteAsync(MobileServicePushCompletionResult result)
        {
            return Task.FromResult(0);
        }

        public virtual async Task<JObject> ExecuteTableOperationAsync(IMobileServiceTableOperation op)
        {
            MobileServicePreconditionFailedException error;
            do
            {
                error = null;

                try
                {
                    return await op.ExecuteAsync();
                }
                catch (MobileServicePreconditionFailedException ex)
                {
                    error = ex;
                }

                if (error != null)
                {
                    // conflict on the server
                    var localValue = op.Item.ToObject<SensorDataItem>();
                    var serverValue = error.Value;
                    switch (_conflictResolutionPolicy)
                    {
                        case ConflictResolutionPolicy.KeepLocal:
                            // forcing local (client) data
                            if (serverValue == null)
                            {
                                // try retrieving serverValue (null)
                                var table = _client.GetTable(op.Table.TableName);
                                table.SystemProperties = MobileServiceSystemProperties.Version;
                                serverValue = (JObject)(await table.LookupAsync((string)op.Item[MobileServiceSystemColumns.Id]));
                            }
                            if (serverValue != null)
                                // update local with server value
                                op.Item[MobileServiceSystemColumns.Version] = serverValue[MobileServiceSystemColumns.Version];
                            else
                                op.AbortPush();
                            break;
                        case ConflictResolutionPolicy.KeepRemote:
                            return (JObject)serverValue;
                        default:
                            op.AbortPush();
                            break;
                    }

                }
            } while (error != null);

            return null;
        }


    }

    public enum ConflictResolutionPolicy
    {
        KeepLocal,
        KeepRemote,
        Abort
    }
}
