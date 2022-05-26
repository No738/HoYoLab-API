using System.Text.Json.Serialization;

namespace HoYoLab_API.Responses
{
    internal sealed class RewardInfoResponse : DefaultResponse
    {
        [JsonPropertyName("data")]
        public new CheckIn.RewardInfo ReturnedData { get; init; }
    }
}