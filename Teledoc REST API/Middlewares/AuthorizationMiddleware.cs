using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using NuGet.Protocol;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using Teledoc_REST_API.Models;
using Teledoc_REST_API.Responses;

namespace Teledoc_REST_API.Middlewares
{
    public class AuthorizationMiddleware
    {
        private readonly RequestDelegate _next;
        private HttpContext context { get; set; } = null!;
        public AuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task InvokeAsync(HttpContext context)
        {
            this.context = context;

            // Получаем токен из заголовка
            var token = GetAuthorizationToken(context);
            if (token == null)
            {
                SendBadRequest("Missing or invalid token.");
                return Task.CompletedTask;
            }

            // Проверяем права доступа пользователя
            if (!AuthorizeToken(token))
            {
                SendBadRequest("Method is not permitted.");
                return Task.CompletedTask;
            }

            _next.Invoke(context);
            return Task.CompletedTask;
        }

        private void SendBadRequest(string message)
        {
            context.Response.StatusCode = 401;
            context.Response.WriteAsync(new UnauthorizedAccessException(message).ToJson());
        }
        public bool AuthorizeToken(AuthorizationToken token)
        {
            // Получаем тип контроллера, к которому поступил запрос
            var controllerType = GetControllerType();

            if (controllerType != null)
            {
                // Если в аттруибуте Authorization указан тот тип пользователя, то возвращаем true
                return controllerType.GetCustomAttribute<Authorization>()!
                    .Permissions.Any(i => i == token.AuthorizationRightNavigation.Name);
            }

            // При несовпадении возвращаем false
            return false;
        }
        private Type GetControllerType()
        {
            var controllerActionDescriptor = context
                .GetEndpoint()
                .Metadata
                .GetMetadata<ControllerActionDescriptor>();

            return controllerActionDescriptor.ControllerTypeInfo;
        }

        public static AuthorizationToken? GetAuthorizationToken(HttpContext context)
        {
            using var dbContext = new TeledocContext();

            StringValues tokenHeader;

            // Получаем токен из заголовка
            if (!context.Request.Headers.TryGetValue("Token", out tokenHeader))
            {
                return null;
            }

            string tokenString = tokenHeader.First()!;

            var md5 = MD5.Create();

            // Хэшируем токен из заголовка
            string tokenHash = AuthorizationToken.GetHashString(md5.ComputeHash(new Guid(tokenString).ToByteArray()));

            // Проверяем хэш токена в базе данных, при наличии, возвращается информация о токене
            var token = dbContext.AuthorizationTokens
                .Include(i => i.AuthorizationRightNavigation)
                .FirstOrDefault(i => i.TokenHash == tokenHash);

            return token;
        }
    }
}
