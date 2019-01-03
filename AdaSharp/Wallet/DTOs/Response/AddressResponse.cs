using System;
using System.Collections.Generic;
using System.Text;

namespace AdaSharp.Wallet.DTOs.Response
{
    public class AddressResponse
    {
        public bool Used { get; set; }
        public bool ChangeAddress { get; set; }
        public string Id { get; set; }
        public string Ownership { get; set; }
    }
}
