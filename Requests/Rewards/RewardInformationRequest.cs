namespace HoYoLab_API.Requests
{
    internal sealed class RewardInformationRequest : DefaultRequest
    {
        private static readonly string AccountInfoUrl = "https://hk4e-api-os.mihoyo.com/event/sol/info";

        public RewardInformationRequest(HoyolabAccount account) : base(account.AuthenticationData,
            account.UserAgent)
        {
            RequestMessage = new HttpRequestMessage(HttpMethod.Get,
                $"{AccountInfoUrl}?act_id=e202102251931481&lang={account.Language}&{AdditionalMetaParameters}");
        }

        protected override HttpRequestMessage RequestMessage { get; }
    }
}
