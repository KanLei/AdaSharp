using AdaSharp.Wallet.DTOs.Request;
using AdaSharp.Wallet.DTOs.Response;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AdaSharp.Wallet
{
    public class AdaWalletService
    {
        private readonly string baseUrl;
        private readonly ApiClient apiClient;

        public string WalletId { get; set; }

        public AdaWalletService(string url, string certificatePath, string certificatePassword, bool authServerCert = false)
        {
            this.baseUrl = url;
            var handler = new HttpClientHandler();
            if (!authServerCert)
                handler.ServerCertificateCustomValidationCallback = (message, cert, chain, error) => true;
            if (!string.IsNullOrWhiteSpace(certificatePath))
                handler.ClientCertificates.Add(LoadClientCertificate(certificatePath, certificatePassword));
            apiClient = new ApiClient(handler);
        }

        #region Wallet

        public async Task<ApiResult<List<WalletResponse>>> GetAllWalletsAsync(UInt32 page = 1, UInt32 perPage = 10)
        {
            var url = $"{baseUrl}/api/v1/wallets";
            return await apiClient.GetAsync<ApiResult<List<WalletResponse>>>(url).ConfigureAwait(false);
        }

        public async Task<ApiResult<List<WalletResponse>>> GetWalletsByFilterAsync(UInt32 page = 1,
            UInt32 perPage = 10,
            string id = "",
            FilterOperator idOperator = FilterOperator.None,
            ulong balance = 0,
            FilterOperator balanceOperator = FilterOperator.None)
        {
            var url = $"{baseUrl}/api/v1/wallets";
            var nameValues = new List<KeyValuePair<string, string>>();
            if (!string.IsNullOrEmpty(id) && idOperator != FilterOperator.None)
            {
                nameValues.Add(new KeyValuePair<string, string>("id", $"{idOperator.GetName()}[{id}]"));
            }
            if ((balance >= 0) && balanceOperator != FilterOperator.None)
                nameValues.Add(new KeyValuePair<string, string>("balance", $"{balanceOperator.GetName()}[{balance}]"));

            var query = await (new FormUrlEncodedContent(nameValues).ReadAsStringAsync());
            if (!string.IsNullOrWhiteSpace(query))
                url += "?" + query;
            return await apiClient.GetAsync<ApiResult<List<WalletResponse>>>(url).ConfigureAwait(false);
        }

        public async Task<ApiResult<WalletResponse>> GetWalletAsync(string walletId)
        {
            var url = $"{baseUrl}/api/v1/wallets/{walletId}";
            return await apiClient.GetAsync<ApiResult<WalletResponse>>(url).ConfigureAwait(false);
        }

        public async Task<ApiResult<WalletResponse>> CreateWalletAsync(List<string> backupPhrase, AssuranceLevel assuranceLevel, string name, string spendingPassword = "")
        {
            var data = new WalletCreatedRequest()
            {
                Operation = "create",
                BackupPhrase = backupPhrase,
                AssuranceLevel = assuranceLevel.GetName().ToLower(),
                Name = name,
                SpendingPassword = spendingPassword
            };

            var url = $"{baseUrl}/api/v1/wallets";

            return await apiClient.PostAsync<ApiResult<WalletResponse>>(url, data).ConfigureAwait(false);
        }

        public async Task<ApiResult<WalletResponse>> RestoreWalletAsync(List<string> backupPhrase, string assuranceLevel, string name, string spendingPassword = "")
        {
            var url = $"{baseUrl}/api/v1/wallets";
            var data = new WalletCreatedRequest()
            {
                Operation = "restore",
                BackupPhrase = backupPhrase,
                AssuranceLevel = assuranceLevel,
                Name = name,
                SpendingPassword = spendingPassword
            };
            return await apiClient.PostAsync<ApiResult<WalletResponse>>(url, data).ConfigureAwait(false);
        }

        public async Task<ApiResult<WalletResponse>> UpdateWalletPasswordAsync(string walletId, string old, string @new)
        {
            var url = $"{baseUrl}/api/v1/wallets/{walletId}/password";
            var result = await apiClient.PutAsync<ApiResult<WalletResponse>>(url, new { old = old, @new = @new }).ConfigureAwait(false);
            return result;
        }

        public async Task<ApiResult<WalletResponse>> UpdateWalletIdentifier(string walletId, AssuranceLevel assuranceLevel, string name)
        {
            var url = $"{baseUrl}/api/v1/wallets/{walletId}";
            var data = new { assuranceLevel = assuranceLevel.GetName().ToLower(), name = name };
            var result = await apiClient.PutAsync<ApiResult<WalletResponse>>(url, data).ConfigureAwait(false);
            return result;
        }

        public async Task DeleteWalletAsync(string walletId)
        {
            var url = $"{baseUrl}/api/v1/wallets/{walletId}";
            await apiClient.DeleteAsync(url).ConfigureAwait(false);
        }

        #endregion

        #region Account

        public async Task<ApiResult<List<AccountResponse>>> GetAllAccountsAsync(string walletId, ulong page = 1, uint perPage = 10)
        {
            var url = $"{baseUrl}/api/v1/wallets/{walletId}/accounts";
            var accounts = await apiClient.GetAsync<ApiResult<List<AccountResponse>>>(url).ConfigureAwait(false);
            return accounts;
        }

        public async Task<ApiResult<AccountResponse>> GetAccountAsync(string walletId, string accountId)
        {
            var url = $"{baseUrl}/api/v1/wallets/{walletId}/accounts/{accountId}";
            var account = await apiClient.GetAsync<ApiResult<AccountResponse>>(url).ConfigureAwait(false);
            return account;
        }

        public async Task<ApiResult<BalanceResponse>> GetAccountBalanceAsync(string walletId, UInt32 accountId)
        {
            var url = $"{baseUrl}/api/v1/wallets/{walletId}/accounts/{accountId}/amount";
            var balance = await apiClient.GetAsync<ApiResult<BalanceResponse>>(url).ConfigureAwait(false);
            return balance;
        }

        public async Task<ApiResult<AccountResponse>> CreateAccountAsync(string walletId, string name, string spendingPassword = "")
        {
            var url = $"{baseUrl}/api/v1/wallets/{walletId}/accounts";
            var data = new AccountCreatedRequest
            {
                Name = name,
                SpendingPassword = spendingPassword
            };
            var account = await apiClient.PostAsync<ApiResult<AccountResponse>>(url, data).ConfigureAwait(false);
            return account;
        }

        public async Task<ApiResult<AccountResponse>> UpdateAccountAsync(string walletId, string accountId, string name)
        {
            var url = $"{baseUrl}/api/v1/wallets/{walletId}/accounts/{accountId}";
            var account = await apiClient.PutAsync<ApiResult<AccountResponse>>(url, new { name = name }).ConfigureAwait(false);
            return account;
        }

        public async Task DeleteAccountAsync(string walletId, string accountId)
        {
            var url = $"{baseUrl}/api/v1/wallets/{walletId}/accounts/{accountId}";
            await apiClient.DeleteAsync(url).ConfigureAwait(false);
        }

        #endregion

        #region Address
        public async Task<ApiResult<List<AddressResponse>>> GetAllAddressesAsync(ulong page = 1, uint perPage = 10)
        {
            if (page < 1)
                throw new ArgumentOutOfRangeException($"{nameof(page)} cannot be less than 1");

            if (perPage > 50)
                throw new ArgumentOutOfRangeException($"{nameof(perPage)} cannot be greater than 50");

            var url = $"{baseUrl}/api/v1/addresses?page={page}&per_page={perPage}";
            return await apiClient.GetAsync<ApiResult<List<AddressResponse>>>(url).ConfigureAwait(false);
        }

        public async Task<ApiResult<AddressResponse>> GetAddressAsync(string address)
        {
            var url = $"{baseUrl}/api/v1/addresses/{address}";
            var addressInfo = await apiClient.GetAsync<ApiResult<AddressResponse>>(url).ConfigureAwait(false);
            return addressInfo;
        }

        public async Task<ApiResult<AccountAddressesResponse>> GetAddressesByAccount(string walletId, UInt32 accountIndex)
        {
            var url = $"{baseUrl}/api/v1/wallets/{walletId}/accounts/{accountIndex}/addresses";
            var addresses = await apiClient.GetAsync<ApiResult<AccountAddressesResponse>>(url).ConfigureAwait(false);
            return addresses;
        }

        public async Task<ApiResult<AddressResponse>> CreateAddressAsync(string walletId, UInt32 accountIndex, string spendingPassword = "")
        {
            // hardened key:  i > 2**31 - 1
            // non-hardened key: i <= 2**31 - 1
            if (accountIndex <= Int32.MaxValue || accountIndex > UInt32.MaxValue)
                throw new ArgumentOutOfRangeException($"{nameof(accountIndex)} should be in [2147483648 .. 4294967295]");

            var url = $"{baseUrl}/api/v1/addresses";
            var data = new AddressCreatedRequest
            {
                WalletId = walletId,
                AccountIndex = accountIndex,
                SpendingPassword = spendingPassword
            };
            var result = await apiClient.PostAsync<ApiResult<AddressResponse>>(url, data).ConfigureAwait(false);
            return result;
        }

        #endregion


        #region Transaction History

        public async Task<ApiResult<List<TransactionResponse>>> GetAllTransactionsAsync()
        {
            var url = $"{baseUrl}/api/v1/transactions";
            var result = await apiClient.GetAsync<ApiResult<List<TransactionResponse>>>(url).ConfigureAwait(false);
            return result;
        }

        public async Task<ApiResult<List<TransactionResponse>>> GetTransactionsByFilterAsync(string walletId = "",
         UInt32 accountIndex = 0,
         string address = "",
         ulong page = 1,
         uint perPage = 10,
         string id = "",
         FilterOperator idOperator = FilterOperator.None,
         DateTime createdAt = default(DateTime),
         FilterOperator createAtOperator = FilterOperator.None,
         SortByOperator sortByCreateAt = SortByOperator.DES)
        {
            var url = $"{baseUrl}/api/v1/transactions";

            var nameValues = new List<KeyValuePair<string, string>>();
            if (!string.IsNullOrWhiteSpace(walletId))
                nameValues.Add(new KeyValuePair<string, string>("wallet_id", walletId));
            if (accountIndex != 0 && !string.IsNullOrWhiteSpace(walletId))
                nameValues.Add(new KeyValuePair<string, string>("account_index", accountIndex.ToString()));
            if (!string.IsNullOrWhiteSpace(address))
                nameValues.Add(new KeyValuePair<string, string>("address", address));
            if (page != 1)
                nameValues.Add(new KeyValuePair<string, string>("page", page.ToString()));
            if (perPage != 10)
                nameValues.Add(new KeyValuePair<string, string>("per_page", perPage.ToString()));
            if (!string.IsNullOrWhiteSpace(id) && idOperator != FilterOperator.None)
            {
                string ops = $"{idOperator.GetName()}[{id}]";
                nameValues.Add(new KeyValuePair<string, string>("id", ops));
            }
            if (createdAt != default(DateTime) && createAtOperator != FilterOperator.None)
            {
                string ops = $"{createAtOperator.GetName()}[{createdAt.ToString("s")}]";
                nameValues.Add(new KeyValuePair<string, string>("created_at", ops));
                nameValues.Add(new KeyValuePair<string, string>("sort_by", $"{sortByCreateAt.GetName()}[created_at]"));
            }

            var query = await (new FormUrlEncodedContent(nameValues).ReadAsStringAsync());
            if (!string.IsNullOrWhiteSpace(query))
                url += "?" + query;
            var result = await apiClient.GetAsync<ApiResult<List<TransactionResponse>>>(url).ConfigureAwait(false);
            return result;
        }

        public async Task<ApiResult<TransactionResponse>> CreateTransactionAsync(TransactionCreatedRequest data)
        {
            if (data == null || data.Source == null)
                throw new ArgumentNullException($"transaction source cannot be null");
            if (data == null || data.Destinations == null || !data.Destinations.Any())
                throw new ArgumentNullException($"transaction destinations cannot be null");

            var url = $"{baseUrl}/api/v1/transactions";
            var result = await apiClient.PostAsync<ApiResult<TransactionResponse>>(url, data).ConfigureAwait(false);
            return result;
        }

        public async Task<ApiResult<EstimatedTransactionFeeResponse>> EstimateTransactionFeeAsync(TransactionCreatedRequest data)
        {
            if (data == null || data.Source == null)
                throw new ArgumentNullException($"transaction source cannot be null");
            if (data == null || data.Destinations == null || !data.Destinations.Any())
                throw new ArgumentNullException($"transaction destinations cannot be null");

            var url = $"{baseUrl}/api/v1/transactions/fees";
            var result = await apiClient.PostAsync<ApiResult<EstimatedTransactionFeeResponse>>(url, data).ConfigureAwait(false);
            return result;
        }

        public async Task<ApiResult<TransactionResponse>> RedeemCertificateAsync(UInt32 accountIndex, string redemptionCode, string walletId, string spendingPassword, string mnemonic = "")
        {
            var url = $"{baseUrl}/api/v1/transactions/certificates";
            var data = new
            {
                mnemonic = mnemonic,
                accountIndex = accountIndex,
                redemptionCode = redemptionCode,
                walletId = walletId,
                spendingPassword = spendingPassword
            };
            var result = await apiClient.PostAsync<ApiResult<TransactionResponse>>(url, data).ConfigureAwait(false);
            return result;
        }

        #endregion


        #region Node Setting

        public async Task<ApiResult<NodeSettingsResponse>> GetNodeSettingsAsync()
        {
            var url = $"{baseUrl}/api/v1/node-settings";
            var result = await apiClient.GetAsync<ApiResult<NodeSettingsResponse>>(url).ConfigureAwait(false);
            return result;
        }

        public async Task<ApiResult<NodeResponse>> GetNodeInfoAsync(bool forceNTPCheck = false)
        {
            var url = $"{baseUrl}/api/v1/node-info?force_ntp_check={forceNTPCheck}";
            return await apiClient.GetAsync<ApiResult<NodeResponse>>(url).ConfigureAwait(false);
        }


        #endregion

        #region Internal

        public async Task<(string applicationName, uint version)> GetNextUpdateVersionAsync()
        {
            var url = $"{baseUrl}/api/internal/next-update";
            var result = await apiClient.GetAsync<(string, uint)>(url).ConfigureAwait(false);
            return result;
        }

        public async Task ClearWalletAsync()
        {
            var url = $"{baseUrl}/api/internal/reset-wallet-state";
            await apiClient.DeleteAsync(url).ConfigureAwait(false);
        }

        public async Task<ApiResult<WalletResponse>> ImportWalletAsync(string keyPath, string spendingPassword)
        {
            if (!File.Exists(keyPath))
                throw new FileNotFoundException(keyPath);

            var url = $"{baseUrl}/api/internal/import-wallet";
            var data = new { filePath = keyPath, spendingPassword = spendingPassword };
            var result = await apiClient.PostAsync<ApiResult<WalletResponse>>(url, data).ConfigureAwait(false);
            return result;
        }

        #endregion

        private X509Certificate LoadClientCertificate(string path, string password)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException(path);

            var certificate = new X509Certificate2(path, password);
            return certificate;
        }
    }

}
