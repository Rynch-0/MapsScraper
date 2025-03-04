using Newtonsoft.Json;
using System.Collections.Generic;

namespace MapsScraper.Models
{
    // Google Places API'den dönen yanıtı modellemek için
    public class GooglePlacesApiResponse
    {
        [JsonProperty("results")]
        public List<GooglePlaceDetails> Results { get; set; } // Sonuçlar GooglePlaceDetails türünde
    }

    // Google Places API'den dönen mekanlar için detaylar
    public class GooglePlaceDetails
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("formatted_address")]
        public string FormattedAddress { get; set; }

        [JsonProperty("formatted_phone_number")]
        public string FormattedPhoneNumber { get; set; }

        [JsonProperty("website")]
        public string Website { get; set; }
    }

    // Her bir mekan (yer) için detaylı bilgi
    public class GooglePlace
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("formatted_address")]
        public string FormattedAddress { get; set; }

        [JsonProperty("formatted_phone_number")]
        public string FormattedPhoneNumber { get; set; }

        [JsonProperty("website")]
        public string Website { get; set; }
    }
}
