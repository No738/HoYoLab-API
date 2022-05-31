using System.Net;

namespace HoYoLab_API.Requests
{
    internal abstract class DefaultRequest
    {
        private readonly AuthenticationData _authenticationData;
        private readonly string _userAgent;

        protected readonly string AdditionalMetaParameters =
            "mhy_auth_required=true&mhy_presentation_style=fullscreen&utm_source=tools&bbs_theme=dark&bbs_theme_device=1";

        protected DefaultRequest(AuthenticationData authenticationData, string userAgent)
        {
            _authenticationData = authenticationData;
            _userAgent = userAgent;
        }

        protected abstract HttpRequestMessage RequestMessage { get; }

        public bool TrySend(out string response)
        {
            try
            {
                AddRequestMessageHeaders();

                using var client = new HttpClient(new HttpClientHandler
                {
                    CookieContainer = BuildCookieContainer()
                });

                HttpResponseMessage responseMessage = client.SendAsync(RequestMessage).Result;
                response = responseMessage.Content.ReadAsStringAsync().Result;
                return true;
            }
            catch
            {
                response = string.Empty;
                return false;
            }
        }

        private void AddRequestMessageHeaders()
        {
            if (RequestMessage.RequestUri == null)
            {
                throw new ArgumentNullException(nameof(RequestMessage.RequestUri), "Request message URI is null!");
            }

            if (_userAgent != string.Empty)
            {
                RequestMessage.Headers.Add("User-Agent", _userAgent);
            }

            RequestMessage.Headers.Add("Accept", "*/*");
            RequestMessage.Headers.Add("Host", RequestMessage.RequestUri.Host);
            RequestMessage.Headers.Add("Accept-Encoding", "gzip, deflate, br");
            RequestMessage.Headers.Add("Connection", "keep-alive");
        }

        private CookieContainer BuildCookieContainer()
        {
            var cookieContainer = new CookieContainer();

            foreach (Cookie cookie in _authenticationData.AuthenticationCookies)
            {
                cookieContainer.Add(cookie);
            }
            
            return cookieContainer;
        }
    }
}