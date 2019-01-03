using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdaSharp.Wallet.DTOs.Request
{
    public class AccountCreatedRequest
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "spendingPassword")]
        public string SpendingPassword { get; set; }
    }
}
