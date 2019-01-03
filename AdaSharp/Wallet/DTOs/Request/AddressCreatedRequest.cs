using Newtonsoft.Json;
using System;

namespace AdaSharp.Wallet.DTOs.Request
{
    public class AddressCreatedRequest
    {
        [JsonProperty(PropertyName = "walletId")]
        public string WalletId { get; set; }

        [JsonProperty(PropertyName = "accountIndex")]
        public UInt32 AccountIndex { get; set; }

        [JsonProperty(PropertyName = "spendingPassword")]
        public string SpendingPassword { get; set; }
    }
}
