using System.Net;

namespace HoYoLab_API.Requests
{
    internal abstract class DefaultRequest
    {
        private readonly AuthenticationData _authenticationData;
        private readonly string _language;
        private readonly string _userAgent;

        protected readonly string AdditionalMetaParameters =
            "mhy_auth_required=true&mhy_presentation_style=fullscreen&utm_source=tools&bbs_theme=dark&bbs_theme_device=1";

        protected DefaultRequest(HoyolabAccount account)
        {
            _authenticationData = account.AuthenticationData;
            _language = account.Language;
            _userAgent = account.UserAgent;
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

            RequestMessage.Headers.Add("Host", RequestMessage.RequestUri.Host);

            if (_userAgent != string.Empty)
            {
                RequestMessage.Headers.Add("User-Agent", _userAgent);
            }
            
            RequestMessage.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/jxl,image/webp,*/*;q=0.8");
            RequestMessage.Headers.Add("Accept-Language", _language);
            RequestMessage.Headers.Add("DNT", "1");
            RequestMessage.Headers.Add("Connection", "keep-alive");
            RequestMessage.Headers.Add("Upgrade-Insecure-Requests", "1");
            RequestMessage.Headers.Add("Sec-Fetch-Dest", "document");
            RequestMessage.Headers.Add("Sec-Fetch-Mode", "navigate");
            RequestMessage.Headers.Add("Sec-Fetch-Site", "cross-site");
            RequestMessage.Headers.Add("Sec-Fetch-User", "?1");
            RequestMessage.Headers.Add("Sec-GPC", "1");
            RequestMessage.Headers.Add("Pragma", "no-cache");
            RequestMessage.Headers.Add("Cache-Control", "no-cache");
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