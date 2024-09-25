using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using Teledoc_REST_API.Models;
using Teledoc_REST_API.Responses;
using Teledoc_REST_API.Templates;

namespace Teledoc_REST_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorization("User","Admin")]
    public class FounderController : ControllerBase
    {
        private readonly IMapper mapper;
        public FounderController(IMapper mapper)
        {
            this.mapper = mapper;
        }

        /// <summary>
        /// Поиск всех учредителей
        /// </summary>
        [HttpGet("get_all_founders")]
        public IActionResult GetAllFounders()
        {
            using var dbContext = new TeledocContext();

            var founders = dbContext.Founders
                .Include(i => i.Client.ClientTypeNavigation);

            return Ok(mapper.Map<FounderResponse[]>(founders.ToArray()));
        }

        /// <summary>
        /// Поиск учредителя по Id
        /// </summary>
        [HttpGet("get_founder")]
        public IActionResult GetFounder(int founderId)
        {
            using var dbContext = new TeledocContext();

            var founder = dbContext.Founders
                .Include(i => i.Client.ClientTypeNavigation)
                .FirstOrDefault(i => i.Id == founderId);

            return Ok(mapper.Map<FounderResponse>(founder));
        }

        /// <summary>
        /// Создание учредителя
        /// </summary>
        [HttpPost("create_founder")]
        public IActionResult CreateFounder(FounderTemplate founderTemplate)
        {
            using var dbContext = new TeledocContext();

            if (founderTemplate.Id != null)
                return BadRequest("Id must be null when creating a founder");

            var founder = mapper.Map<Founder>(founderTemplate);

            dbContext.Add(founder);

            dbContext.SaveChanges();

            return GetFounder(founder.Id);
        }

        /// <summary>
        /// Обновление учредителя
        /// </summary>
        [HttpPut("update_founder")]
        public IActionResult UpdateFounder(FounderTemplate founderTemplate)
        {
            using var dbContext = new TeledocContext();

            var newFounder = dbContext.Founders
                .Include(i => i.Client)
                .FirstOrDefault(i => i.Id == founderTemplate.Id);

            newFounder = (Founder)mapper.Map(founderTemplate, newFounder, typeof(FounderTemplate), typeof(Founder));
            dbContext.SaveChanges();

            return GetFounder(newFounder.Id);
        }

        /// <summary>
        /// Удаление учредителя
        /// </summary>
        [HttpDelete("delete_founder")]
        public IActionResult DeleteFounder(int founderId)
        {
            using var dbContext = new TeledocContext();

            var founder = dbContext.Founders
                .Include(i => i.Client)
                .FirstOrDefault(i => i.Id == founderId);

            dbContext.Founders.Remove(founder);
            dbContext.SaveChanges();

            return Ok(new TextResponse("Founder has been deleted."));
        }
    }
}
