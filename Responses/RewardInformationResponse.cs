using System.Text.Json.Serialization;

namespace HoYoLab_API.Responses
{
    internal sealed class RewardInformationResponse : DefaultResponse
    {
        [JsonPropertyName("data")]
        public new CheckIn.RewardInformation ReturnedData { get; init; }
    }
}