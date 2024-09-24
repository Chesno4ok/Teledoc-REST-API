using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Teledoc_REST_API.Templates
{
    public class ClientTemplate : IValidatableObject
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(maximumLength: 100, MinimumLength = 1)]
        public string Name { get; set; } = null!;

        [Required]
        [Range(100000000000, 999999999999)]
        public long Inn { get; set; }

        [Required]
        public int ClientType { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            using var dbContext = new TeledocContext();

            if(dbContext.Clients.FirstOrDefault(i => i.Id == Id) == null)
            {
                yield return new ValidationResult("Client not found", new string[] { "Inn" });
                yield break;
            }

            if (dbContext.Clients.FirstOrDefault(i => i.Inn == Inn && i.Id != Id) != null)
                yield return new ValidationResult("INN alreay exitsts", new string[] { "Inn" });
            if (dbContext.Clients.FirstOrDefault(i => i.Name == Name && i.Id != Id) != null)
                yield return new ValidationResult("Name alreay exitsts", new string[] { "Name" });
            if (dbContext.ClientTypes.FirstOrDefault(i => i.Id == ClientType) == null)
                yield return new ValidationResult("ClientType doesn't exist", new string[] { "ClientType" });
            if(dbContext.Founders.FirstOrDefault(i => i.ClientId == Id) != null && ClientType != 1)
                yield return new ValidationResult("Client is not allowed to have Founders with such ClientType", new string[] { "ClientType" });


        }
    }
}
