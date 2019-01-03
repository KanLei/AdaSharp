using System;
using System.Collections.Generic;
using System.Text;

namespace AdaSharp.Wallet.DTOs.Response
{
    public class NodeResponse
    {
        public SyncProgress SyncProgress { get; set; }
        public BlockchainHeight BlockchainHeight { get; set; }
        public BlockchainHeight LocalBlockchainHeight { get; set; }
        public LocalTimeInformation LocalTimeInformation { get; set; }
        public Dictionary<string, string> SubscriptionStatus { get; set; }
    }

    public class SyncProgress
    {
        public int Quantity { get; set; }
        public string Unit { get; set; }
    }

    public class BlockchainHeight
    {
        public ulong Quantity { get; set; }
        public string Unit { get; set; }
    }

    public class LocalTimeInformation
    {
        public DifferenceFromNtpServer DifferenceFromNtpServer { get; set; }
    }

    public class DifferenceFromNtpServer
    {
        public long Quantity { get; set; }
        public string Unit { get; set; }
    }
}
