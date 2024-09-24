using System.Reflection;
using Teledoc_REST_API.Models;
using NuGet.Protocol;
using Teledoc_REST_API.Responses;
using Microsoft.AspNetCore.Http;

namespace Teledoc_REST_API.Middlewares
{
    /// <summary>
    /// Аттрибут для хранения обрабатываемых параметров
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class ParameterValidation : Attribute
    {
        public string[] parameters;
        public ParameterValidation()
        {

        }
        public ParameterValidation(params string[] parameters)
        {
            this.parameters = parameters;
        }
    }
    public class ParamCheckMiddleware
    {
        private readonly RequestDelegate _next;
        private HttpContext _httpContext;
        private Dictionary<string, string> _query;
        public ParamCheckMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task InvokeAsync(HttpContext context)
        {
            _query = context.Request.Query.ToDictionary(i => i.Key, i => i.Value.ToString());
            _httpContext = context;

            bool result = true;

            // Список неправильных параметров
            List<string> invalidParameters = new();

            var methods = GetType().GetMethods()
                // Ищем методы с наличием аттрибута ParameterValidation
                 .Where(i => i.GetCustomAttribute<ParameterValidation>() != null)
                // Получаем из них те, в аттрибутах которых указаны нужные параметры
                 .Where(i => i.GetCustomAttribute<ParameterValidation>().parameters.All(i => _query.ContainsKey(i)));

            // Пробегаемся по методам
            foreach (var m in methods)
            {
                // Передаем параметры в метод. 
                // True - параметры прошли проверку
                // False - не прошли
                result = (bool)m.Invoke(this, new object[1] { _query });

                // Если метод вернул false, добавляем неверный параметр в список
                if (!result)
                    invalidParameters.AddRange(m.GetCustomAttribute<ParameterValidation>().parameters);
            }

            
            if (invalidParameters.Count == 0)
                _next.Invoke(context);
            else
                SendBadRequest(context, invalidParameters.ToArray());

            return Task.CompletedTask;
        }
        private void SendBadRequest(HttpContext context, string[] parameters)
        {
            context.Response.StatusCode = 400;
            context.Response.WriteAsync(new InvalidArgumentResponse("Invalid parameters", parameters).ToJson());
        }

        [ParameterValidation("clientId")]
        public bool Validate_ClientId(Dictionary<string, string> _query)
        {
            var dbContext = new TeledocContext();

            int clientId = 0;
            try
            {
                clientId = Int32.Parse(_query["clientId"]);
            }
            catch
            {
                
            }
            

            var client = dbContext.Clients.FirstOrDefault(i => i.Id == clientId);

            return client != null;
        }

        [ParameterValidation("founderId")]
        public bool Validate_FounderId(Dictionary<string, string> _query)
        {
            var dbContext = new TeledocContext();

            int founderId = 0;
            try
            {
                founderId = Convert.ToInt32(_query["founderId"]);
            }
            catch
            {

            }

            var founder = dbContext.Founders.FirstOrDefault(i => i.Id == founderId);

            return founder != null;
        }

        [ParameterValidation("tokenId")]
        public bool Validate_TokenId(Dictionary<string, string> _query)
        {
            var dbContext = new TeledocContext();

            int tokenId = 0;
            try
            {
                tokenId = Convert.ToInt32(_query["tokenId"]);
            }
            catch
            {

            }

            var token = dbContext.AuthorizationTokens.FirstOrDefault(i => i.Id == tokenId);

            return token != null;
        }
    }
}
