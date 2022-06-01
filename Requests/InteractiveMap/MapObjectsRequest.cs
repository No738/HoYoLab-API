namespace HoYoLab_API.Requests
{
    internal sealed class MapObjectsRequest : DefaultRequest
    {
        private static readonly string MapObjectsUrl = "https://sg-public-api-static.hoyolab.com/common/map_user/ys_obc/v1/map/point/list";

        public MapObjectsRequest(HoyolabAccount account, InteractiveMap.Location mapLocation) : base(account)
        {
            RequestMessage = new HttpRequestMessage(HttpMethod.Get,
                $"{MapObjectsUrl}?map_id={(int)mapLocation}&app_sn=ys_obc&lang={account.Language}");
        }

        protected override HttpRequestMessage RequestMessage { get; }
    }
}