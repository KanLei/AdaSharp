using System;
using System.Collections.Generic;
using System.Text;

namespace AdaSharp.Wallet.DTOs.Response
{
    public class AccountResponse
    {
        public ulong Amount { get; set; }
        public List<AddressResponse> Addresses { get; set; }
        public string Name { get; set; }
        public string WalletId { get; set; }
        public ulong Index { get; set; }
    }
}
