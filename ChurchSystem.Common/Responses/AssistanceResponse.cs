using ChurchSystem.Common.Entities;
using Newtonsoft.Json;

namespace ChurchSystem.Common.Responses
{
    public class AssistanceResponse
    {
        public int Id { get; set; }
        public bool IsPresent { get; set; }
        public User User { get; set; }
        [JsonIgnore]
        public Meeting Meeting { get; set; }
    }
}
