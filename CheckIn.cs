using System.Text.Json;
using System.Text.Json.Serialization;
using HoYoLab_API.Requests;
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

        public enum ClaimResult
        {
            Success,
            AuthenticationError,
            DeserializeError,
            AlreadyClaimed,
            UnknownResultCode
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

        public ClaimResult ClaimReward()
        {
            new ClaimRewardRequest(_hoyolabAccount).TrySend(out string response);

            try
            {
                var deserializedCurrentReward = JsonSerializer.Deserialize<DefaultResponse>(response);

                if (deserializedCurrentReward == null)
                {
                    return ClaimResult.DeserializeError;
                }

                return deserializedCurrentReward.ResultCode switch
                {
                    0 => ClaimResult.Success,
                    -5003 => ClaimResult.AlreadyClaimed,
                    -100 => ClaimResult.AuthenticationError,
                    _ => ClaimResult.UnknownResultCode
                };
            }
            catch
            {
                return ClaimResult.DeserializeError;
            }
        }

        private MonthlyRewards? GetMonthlyRewards()
        {
            if (new MonthlyRewardsRequest(_hoyolabAccount).TrySend(out string response) == false)
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
            if (new RewardInformationRequest(_hoyolabAccount).TrySend(out string response) == false)
            {
                return null;
            }

            try
            {
                var deserializedCurrentReward = JsonSerializer.Deserialize<RewardInformationResponse>(response);

                if (deserializedCurrentReward?.ReturnedData != null)
                {
                    return deserializedCurrentReward.ReturnedData;
                }
            }
            catch
            {
                // ignored
            }

            return null;
        }

        public class RewardInformation
        {
            [JsonPropertyName("is_sign")]
            public bool IsClaimed { get; init; }

            /// <summary>
            /// Total signed days count (Increment, when reward has claimed. If next reward day number is 23, value will be 22)
            /// </summary>
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
                /// <summary>
                /// Name of reward item
                /// </summary>
                [JsonPropertyName("name")]
                public string Name { get; init; }

                /// <summary>
                /// Count of reward items
                /// </summary>
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
