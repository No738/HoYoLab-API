using System.Net.Http.Json;

namespace HoYoLab_API.Requests
{
    internal sealed class MarkInteractiveMapObjectRequest : DefaultRequest
    {
        private static readonly string MapObjectsUrl = "https://sg-public-api-static.hoyolab.com/common/map_user/ys_obc/v1/map/point/add_mark_map_point";
       
        public MarkInteractiveMapObjectRequest(HoyolabAccount account, InteractiveMap.Location mapLocation, int pointId) : base(account)
        {
            RequestMessage = new HttpRequestMessage(HttpMethod.Post,
                $"{MapObjectsUrl}");

            RequestMessage.Content = JsonContent.Create(new { map_id = (int)mapLocation, point_id = pointId, app_sn = "ys_obc", lang = account.Language });
        }

        protected override HttpRequestMessage RequestMessage { get; }
    }
}