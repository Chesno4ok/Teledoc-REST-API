using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Teledoc_REST_API;

[JsonObject(MemberSerialization.OptIn)]
public partial class Client
{
    [JsonProperty]
    public int Id { get; set; }
    [JsonProperty]
    public string Name { get; set; } = null!;
    [JsonProperty]
    public long Inn { get; set; }
    [JsonProperty]
    public int ClientType { get; set; }
    [JsonProperty]
    public DateTime? UpdateDate { get; set; }
    [JsonProperty]
    public DateTime? CreationDate { get; set; }
    [JsonProperty]
    public virtual ClientType ClientTypeNavigation { get; set; } = null!;
    public virtual ICollection<Founder> Founders { get; set; } = new List<Founder>();
}
