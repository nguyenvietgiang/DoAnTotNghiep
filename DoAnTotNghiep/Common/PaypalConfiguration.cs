using DoAnTotNghiep.Models;
using PayPal.Api;

namespace DoAnTotNghiep.Common
{
    public static class PaypalConfiguration
    {
        public readonly static string ClientId;
        public readonly static string ClientSecret;

        static PaypalConfiguration()
        {
            ClientId = "Aaz-KuDGSlO_rln_zXrqY30bVCfExLr62RPzTuaQqr9OgCQKV72KFj8ZVO7ApGvoNTwQJkyXyTbK3m8l";
            ClientSecret = "ELJVCbAS2QBjz-Rn8WBbPgjasHWGlFY-jYH0vTYr8UFrYkcubydWHucaPUEzaqgh3JQiOXE72tKvZZz8";
        }

        private static string GetAccessToken()
        {
            var configDictionary = new Dictionary<string, string>
        {
            { "clientId", ClientId },
            { "clientSecret", ClientSecret }
        };

            string accessToken = new OAuthTokenCredential(configDictionary).GetAccessToken();
            return accessToken;
        }

        public static APIContext GetAPIContext()
        {
            APIContext apiContext = new APIContext(GetAccessToken());
            return apiContext;
        }
    }
}
