using System.Text.Json;
using System.Text.Json.Serialization;

namespace HoYoLab_API.Responses
{
    internal sealed class MapObjectsResponse : DefaultResponse
    {
        [JsonPropertyName("data")]
        public new MapObjects ReturnedData { get; init; }

        internal class MapObjects
        {
            [JsonPropertyName("point_list")]
            public IReadOnlyList<MapObject> Objects { get; init; }

            [JsonPropertyName("label_list")]
            public IReadOnlyList<ObjectLabel> ObjectLabels { get; init; }
        }

        internal class MapObject
        {
            [JsonPropertyName("id")]
            public int Id { get; init; }

            [JsonPropertyName("label_id")]
            public int LabelId { get; init; }

            [JsonPropertyName("x_pos")]
            public float PositionX { get; init; }

            [JsonPropertyName("y_pos")]
            public float PositionY { get; init; }
        }

        internal class ObjectLabel
        {
            [JsonPropertyName("id")]
            public int Id { get; init; }

            [JsonPropertyName("name")]
            public string Name { get; init; }

            [JsonPropertyName("icon")]
            public string IconLink { get; init; }
        }
    }
}