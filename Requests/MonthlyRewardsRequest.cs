namespace HoYoLab_API.Requests
{
    internal sealed class MonthlyRewardsRequest : DefaultRequest
    {
        private static readonly string AccountInfoUrl = "https://sg-hk4e-api.hoyolab.com/event/sol/home";

        public MonthlyRewardsRequest(HoyolabAccount account) :
            base(account.AuthenticationData, account.UserAgent)
        {
            RequestMessage = new HttpRequestMessage(HttpMethod.Get,
                $"{AccountInfoUrl}?act_id=e202102251931481&lang={account.Language}&{AdditionalMetaParameters}");
        }

        protected override HttpRequestMessage RequestMessage { get; }
    }
}