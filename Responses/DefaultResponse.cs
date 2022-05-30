using System.Text.Json.Serialization;

namespace HoYoLab_API.Responses
{
    internal class DefaultResponse
    {
        [JsonPropertyName("retcode")]
        public int ResultCode { get; init; }

        [JsonPropertyName("message")]
        public string ResultMessage { get; init; }

        [JsonPropertyName("data")]
        public object ReturnedData { get; init; }
    }
}