using System;
using Newtonsoft.Json;

namespace NetBaires.Api.Services.Meetup.Models
{
    public class Photo
    {

        [JsonProperty("highres_link")]
        public Uri HighresLink { get; set; }

    }
}