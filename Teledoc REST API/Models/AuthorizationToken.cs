using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Teledoc_REST_API.Models;

[JsonObject(MemberSerialization.OptIn)]
public partial class AuthorizationToken
{
    [JsonProperty]
    public int Id { get; set; }

    public string TokenHash { get; set; } = null!;
    
    public int AuthorizationRight { get; set; }

    [JsonProperty]
    public virtual AuthorizationRight AuthorizationRightNavigation { get; set; } = null!;
    public static string GetHashString(byte[] hash)
    {
        StringBuilder sb = new StringBuilder();

        foreach (var b in hash)
            sb.Append(b.ToString("x2"));

        return sb.ToString();
    }

}
