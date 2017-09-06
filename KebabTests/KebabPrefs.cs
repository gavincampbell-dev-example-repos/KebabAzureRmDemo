
using Newtonsoft.Json;

namespace KebabTests
{
    class KebabPrefs
    {
        [JsonProperty(PropertyName = "id)]")]
        public string Id { get; set; }
        public string name { get; set; }
        public string favouriteKebab { get; set; }

    }
}
