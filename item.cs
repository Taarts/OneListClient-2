using System;
using System.Text.Json.Serialization;

namespace OneListClient
{
    public class item
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("text")]
        public string Text { get; set; }
        [JsonPropertyName("completed")]
        public bool Complete { get; set; }
        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }
        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt { get; set; }

        public string CompletedStatus
        {
            get
            {
                // boolean expression  ?  true       : false
                return Complete ? "complete" : "incomplete";
            }
        }
    }
}