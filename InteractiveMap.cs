using System.Text.Json;
using System.Text.Json.Serialization;
using HoYoLab_API.Requests;
using HoYoLab_API.Responses;

namespace HoYoLab_API
{
    public class InteractiveMap
    {
        private readonly HoyolabAccount _hoyolabAccount;

        private DateTime _lastUpdateTime = DateTime.MinValue;
        private IReadOnlyList<MapObject>? _mapObjects = null;
        private IReadOnlyList<MapObjectLabel>? _objectLabels = null;

        internal InteractiveMap(HoyolabAccount hoyolabAccount)
        {
            _hoyolabAccount = hoyolabAccount;
        }

        public int MapCacheTime => 30;

        public IReadOnlyList<MapObject>? TeyvatPoints
        {
            get
            {
                if (_mapObjects == null || _lastUpdateTime.AddMinutes(MapCacheTime) < DateTime.Now)
                {
                    RequestObjectsList();
                }

                return _mapObjects;
            }
        }

        public IReadOnlyList<MapObjectLabel>? TeyvatPointLabels
        {
            get
            {
                if (_objectLabels == null || _lastUpdateTime.AddMinutes(MapCacheTime) < DateTime.Now)
                {
                    RequestObjectsList();
                }

                return _objectLabels;
            }
        }

        public string MarkedObjectsResponse = string.Empty;

        public bool TryMark(int objectId, out string response)
        {
            MapObject? foundObject = TeyvatPoints?.FirstOrDefault(x => x.Id == objectId);

            if (foundObject == null)
            {
                response = string.Empty;
                return false;
            }

            if (new MarkInteractiveMapObjectRequest(_hoyolabAccount, objectId).TrySend(out response) == false)
            {
                return false;
            }
            
            return true;
        }

        public bool TryRemoveMark(int objectId)
        {
            return false;
        }

        private void RequestObjectsList()
        {
            if (new InteractiveMapObjectsRequest(_hoyolabAccount).TrySend(out string response) == false)
            {
                return;
            }

            try
            {
                var deserializedMapObjects = JsonSerializer.Deserialize<InteractiveMapObjectsResponse>(response);

                if (deserializedMapObjects?.ReturnedData != null)
                {
                    _mapObjects = deserializedMapObjects.ReturnedData.Objects;
                    _objectLabels = deserializedMapObjects.ReturnedData.ObjectLabels;
                }
            }
            catch
            {
                return;
            }

            if (new MarkedMapObjectsRequest(_hoyolabAccount).TrySend(out string markedObjectsResponse) == false)
            {
                return;
            }

            try
            {
                var deserializedMapObjects = JsonSerializer.Deserialize<DefaultResponse>(markedObjectsResponse);

                if (deserializedMapObjects?.ReturnedData != null)
                {
                    MarkedObjectsResponse = markedObjectsResponse;
                }
            }
            catch
            {
                // ignored
            }

            _lastUpdateTime = DateTime.Now;
        }

        public class MapObject
        {
            [JsonPropertyName("id")]
            public int Id { get; init; }

            [JsonPropertyName("label_id")]
            public int LabelId { get; init; }

            [JsonPropertyName("x_pos")]
            public float PositionX { get; init; }

            [JsonPropertyName("y_pos")]
            public float PositionY { get; init; }

            public bool IsMarked { get; private set; }

            public override string ToString()
            {
                return $"{Id} ({PositionX};{PositionY})";
            }
        }

        public class MapObjectLabel
        {
            [JsonPropertyName("id")]
            public int Id { get; init; }

            [JsonPropertyName("name")]
            public string Name { get; init; }

            [JsonPropertyName("icon")]
            public string IconLink { get; init; }
        }

        public class MarkedPointList
        {
            [JsonPropertyName("list")]
            public IReadOnlyList<MarkedPoint> Points { get; init; }
        }

        public class MarkedPoint
        {
            [JsonPropertyName("point_id")]
            public int Id { get; init; }
        }
    }
}
