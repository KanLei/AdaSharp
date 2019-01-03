using System;
using System.Linq;
using AdaSharp;
using AdaSharp.Wallet;
using Newtonsoft.Json;

namespace AdaSharpTest
{
    class Program
    {
        static void Main(string[] args)
        {

            var service = new AdaWalletService("https://172.18.1.219:8090", "", "", false);
            var tx = service.GetTransactionsByFilterAsync("2cWKMJemoBak8MPHidBtVJG63rD76dD21vTk4dLzxoajfZSEgrLRuPTXuwe8M8vvdJCnG", 2147483648, id: "0f7baf6ba6827adf37b0d0be2c408b9745f35670c415af906fd74a560d65436b", idOperator: AdaSharp.Wallet.DTOs.Request.FilterOperator.EQ).Result;

            //var json = JsonConvert.SerializeObject(tx, Formatting.Indented);
            //Console.WriteLine(json);

            Console.WriteLine(tx.Data.Any());

            Console.ReadLine();
        }
    }
}
