using System;
using System.Collections.Generic;
using System.Text;

namespace AdaSharp.Wallet.DTOs.Response
{
    public class WalletResponse
    {
        public string CreatedAt { get; set; }
        public SyncState SyncState { get; set; }
        public long Balance { get; set; }
        public bool HasSpendingPassword { get; set; }
        public string AssuranceLevel { get; set; }
        public string Name { get; set; }
        public string Id { get; set; }
        public string SpendingPasswordLastUpdate { get; set; }
        public string Type { get; set; }
    }

    public class SyncState
    {
        public string Tag { get; set; }
        public SyncStateData Data { get; set; }
    }

    public class SyncStateData
    {
        public SyncStateModel Throughput { get; set; }
        public SyncStateModel Percentage { get; set; }
        public SyncStateModel EstimatedCompletionTime { get; set; }
    }

    public class SyncStateModel
    {
        public int Quantity { get; set; }
        public string Unit { get; set; }
    }

    public enum AssuranceLevel
    {
        Normal, Strict
    }
}
