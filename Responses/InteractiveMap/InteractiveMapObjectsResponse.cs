using System.Text.Json.Serialization;

namespace HoYoLab_API.Responses
{
    internal sealed class InteractiveMapObjectsResponse : DefaultResponse
    {
        [JsonPropertyName("data")]
        public new MapObjects? ReturnedData { get; init; }

        internal class MapObjects
        {
            [JsonPropertyName("point_list")]
            public IReadOnlyList<InteractiveMap.MapObject>? Objects { get; init; }

            [JsonPropertyName("label_list")]
            public IReadOnlyList<InteractiveMap.MapObjectLabel>? ObjectLabels { get; init; }
        }
    }
}