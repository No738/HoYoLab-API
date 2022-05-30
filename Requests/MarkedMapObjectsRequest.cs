namespace HoYoLab_API.Requests
{
    internal sealed class MarkedMapObjectsRequest : DefaultRequest
    {
        private static readonly string MarkedMapObjectsUrl = "https://sg-public-api-static.hoyolab.com/common/map_user/ys_obc/v1/map/point/mark_map_point_list";

        public MarkedMapObjectsRequest(HoyolabAccount account) : base(account.AuthenticationData, account.UserAgent)
        {
            RequestMessage = new HttpRequestMessage(HttpMethod.Get,
                $"{MarkedMapObjectsUrl}?map_id=2&app_sn=ys_obc&lang={account.Language}");
        }

        protected override HttpRequestMessage RequestMessage { get; }
    }
}