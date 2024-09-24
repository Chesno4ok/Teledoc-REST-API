namespace Teledoc_REST_API.Models
{
    public class Authorization : Attribute
    {
        public string[] Permissions { get; set; } = null!;

        public Authorization(params string[] permissions)
        {
            this.Permissions = permissions;
        }
    }
}
