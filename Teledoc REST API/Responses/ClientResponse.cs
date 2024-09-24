using Newtonsoft.Json;

namespace Teledoc_REST_API.Responses
{
    public class ClientResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public long Inn { get; set; }
        public int ClientType { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? CreationDate { get; set; }
        public ClientType ClientTypeNavigation { get; set; } = null!;
        public ICollection<Founder> Founders { get; set; } = new List<Founder>();
    }
}
