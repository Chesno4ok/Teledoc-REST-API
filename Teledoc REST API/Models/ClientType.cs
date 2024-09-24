using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Teledoc_REST_API;

[JsonObject(MemberSerialization.OptIn)]
public partial class ClientType
{
    [JsonProperty]
    public int Id { get; set; }
    [JsonProperty]
    public string Name { get; set; } = null!;

    public virtual ICollection<Client> Clients { get; set; } = new List<Client>();
}
