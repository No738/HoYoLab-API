using System.Text.Json;
using System.Text.Json.Serialization;
using HoYoLab_API.Responses;

namespace HoYoLab_API
{
    public class CheckIn
    {
        private readonly HoyolabAccount _hoyolabAccount;
        private MonthlyRewards? _monthlyRewardsCache;

        internal CheckIn(HoyolabAccount hoyolabAccount)
        {
            _hoyolabAccount = hoyolabAccount;
        }

        public RewardInformation? CurrentRewardInformation => GetCurrentRewardInfo();

        public MonthlyRewards? CurrentMonthlyRewards
        {
            get
            {
                if (_monthlyRewardsCache == null || _monthlyRewardsCache.Month != DateTime.Now.Month)
                {
                    _monthlyRewardsCache = GetMonthlyRewards();
                }

                return _monthlyRewardsCache;
            }
        }

        private MonthlyRewards? GetMonthlyRewards()
        {
            if (new Requests.MonthlyRewardsRequest(_hoyolabAccount).TrySend(out string response) == false)
            {
                return null;
            }

            try
            {
                var deserializedMonthlyRewards = JsonSerializer.Deserialize<MonthlyRewardsResponse>(response);

                if (deserializedMonthlyRewards?.ReturnedData != null)
                {
                    return deserializedMonthlyRewards.ReturnedData;
                }
            }
            catch
            {
                // ignored
            }

            return null;
        }

        private RewardInformation? GetCurrentRewardInfo()
        {
            if (new Requests.RewardInformationRequest(_hoyolabAccount).TrySend(out string response) == false)
            {
                return null;
            }

            try
            {
                var deserializedMonthlyRewards = JsonSerializer.Deserialize<RewardInformationResponse>(response);

                if (deserializedMonthlyRewards?.ReturnedData != null)
                {
                    return deserializedMonthlyRewards.ReturnedData;
                }
            }
            catch
            {
                // ignored
            }

            return null;
        }

        public readonly struct RewardInformation
        {
            [JsonPropertyName("is_sign")]
            public bool IsClaimed { get; init; }

            [JsonPropertyName("total_sign_day")]
            public int SignedDays { get; init; }

            public override string ToString()
            {
                return $"Is already claimed: {IsClaimed}\n" +
                       $"Total signed days: {SignedDays}";
            }
        }

        public class MonthlyRewards
        {
            [JsonPropertyName("month")]
            public int Month { get; init; }

            [JsonPropertyName("awards")]
            public IReadOnlyList<Reward> Rewards { get; init; }

            public override string ToString()
            {
                return $"Month: {Month}\n" +
                       $"Rewards: {string.Join(", ", Rewards)}";
            }

            public readonly struct Reward
            {
                [JsonPropertyName("name")]
                public string Name { get; init; }

                [JsonPropertyName("cnt")]
                public int Count { get; init; }

                public override string ToString()
                {
                    return $"{Name} - {Count}";
                }
            }
        }
    }
}
