using Teledoc_REST_API.Models;

namespace Teledoc_REST_API.Responses
{
    class TokenResponse
    {
        public string AuthorizationToken { get; set; } = null!;
        public AuthorizationRight AuthorizationRightNavigation { get; set; } = null!;
        public TokenResponse(string token, AuthorizationRight authorizationRight)
        {
            this.AuthorizationRightNavigation = authorizationRight;
            this.AuthorizationToken = token;
        }
    }
}
