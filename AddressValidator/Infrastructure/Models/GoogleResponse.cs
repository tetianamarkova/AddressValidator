using Newtonsoft.Json;

namespace AddressValidator.Infrastructure.Models
{
    public class GoogleResponse
    {
        [JsonProperty("results")]
        public Result[] Results { get; set; }
    }

    public class Result
    {
        [JsonProperty("address_components")]
        public AddressComponent[] AddressComponents { get; set; }
    }

    public class AddressComponent
    {
        [JsonProperty("long_name")]
        public string LongName { get; set; }

        [JsonProperty("short_name")]
        public string ShortName { get; set; }

        [JsonProperty("types")]
        public string[] Types { get; set; }
    }
}
