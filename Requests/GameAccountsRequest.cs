namespace HoYoLab_API.Requests
{
    internal sealed class GameAccountsRequest : DefaultRequest
    {
        private static readonly string AccountInfoUrl = "https://api-os-takumi.mihoyo.com/binding/api/getUserGameRolesByCookie";

        public GameAccountsRequest(HoyolabAccount account) : base(account.AuthenticationData,
            account.UserAgent)
        {
            RequestMessage = new HttpRequestMessage(HttpMethod.Get,
                $"{AccountInfoUrl}?lang={account.Language}&{AdditionalMetaParameters}");
        }

        protected override HttpRequestMessage RequestMessage { get; }
    }
}