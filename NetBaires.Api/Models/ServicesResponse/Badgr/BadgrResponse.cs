using System.Collections.Generic;
using Newtonsoft.Json;

namespace NetBaires.Api.Models.ServicesResponse.Badgr
{
    public  class BadgrResponse<TType>
    {
        [JsonProperty("status")]
        public Status Status { get; set; }

        [JsonProperty("result")]
        public List<TType> Result { get; set; }
    }
}
