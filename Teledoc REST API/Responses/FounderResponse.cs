using Newtonsoft.Json;

namespace Teledoc_REST_API.Responses
{
    public class FounderResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? MiddleName { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public Client Client { get; set; } = null!;
    }
}
