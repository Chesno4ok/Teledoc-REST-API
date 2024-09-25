using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text;
using NuGet.Protocol;
using Microsoft.AspNetCore.Authorization;
using Teledoc_REST_API.Responses;
using Teledoc_REST_API.Templates;
using Teledoc_REST_API.Models;

namespace Teledoc_REST_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorization("User", "Admin")]
    public class ClientController : ControllerBase
    {
        private readonly IMapper mapper;
        public ClientController(IMapper mapper)
        {
            this.mapper = mapper;
        }


        /// <summary>
        /// Поиск клиента по Id учредителя
        /// </summary>
        [HttpGet("find_client_by_founder")]
        public IActionResult FindClientByFounder(int founderId)
        {
            using var dbContext = new TeledocContext();

            

            var client = dbContext.Founders
                .Include(i => i.Client)
                .Select(i => i.Client)
                .FirstOrDefault(i => i.Id == founderId);

            if (client == null)
                return NotFound(new TextResponse("Client not found"));

            return Ok(mapper.Map<ClientResponse>(client));
        }

        /// <summary>
        /// Поиск клиента по ИНН
        /// </summary>
        [HttpGet("find_client_by_inn")]
        public IActionResult FindClientByInn(long inn)
        {
            using var dbContext = new TeledocContext();

            var client = dbContext.Clients
                .Include(i => i.ClientTypeNavigation)
                .Include(i => i.Founders)
                .FirstOrDefault(i => i.Inn == inn);

            if (client == null)
                return NotFound(new TextResponse("Client not found"));

            return Ok(mapper.Map<ClientResponse>(client));
        }

        /// <summary>
        /// Поиск всех клиентов
        /// </summary>
        [HttpGet("get_all_clients")]
        public IActionResult GetAllClients()
        {
            using var dbContext = new TeledocContext();

            var clients = dbContext.Clients
                .Include(i => i.ClientTypeNavigation)
                .Include(i => i.Founders);

            return Ok(mapper.Map<ClientResponse[]>(clients.ToArray()));
        }

        /// <summary>
        /// Поиск клиента по Id
        /// </summary>
        [HttpGet("get_client")]
        public IActionResult GetClient(int clientId)
        {
            using var dbContext = new TeledocContext();

            var client = dbContext.Clients
                .Include(i => i.ClientTypeNavigation)
                .Include(i => i.Founders)
                .FirstOrDefault(i => i.Id == clientId);

            return Ok(mapper.Map<ClientResponse>(client));
        }

        /// <summary>
        /// Создание клиента
        /// </summary>
        [HttpPost("create_client")]
        public IActionResult CreateClient(ClientTemplate clientTemplate)
        {
            using var dbContext = new TeledocContext();

            if (clientTemplate.Id != null)
                return BadRequest(new TextResponse("Id must be null when creating a client"));

            var client = mapper.Map<Client>(clientTemplate);

            dbContext.Clients.Add(client);

            dbContext.SaveChanges();

            return GetClient(client.Id);
        }

        /// <summary>
        /// Удаление клиента
        /// </summary>
        [HttpDelete("delete_client")]
        public IActionResult DeleteClient(int clientId)
        {
            using var dbContext = new TeledocContext();

            var client = dbContext.Clients
                .Include(i => i.Founders)
                .FirstOrDefault(i => i.Id == clientId);

            if (client.Founders.Count != 0)
                return BadRequest(new InvalidArgumentResponse(
                    "Client cannot be deleted due to Founders reffering to it.",
                    new string[] {"clientId"}));

            dbContext.Clients.Remove(client);
            
            dbContext.SaveChanges();

            return Ok(new TextResponse("Client has been deleted"));
        }

        /// <summary>
        /// Обновление клиента
        /// </summary>
        [HttpPut("update_client")]
        public IActionResult UpdateClient(ClientTemplate clientTemplate)
        {
            using var dbContext = new TeledocContext();

            if(clientTemplate.Id == null)
                return BadRequest(new TextResponse("Id cannot be null"));

            var newClient = dbContext.Clients
                .Include(i => i.ClientTypeNavigation)
                .Include(i => i.Founders)
                .FirstOrDefault(i => i.Id == clientTemplate.Id);

            newClient = (Client)mapper.Map(clientTemplate, newClient, typeof(ClientTemplate), typeof(Client));

            dbContext.SaveChanges();

            return GetClient(newClient.Id);
        }

    }
}
