namespace HoYoLab_API
{
    public class HoyolabAccount
    {
        public static readonly string[] SupportedLanguages =
        {
            "en-us", "ru-ru", "de-de", "es-es", "fr-fr", "id-id",
            "pt-pt", "vi-vn", "zh-cn", "zh-tw", "ko-kr", "ja-jp",
            "th-th"
        };
        private readonly GameAccounts _gameAccounts;

        /// <summary>
        /// Initializes a instance of <see cref="HoyolabAccount"/>
        /// </summary>
        /// <remarks>Important: During initialization, the validity of the account is not checked!</remarks>
        /// <param name="rawCookies">A raw cookie string, that contains ltuid and ltoken</param>
        /// <param name="language">Affects the received letters in game and language of http(s) responses from Hoyolab.
        /// Supported languages contains in <see cref="SupportedLanguages"/></param>
        /// <param name="userAgent">A UserAgent, that will be used to send requests. Can be empty.</param>
        /// <exception cref="ArgumentException">Throws when the provided language is not supported, or cookies doesn't contains required values</exception>
        public HoyolabAccount(string rawCookies, string language = "en-us", string userAgent = "")
        {
            if (SupportedLanguages.Contains(language) == false)
            {
                throw new ArgumentException("Language is not supported!");
            }

            var authenticationData = AuthenticationData.CreateInstance(rawCookies);

            AuthenticationData = authenticationData ?? throw new ArgumentException("Failed to parse cookies!");

            UserAgent = userAgent;
            Language = language;

            _gameAccounts = new GameAccounts(this);
        }

        /// <summary>
        /// Indicates, that AuthenticationData valid and account authorized in HoYoLab
        /// </summary>
        public bool IsValid => _gameAccounts.Accounts != null;

        /// <summary>
        /// All Genshin Impact accounts, connected to the HoYoLab account [Has a cache time!]
        /// </summary>
        public IReadOnlyList<GameAccounts.GameAccount>? GameAccounts => _gameAccounts.Accounts;

        /// <summary>
        /// Authentication data for HoYoLab authorization 
        /// </summary>
        internal AuthenticationData AuthenticationData { get; }

        /// <summary>
        /// A UserAgent, that will be used to send requests. Can be empty.
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        /// Affects the received letters in game and language of http(s) responses from Hoyolab.
        /// </summary>
        public string Language { get; }
    }
}