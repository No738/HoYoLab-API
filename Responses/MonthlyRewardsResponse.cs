using System.Text.Json.Serialization;

namespace HoYoLab_API.Responses;

internal class MonthlyRewardsResponse : DefaultResponse
{
    [JsonPropertyName("data")]
    public new CheckIn.MonthlyRewards? ReturnedData { get; init; }
}