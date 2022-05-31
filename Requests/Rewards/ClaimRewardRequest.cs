namespace HoYoLab_API.Requests
{
    internal sealed class ClaimRewardRequest : DefaultRequest
    {
        private static readonly string AccountInfoUrl = "https://hk4e-api-os.mihoyo.com/event/sol/sign";

        public ClaimRewardRequest(HoyolabAccount account) : base(account.AuthenticationData, account.UserAgent)
        {
            RequestMessage = new HttpRequestMessage(HttpMethod.Post,
                $"{AccountInfoUrl}?act_id=e202102251931481&lang={account.Language}&{AdditionalMetaParameters}");
        }

        protected override HttpRequestMessage RequestMessage { get; }
    }
}