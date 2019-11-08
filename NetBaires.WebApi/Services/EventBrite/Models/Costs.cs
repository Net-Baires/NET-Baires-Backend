using Newtonsoft.Json;

namespace NetBaires.Api.Services.EventBrite.Models
{
    public class Costs
    {
        [JsonProperty("base_price")]
        public BasePrice BasePrice { get; set; }

        [JsonProperty("eventbrite_fee")]
        public BasePrice EventbriteFee { get; set; }

        [JsonProperty("gross")]
        public BasePrice Gross { get; set; }

        [JsonProperty("payment_fee")]
        public BasePrice PaymentFee { get; set; }

        [JsonProperty("tax")]
        public BasePrice Tax { get; set; }
    }
}