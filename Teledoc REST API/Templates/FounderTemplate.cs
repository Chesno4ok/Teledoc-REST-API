using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Teledoc_REST_API.Templates
{
    public class FounderTemplate : IValidatableObject
    {
        [Display]
        public int? Id { get; set; }

        [Required]
        [StringLength(maximumLength: 50, MinimumLength = 1)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(maximumLength: 50, MinimumLength = 1)]
        public string LastName { get; set; } = null!;

        [Required]
        [StringLength(maximumLength: 50, MinimumLength = 1)]
        public string? MiddleName { get; set; }

        [Display]
        public int? ClientId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            using var dbContext = new TeledocContext();

            if (dbContext.Founders.FirstOrDefault(i => i.Id == Id) == null && Id != null)
            {
                yield return new ValidationResult("Founder not found", new string[] { "Id" });
                yield break;
            }

            if (dbContext.Clients.FirstOrDefault(i => i.Id == ClientId) == null && ClientId != null)
                yield return new ValidationResult("Client doesn't exist", new string[] { "ClientId" });
            if (dbContext.Clients.FirstOrDefault(i => i.Id == ClientId && i.ClientType == 2) != null)
                yield return new ValidationResult("Client cannot be added to founder due to invalid ClientType.");

        }
    }
}
