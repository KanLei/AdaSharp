using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AdaSharp.Wallet.DTOs.Request
{
    public class TransactionCreatedRequest
    {
        [JsonProperty(PropertyName = "groupingPolicy")]
        public GroupingPolicy GroupingPolicy { get; set; }
        [JsonProperty(PropertyName = "destinations")]
        public List<TransactionIOput> Destinations { get; set; }
        [JsonProperty(PropertyName = "source")]
        public TransactionSource Source { get; set; }
        [JsonProperty(PropertyName = "spendingPassword")]
        public string SpendingPassword { get; set; }
    }

    public class TransactionSource
    {
        [JsonProperty(PropertyName = "accountIndex")]
        public UInt32 AccountIndex { get; set; }
        [JsonProperty(PropertyName = "walletId")]
        public string WalletId { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum GroupingPolicy
    {
        [EnumMember(Value = "OptimizeForSecurity")]
        OptimizeForSecurity,
        [EnumMember(Value = "OptimizeForHighThroughput")]
        OptimizeForHighThroughput
    }
}
