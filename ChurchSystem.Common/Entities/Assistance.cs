using Newtonsoft.Json;

namespace ChurchSystem.Common.Entities
{
    public class Assistance
    {
        public int Id { get; set; }

        public bool IsPresent { get; set; }
        public User User { get; set; }
        [JsonIgnore]
        public Meeting Meeting { get; set; }
    }
}
