using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ShaRide.Application.Helpers
{
    public static class BAKCELL
    {
        /// <summary>
        /// Checks whether the number is a Bakcell number.
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public static async Task<bool> IsBakCellNUmber(string phoneNumber)
        {
            string xml = @"<?xml version=""1.0"" encoding=""UTF-8""?>
                            <autocomplete>
                             <authcode>dfe1efbacb9130717d30b085249fb54e</authcode>
                             <msisdn>" + phoneNumber + @"</msisdn>
                             <amount>10</amount>
                            </autocomplete>";

            var SECRETKEY = "0e1e184ac8ddff661f64c23a555ea895";

            var hash = MD5Hasher.CreateMD5Hash(xml + SECRETKEY);

            var requestContent = new StringContent(xml, Encoding.UTF8, "application/json");
            using var _httpClient = new HttpClient();
            var response = await _httpClient.PostAsync($"https://portal.emobile.az/externals/loyalty/autocomplete?hash={hash.ToLower()}", requestContent);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            if (!result.Contains("error") && result.Contains("success"))
            {
                return true;
            }
            return false;
        }
    }
}
