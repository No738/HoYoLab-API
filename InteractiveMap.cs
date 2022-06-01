using System.Text.Json;
using HoYoLab_API.Requests;
using HoYoLab_API.Responses;

namespace HoYoLab_API
{
    public class InteractiveMap
    {
        private readonly HoyolabAccount _hoyolabAccount;
        private List<MapPoint>? _mapPoints;

        public InteractiveMap(HoyolabAccount hoyolabAccount, Location mapLocation)
        {
            _hoyolabAccount = hoyolabAccount;
            MapLocation = mapLocation;
        }

        public enum Location
        {
            Teyvat = 2,
            Enkanomiya = 7,
            Chasm = 9
        }

        public Location MapLocation { get; }

        public IReadOnlyList<MapPoint>? Points
        {
            get
            {
                if (new MapObjectsRequest(_hoyolabAccount, MapLocation)
                        .TrySend(out string response) == false)
                {
                    return null;
                }

                try
                {
                    var deserializedMapObjectsResponse =
                        JsonSerializer.Deserialize<MapObjectsResponse>(response);

                    if (deserializedMapObjectsResponse == null)
                    {
                        return null;
                    }

                    _mapPoints = new List<MapPoint>(deserializedMapObjectsResponse.ReturnedData.Objects.Count);

                    foreach (MapObjectsResponse.MapObject mapObject
                             in deserializedMapObjectsResponse.ReturnedData?.Objects!)
                    {
                        foreach (MapObjectsResponse.ObjectLabel label
                                 in deserializedMapObjectsResponse.ReturnedData.ObjectLabels)
                        {
                            if (label.Id != mapObject.LabelId)
                            {
                                continue;
                            }

                            var mapPoint = new MapPoint(mapObject.Id, mapObject.PositionX, 
                                mapObject.PositionY, label.Name, label.IconLink);

                            _mapPoints.Add(mapPoint);
                        }
                    }

                    UpdateMarkedPoints();

                    return _mapPoints;
                }
                catch
                {
                    // ignored
                }
                
                return null;
            }
        }

        public bool TryMarkPoint(MapPoint point, out string response)
        {
            response = string.Empty;

            if (point.IsMarked)
            {
                return true;
            }

            if (new MarkMapObjectRequest(_hoyolabAccount, MapLocation, point.Id)
                    .TrySend(out response) == false)
            {
                return false;
            }

            return true;
        }

        public bool TryUnmark(MapPoint point, out string response)
        {
            response = string.Empty;

            if (point.IsMarked)
            {
                return true;
            }

            if (new MarkMapObjectRequest(_hoyolabAccount, MapLocation, point.Id)
                    .TrySend(out response) == false)
            {
                return false;
            }

            return true;
        }

        private void UpdateMarkedPoints()
        {
            if (new MarkedMapObjectsRequest(_hoyolabAccount, MapLocation)
                    .TrySend(out string response) == false)
            {
                return;
            }

            try
            {
                var deserializedMarkedPointsResponse =
                    JsonSerializer.Deserialize<MarkedMapPointsResponse>(response);

                if (deserializedMarkedPointsResponse == null)
                {
                    return;
                }

                foreach (MapPoint point in _mapPoints)
                {
                    if (deserializedMarkedPointsResponse.ReturnedData?.Points.FirstOrDefault(x =>
                            x.Location == MapLocation && x.Id == point.Id) == null)
                    {
                        point.IsMarked = false;
                        continue;
                    }

                    point.IsMarked = true;
                }
            }
            catch
            {
                // ignored
            }
        }

        public class MapPoint
        {
            internal MapPoint(int id, float positionX, float positionY, string name, string iconLink)
            {
                Id = id;
                PositionX = positionX;
                PositionY = positionY;
                Name = name;
                IconLink = iconLink;
            }

            public int Id { get; }
            public float PositionX { get; }
            public float PositionY { get; }
            public string Name { get; }
            public string IconLink { get; }
            public bool IsMarked { get; internal set; }

            public override string ToString()
            {
                return $"{Id} - {Name} ({PositionX};{PositionY})";
            }
        }
    }
}
