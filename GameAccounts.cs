using System.Text.Json;
using System.Text.Json.Serialization;
using HoYoLab_API.Requests;
using HoYoLab_API.Responses;

namespace HoYoLab_API
{
    public class GameAccounts
    {
        private readonly HoyolabAccount _hoyolabAccount;

        private IReadOnlyList<GameAccount>? _cachedAccounts;
        private DateTime _lastCacheUpdate = DateTime.MinValue;

        internal GameAccounts(HoyolabAccount hoyolabAccount)
        {
            _hoyolabAccount = hoyolabAccount;
        }

        public static int CacheMinutes => 30;

        public IReadOnlyList<GameAccount>? Accounts
        {
            get
            {
                if (_cachedAccounts == null ||  _lastCacheUpdate.AddMinutes(CacheMinutes) < DateTime.Now)
                {
                    _cachedAccounts = GetGameAccounts();
                    _lastCacheUpdate = DateTime.Now;
                }

                return _cachedAccounts;
            }
        }

        private IReadOnlyList<GameAccount>? GetGameAccounts()
        {
            if (new GameAccountsRequest(_hoyolabAccount).TrySend(out string response) == false)
            {
                return null;
            }

            try
            {
                var deserializedGameAccounts = JsonSerializer.Deserialize<GameAccountsResponse>(response);

                if (deserializedGameAccounts?.ReturnedData != null)
                {
                    return deserializedGameAccounts.ReturnedData.Accounts;
                }
            }
            catch
            {
                // ignored
            }

            return null;
        }

        public readonly struct GameAccount
        {
            [JsonPropertyName("nickname")]
            public string Nickname { get; init; }

            [JsonPropertyName("region_name")]
            public string Region { get; init; }

            [JsonPropertyName("level")]
            public int Rank { get; init; }

            public override string ToString()
            {
                return $"{Nickname} [{Rank} rank] - {Region}";
            }
        }
    }
}
