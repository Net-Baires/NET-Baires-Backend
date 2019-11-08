using System;
using Newtonsoft.Json;

namespace NetBaires.Api.Services.EventBrite.Models
{
    public class Barcode
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("barcode")]
        public string BarcodeBarcode { get; set; }

        [JsonProperty("created")]
        public DateTimeOffset Created { get; set; }

        [JsonProperty("changed")]
        public DateTimeOffset Changed { get; set; }

        [JsonProperty("checkin_type")]
        public long CheckinType { get; set; }

        [JsonProperty("is_printed")]
        public bool IsPrinted { get; set; }
    }
}