using System.Net;

namespace HoYoLab_API
{
    internal class AuthenticationData
    {
        private readonly string[] _requiredCookiesNames = { "ltoken", "ltuid" };

        private readonly string _rawCookies;

        private Cookie[] _cookies;

        private AuthenticationData(string rawCookies)
        {
            _rawCookies = rawCookies;
            _cookies = new Cookie[_requiredCookiesNames.Length];
        }

        public IEnumerable<Cookie> AuthenticationCookies => _cookies;

        public static AuthenticationData? CreateInstance(string rawCookies)
        {
            var newData = new AuthenticationData(rawCookies);

            if (newData.TryParseRawCookies() == false)
            {
                return null;
            }

            return newData;
        }

        private bool TryParseRawCookies()
        {
            string[] rawCookiesArray = _rawCookies.Split("; ");

            if (rawCookiesArray.Length < _requiredCookiesNames.Length)
            {
                return false;
            }

            if (TryConvertRawCookies(rawCookiesArray,
                    out (string cookieName, string cookieValue)[] cookieTuples) == false)
            {
                return false;
            }

            _cookies = new Cookie[_requiredCookiesNames.Length];
            var cookieIndex = 0;

            foreach (string searchCookieName in _requiredCookiesNames)
            {
                var isFound = false;

                foreach ((string name, string value) in cookieTuples)
                {
                    if (name != searchCookieName)
                    {
                        continue;
                    }
                    
                    _cookies[cookieIndex++] = new Cookie(name, value, "/", ".mihoyo.com");
                    isFound = true;

                    break;
                }

                if (!isFound)
                {
                    return false;
                }
            }
            
            return true;
        }

        private static bool TryConvertRawCookies(string[] rawCookies, out (string cookieName, string cookieValue)[] processedCookies)
        {
            processedCookies = new (string, string)[rawCookies.Length];

            for (var i = 0; i < rawCookies.Length; i++)
            {
                string[] separatedCookie = rawCookies[i].Split("=");

                if (separatedCookie.Length != 2)
                {
                    return false;
                }

                processedCookies[i] = (separatedCookie[0], separatedCookie[1]);
            }

            return true;
        }
    }
}