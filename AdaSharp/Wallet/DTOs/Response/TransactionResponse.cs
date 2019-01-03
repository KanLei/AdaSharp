using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AdaSharp.Wallet.DTOs.Response
{
    public class TransactionResponse
    {
        public DateTime CreationTime { get; set; }
        public TransactionStatus Status { get; set; }

        public ulong Amount { get; set; }
        public List<TransactionIOput> Inputs { get; set; }
        public TransactionDirection Direction { get; set; }
        public List<TransactionIOput> Outputs { get; set; }
        public ulong Confirmations { get; set; }
        public string Id { get; set; }
        public TransactionType Type { get; set; }
    }

    public class TransactionStatus
    {
        public TransactionStatusTag Tag { get; set; }
        public object Data { get; set; }
    }

    /// <summary>
    /// Optional strategy to use for selecting the transaction inputs.
    /// OptimizeForSecurity use more UTXO for input
    /// </summary>
    public enum GroupingPolicy
    {
        OptimizeForSecurity, OptimizeForHighThroughput
    }

    /// <summary>
    /// Whether the transaction is entirely local or foreign.
    /// local means from local to local, otherwise foreign
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TransactionType
    {
        [EnumMember(Value = "local")]
        Local,
        [EnumMember(Value = "foreign")]
        Foreign
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum TransactionDirection
    {
        [EnumMember(Value = "incoming")]
        Incoming,
        [EnumMember(Value = "outgoing")]
        Outgoing
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum TransactionStatusTag
    {
        [EnumMember(Value = "applying")]
        Applying,
        [EnumMember(Value = "inNewestBlocks")]
        InNewestBlocks,
        [EnumMember(Value = "persisted")]
        Persisted,
        [EnumMember(Value = "wontApply")]
        WontApply,
        [EnumMember(Value = "creating")]
        Creating
    }
}
