using System.Net;

namespace HoYoLab_API
{
    internal class AuthenticationData
    {
        private readonly string[] _requiredCookiesNames = { "ltoken", "ltuid" };

        private AuthenticationData(string rawCookies)
        {
            RawCookies = rawCookies;
        }

        public string RawCookies { get; }

        internal static AuthenticationData? CreateInstance(string rawCookies)
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
            string[] rawCookiesArray = RawCookies.Split("; ");

            if (rawCookiesArray.Length < _requiredCookiesNames.Length)
            {
                return false;
            }

            if (TryConvertRawCookies(rawCookiesArray,
                    out string[] cookiesNames) == false)
            {
                return false;
            }

            foreach (string searchCookieName in _requiredCookiesNames)
            {
                bool isFound = cookiesNames.Any(name => name == searchCookieName);

                if (!isFound)
                {
                    return false;
                }
            }

            return true;
        }

        private static bool TryConvertRawCookies(string[] rawCookies, out string[] cookiesNames)
        {
            cookiesNames = new string[rawCookies.Length];

            for (var i = 0; i < rawCookies.Length; i++)
            {
                string[] separatedCookie = rawCookies[i].Split("=");

                if (separatedCookie.Length != 2)
                {
                    return false;
                }

                cookiesNames[i] = separatedCookie[0];
            }

            return true;
        }
    }
}