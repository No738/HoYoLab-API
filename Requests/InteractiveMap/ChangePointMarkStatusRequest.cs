using System.Net.Http.Json;

namespace HoYoLab_API.Requests
{
    internal sealed class ChangePointMarkStatusRequest : DefaultRequest
    {
        private static readonly string MarkPointUrl = "https://sg-public-api-static.hoyolab.com/common/map_user/ys_obc/v1/map/point/add_mark_map_point";
        private static readonly string UnmarkPointUrl = "https://sg-public-api.hoyolab.com/common/map_user/ys_obc/v1/map/point/new_del_mark_map_point";

        public ChangePointMarkStatusRequest(HoyolabAccount account, InteractiveMap.Location mapLocation, int pointId, bool newStatus) : base(account)
        {
            RequestMessage = new HttpRequestMessage(HttpMethod.Post, newStatus ? MarkPointUrl : UnmarkPointUrl)
            {
                Content = JsonContent.Create(new
                    {map_id = (int) mapLocation, point_id = pointId, app_sn = "ys_obc", lang = account.Language})
            };
        }

        protected override HttpRequestMessage RequestMessage { get; }
    }
}