namespace HoYoLab_API.Requests
{
    internal sealed class InteractiveMapObjectsRequest : DefaultRequest
    {
        private static readonly string MapObjectsUrl = "https://sg-public-api-static.hoyolab.com/common/map_user/ys_obc/v1/map/point/list";

        public InteractiveMapObjectsRequest(HoyolabAccount account) : base(account.AuthenticationData, account.UserAgent)
        {
            RequestMessage = new HttpRequestMessage(HttpMethod.Get,
                $"{MapObjectsUrl}?map_id=2&app_sn=ys_obc&lang={account.Language}");
        }

        protected override HttpRequestMessage RequestMessage { get; }
    }
}