using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.Sync;

namespace MsdnOfflineSyncApp
{
    public class SensorDataItem
    {
        public string Id { get; set; }
        
        [Version]
        public string Version { get; set; }

        [JsonProperty]
        public string text { get; set; }

        [JsonProperty]
        public double latitude { get; set; }

        [JsonProperty]
        public double longitude { get; set; }

        [JsonProperty]
        public double distance { get; set; }

        [JsonProperty]
        public double speed { get; set; }
    }
}
