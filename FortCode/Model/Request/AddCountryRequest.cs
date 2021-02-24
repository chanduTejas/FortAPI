using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace FortCode.Model.Request
{
    public class AddCountryRequest
    {
        [JsonProperty("countryid")]
        public string CountryID { get; set; }
        [JsonProperty("countryname")]
        [Required(ErrorMessage = "CountryName is required")]
        public string CountryName { get; set; }
        [JsonProperty("city")]
        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }
    }
}
