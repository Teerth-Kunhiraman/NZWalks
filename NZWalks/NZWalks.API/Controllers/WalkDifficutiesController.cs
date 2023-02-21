using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
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
            this.mapper = mapper;
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
        [HttpPost]
        public async Task<IActionResult> AddWalkDifficultyAsync(Models.DTO.AddWalkDifficultyRequest addWalkDifficultyRequest)
        {
            //Convert dto to domain model
            var walkDifficultyDomain = new Models.Domain.WalkDifficulty
            {
                Code = addWalkDifficultyRequest.Code,
            };
            //call repo
             walkDifficultyDomain = await walkDifficultyRepository.AddAsync(walkDifficultyDomain);
            //Convert domain to dto
           var repoDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficultyDomain);
            //return CreatedAtAction(repoDTO);
            return Ok(repoDTO);
            //return Ok(walkDifficultyDomain);

        }
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkdifficultyAsync(Guid id,Models.DTO.UpdateWalkdifficultyRequest updateWalkdifficultyRequest)
        {
            //convert DTO to domain
            var walkDifficultyDomain = new Models.Domain.WalkDifficulty
            {
                Code = updateWalkdifficultyRequest.Code,
            };
            //Call Repository to update
           walkDifficultyDomain = await  walkDifficultyRepository.UpdateAsync(id, walkDifficultyDomain);
            if(walkDifficultyDomain == null)
            {
                return NotFound();  
            }
            var repoDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficultyDomain);
            return Ok(repoDTO);

        }
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkdifficulty(Guid id)
        {
          var walkDifficultyDomain =   await walkDifficultyRepository.DeleteAsync(id);
            if(walkDifficultyDomain == null)
            {
                return NotFound();
            }
            //convert to DTO
            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficultyDomain);
            return Ok(walkDifficultyDTO);
        }



    }
}
