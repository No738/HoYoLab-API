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
            DeserializeError = int.MinValue,
            AlreadyClaimed = -5003,
            AuthenticationError = -100,
            Success = 0,
            UnknownResultCode = int.MaxValue
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
            if (new ClaimRewardRequest(_hoyolabAccount).TrySend(out string response) == false)
            {
                return ClaimResult.AuthenticationError;
            }

            try
            {
                var deserializedClaimResponse = JsonSerializer.Deserialize<DefaultResponse>(response);

                if (deserializedClaimResponse == null)
                {
                    return ClaimResult.DeserializeError;
                }

                if (Enum.IsDefined(typeof(ClaimResult), deserializedClaimResponse.ResultCode) == false)
                {
                    return ClaimResult.UnknownResultCode;
                }

                return (ClaimResult) deserializedClaimResponse.ResultCode;
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

            public class Reward
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
