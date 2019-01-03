using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace AdaSharp.Wallet.DTOs
{
    public class TransactionIOput
    {
        [JsonProperty(PropertyName = "amount")]
        public decimal Amount { get; set; }
        [JsonProperty(PropertyName = "address")]
        public string Address { get; set; }
    }
}
