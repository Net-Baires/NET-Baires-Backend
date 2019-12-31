using System;
using NetBaires.Api.Helpers;
using Newtonsoft.Json;

namespace NetBaires.Api.Auth
{
    public class NumericDate : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return DateTimeHelper.FromExcelSerialDate(int.Parse(reader.Value.ToString()));
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
        }
    }
}