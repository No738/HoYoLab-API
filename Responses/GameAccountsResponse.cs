using System.Text.Json.Serialization;

namespace HoYoLab_API.Responses
{
    internal sealed class GameAccountsResponse : DefaultResponse
    {
        [JsonPropertyName("data")]
        public new AccountsList? ReturnedData { get; init; }

        internal class AccountsList
        {
            [JsonPropertyName("list")]
            public IReadOnlyList<GameAccounts.GameAccount> Accounts { get; init; }
        }
    }
}