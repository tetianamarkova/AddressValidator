using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AddressValidator.Infrastructure.Models;
using AddressValidator.Infrastructure.Models.AppSettings;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace AddressValidator.Infrastructure.Clients
{
    public class GoogleService : IGoogleService
    {
        private readonly HttpClient _httpClient;
        private readonly GoogleSettings _googleSettings;

        public GoogleService(HttpClient client,
            IOptions<GoogleSettings> googleSettingsOptions)
        {
            _httpClient = client;
            _googleSettings = googleSettingsOptions.Value;
        }

        public async Task<GoogleResponse> ValidateAsync(string address)
        {
            var url = $"{_httpClient.BaseAddress}?address={address}&language=uk&key={_googleSettings.Key}";
            HttpResponseMessage result = await _httpClient.GetAsync(url);

            if (result.IsSuccessStatusCode)
            {
                string json = await result.Content.ReadAsStringAsync();
                GoogleResponse googleResponse = JsonConvert.DeserializeObject<GoogleResponse>(json);

                return googleResponse;
            }
            else
            {
                Console.WriteLine($"Response Status Code is not successful ({result.StatusCode}) for address: {address}");
                return null;
            }
        }
    }
}
