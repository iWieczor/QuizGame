using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ClassCommand
{
    public class Message
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public ActionType Action { get; set; }
        public string Data { get; set; }
    }


}
