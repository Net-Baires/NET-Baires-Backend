using Newtonsoft.Json;

namespace NetBaires.Api.Services.EventBrite.Models
{
    public class Answer
    {
        [JsonProperty("question")]
        public string Question { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("question_id")]
        public long QuestionId { get; set; }

        [JsonProperty("answer", NullValueHandling = NullValueHandling.Ignore)]
        public string AnswerAnswer { get; set; }
    }
}