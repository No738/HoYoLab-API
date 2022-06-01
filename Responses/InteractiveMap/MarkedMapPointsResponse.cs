using System.Text.Json.Serialization;

namespace HoYoLab_API.Responses
{
    internal sealed class MarkedMapPointsResponse : DefaultResponse
    {
        [JsonPropertyName("data")]
        public new MarkedPointsList? ReturnedData { get; init; }

        internal class MarkedPointsList
        {
            [JsonPropertyName("list")]
            public IReadOnlyList<MarkedPoint> Points { get; init; }
        }
        internal class MarkedPoint
        {
            [JsonPropertyName("point_id")]
            public int Id { get; init; }

            [JsonPropertyName("map_id")]
            public InteractiveMap.Location Location { get; init; }
        }
    }
}