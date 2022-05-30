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

        public IReadOnlyList<MapObject>? MapObjects
        {
            get
            {
                if (_mapObjects == null || _lastUpdateTime.AddMinutes(MapCacheTime) < DateTime.Now)
                {
                    RequestObjectsList();
                }

                return _mapObjects;
            }
            private set => _mapObjects = value;
        }

        public IReadOnlyList<MapObjectLabel>? ObjectLabels
        {
            get
            {
                if (_objectLabels == null || _lastUpdateTime.AddMinutes(MapCacheTime) < DateTime.Now)
                {
                    RequestObjectsList();
                }

                return _objectLabels;
            }
            private set => _objectLabels = value;
        }

        public bool TryMark(int objectId, out string response)
        {
            MapObject? foundObject = MapObjects?.FirstOrDefault(x => x.Id == objectId);

            if (foundObject == null)
            {
                response = string.Empty;
                return false;
            }

            if (new MarkInteractiveMapObjectRequest(_hoyolabAccount, objectId).TrySend(out response) == false)
            {
                return false;
            }
            
            foundObject.DisplayState = 1;
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
                    _lastUpdateTime = DateTime.Now;

                    MapObjects = deserializedMapObjects.ReturnedData.Objects;
                    ObjectLabels = deserializedMapObjects.ReturnedData.ObjectLabels;
                }
            }
            catch
            {
                // ignored
            }
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

            [JsonPropertyName("display_state")]
            public int DisplayState { get; internal set; }

            public override string ToString()
            {
                return $"{Id} ({PositionX};{PositionY})\n" +
                       $"Display state: {DisplayState}";
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
    }
}
