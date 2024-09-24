using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Teledoc_REST_API.Models;

[JsonObject(MemberSerialization.OptIn)]
public partial class AuthorizationRight
{
    [JsonProperty]
    public int Id { get; set; }

    [JsonProperty]
    public string Name { get; set; } = null!;

    public virtual ICollection<AuthorizationToken> AuthorizationTokens { get; set; } = new List<AuthorizationToken>();
}
