using System.Text.Json;
using System.Text.Json.Serialization;

namespace EmailSink
{
    public class EmailMsg
    {
        [JsonPropertyName("to")]
        public string To { get; set; }

        [JsonPropertyName("msg")]
        public string Message { get; set; }
    }
}
