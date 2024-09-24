using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Teledoc_REST_API;

[JsonObject(MemberSerialization.OptIn)]
public partial class Founder
{
    [JsonProperty]
    public int Id { get; set; }
    [JsonProperty]
    public string Name { get; set; } = null!;
    [JsonProperty]
    public string LastName { get; set; } = null!;
    [JsonProperty]
    public string? MiddleName { get; set; }
    [JsonProperty]
    public DateTime CreationDate { get; set; }
    [JsonProperty]
    public DateTime UpdateDate { get; set; }
    [JsonProperty]
    public int? ClientId { get; set; }

    public virtual Client Client { get; set; } = null!;
}
