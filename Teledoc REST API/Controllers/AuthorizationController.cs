using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using Teledoc_REST_API.Models;
using Teledoc_REST_API.Responses;
using Teledoc_REST_API.Middlewares;
using Teledoc_REST_API.Templates;
using AuthorizationMiddleware = Teledoc_REST_API.Middlewares.AuthorizationMiddleware;
using NuGet.Common;

namespace Teledoc_REST_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorization("Admin")]
    public class AuthorizationController : ControllerBase
    {
        private readonly IMapper mapper;
        public AuthorizationController(IMapper mapper)
        {
            this.mapper = mapper;
        }

        /// <summary>
        /// Получение информации о токене по Id
        /// </summary>
        [HttpGet("get_token")]
        public IActionResult GetUser(int tokenId)
        {
            using var dbContext = new TeledocContext();

            var token = dbContext.AuthorizationTokens
                .Include(i => i.AuthorizationRightNavigation)
                .FirstOrDefault(i => i.Id == tokenId);

            return Ok(token);
        }

        /// <summary>
        /// Генерация токена
        /// </summary>
        [HttpPost("create_token")]
        public IActionResult CreateUser(AuthorizationTokenTemplate tokenTemplate)
        {
            using var dbContext = new TeledocContext();

            Guid token = System.Guid.NewGuid();
            MD5 MD5Hash = MD5.Create();

            byte[] byteToken = MD5Hash.ComputeHash(token.ToByteArray());

            var auth = new AuthorizationToken()
            {
                AuthorizationRight = tokenTemplate.AuthorizationRight,
                TokenHash = AuthorizationToken.GetHashString(byteToken)
            };

            dbContext.AuthorizationTokens.Add(auth);
            dbContext.SaveChanges();

            var authRight = dbContext.AuthorizationRights.FirstOrDefault(i => i.Id == tokenTemplate.AuthorizationRight);

            return Ok(new TokenResponse(token.ToString(), authRight));
        }

        /// <summary>
        /// Удаления токена
        /// </summary>
        [HttpDelete("delete_token")]
        public IActionResult DeleteToken(int tokenId)
        {
            using var dbContext = new TeledocContext();

            var token = AuthorizationMiddleware.GetAuthorizationToken(HttpContext);

            var deleteToken = dbContext.AuthorizationTokens.FirstOrDefault(i => i.Id == tokenId);

            // Получаем текущий токен, если он совпадает с удалющимся, то запрещаем
            if(token.Id == deleteToken.Id)
            {
                return BadRequest(new TextResponse("Cannot edit yourself!"));
            }

            dbContext.Remove(deleteToken);
            dbContext.SaveChanges();

            return Ok(new TextResponse("Token has been deleted"));
        }

        /// <summary>
        /// Обновление прав токена
        /// </summary>
        [HttpPut("update_token")]
        public IActionResult UpdateToken(AuthorizationTokenTemplate tokenTemplate)
        {
            using var dbContext = new TeledocContext();

            var token = AuthorizationMiddleware.GetAuthorizationToken(HttpContext);

            var updateToken = dbContext.AuthorizationTokens
                .Include(i => i.AuthorizationRightNavigation)
                .FirstOrDefault(i => i.Id == tokenTemplate.Id);

            if (token.Id == updateToken.Id)
            {
                return BadRequest(new TextResponse("Cannot edit yourself!"));
            }

            updateToken = (AuthorizationToken)mapper.Map(tokenTemplate,
                updateToken,
                typeof(AuthorizationTokenTemplate),
                typeof(AuthorizationToken));

            dbContext.SaveChanges();

            updateToken = dbContext.AuthorizationTokens
                .Include(i => i.AuthorizationRightNavigation)
                .FirstOrDefault(i => i.Id == tokenTemplate.Id);

            return Ok(updateToken);
        }
    }
}
