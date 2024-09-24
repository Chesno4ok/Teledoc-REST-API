using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using Teledoc_REST_API.Models;

namespace Teledoc_REST_API.Templates
{
    public class AuthorizationTokenTemplate : IValidatableObject
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int AuthorizationRight { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            using var dbContext = new TeledocContext();

            if (dbContext.AuthorizationTokens.FirstOrDefault(i => i.Id == Id) == null)
            {
                yield return new ValidationResult("Token not found", new string[] { "Id" });
                yield break;
            }

            if (dbContext.AuthorizationRights.FirstOrDefault(i => i.Id == AuthorizationRight) == null)
                yield return new ValidationResult("AuthorizatioRight doesn't exist", new string[] { "AuthorizationRight" });
        }
    }
}
