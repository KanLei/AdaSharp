using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AdaSharp.Wallet.DTOs.Request
{
    public class WalletCreatedRequest
    {
        [JsonProperty(PropertyName = "operation")]
        public string Operation { get; set; }
        [JsonProperty(PropertyName = "backupPhrase")]
        public List<string> BackupPhrase { get; set; }
        [JsonProperty(PropertyName = "assuranceLevel")]
        public string AssuranceLevel { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "spendingPassword")]
        public string SpendingPassword { get; set; }
    }
}
