using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalkDifficutiesController : Controller
    {
        private readonly IWalkDifficultyRepository walkDifficultyRepository;
        private readonly IMapper mapper;
        public WalkDifficutiesController(IWalkDifficultyRepository walkDifficultyRepository,IMapper mapper)
        {
            this.walkDifficultyRepository = walkDifficultyRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllWalkDifficulties()
        {
            //var walkDifficultiesDomain = await walkDifficultyRepository.GetAllAsync();
            //var walkDifficultiesDTO = mapper.Map<List<Models.DTO.WalkDifficulty>>(walkDifficultiesDomain);
            //return Ok(walkDifficultiesDTO);
            return Ok(await walkDifficultyRepository.GetAllAsync());
        }
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetWalkDifficultyById( Guid id)
        {
          var item = await walkDifficultyRepository.GetAsync(id);
            if(item == null)
            {
                return NotFound();
            }
            //convert Domain to dto
            //var itemDTO = mapper.Map<Models.DTO.WalkDifficulty>(item);
            return Ok(item);
        }
    }
}
