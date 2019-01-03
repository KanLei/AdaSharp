using System;
using System.Collections.Generic;
using System.Text;

namespace AdaSharp.Wallet.DTOs.Response
{
    public class NodeSettingsResponse
    {
        public string ProjectVersion { get; set; }
        public string GitRevision { get; set; }
        public SlotDuration SlotDuration { get; set; }
        public SoftwareInfo SoftwareInfo { get; set; }
    }

    public class SlotDuration
    {
        public ulong Quantity { get; set; }
        public string Unit { get; set; }
    }

    public class SoftwareInfo
    {
        public int Version { get; set; }
        public string ApplicationName { get; set; }
    }
}
