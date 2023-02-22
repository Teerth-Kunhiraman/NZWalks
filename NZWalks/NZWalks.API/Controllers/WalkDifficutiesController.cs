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
            //Validate
            if(! ValidateAddWalkdifficultyAsync(addWalkDifficultyRequest))
            {
                return BadRequest(ModelState);
            }
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
           //Validate
           if(! ValidateUpdateWalkdifficultyAsync(updateWalkdifficultyRequest))
            {
                return BadRequest(ModelState);
            }
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

        #region Private methods
        private bool  ValidateAddWalkdifficultyAsync(Models.DTO.AddWalkDifficultyRequest addWalkDifficultyRequest)
        {
           if(addWalkDifficultyRequest == null)
            {
                ModelState.AddModelError(nameof(addWalkDifficultyRequest), $"{nameof(addWalkDifficultyRequest)}cannot be null or empty or white space");
                return false;
            }
            if(string.IsNullOrEmpty(addWalkDifficultyRequest.Code))
            {
                ModelState.AddModelError(nameof(addWalkDifficultyRequest.Code), $"{nameof(addWalkDifficultyRequest.Code)}cannot be null or empty or white space");
                return false;
            }
            return true;
        }
        private bool ValidateUpdateWalkdifficultyAsync(Models.DTO.UpdateWalkdifficultyRequest updateWalkdifficultyRequest)
        {
            if (updateWalkdifficultyRequest == null)
            {
                ModelState.AddModelError(nameof(updateWalkdifficultyRequest), $"{nameof(updateWalkdifficultyRequest)}cannot be null or empty or white space");
                return false;
            }
            if (string.IsNullOrEmpty(updateWalkdifficultyRequest.Code))
            {
                ModelState.AddModelError(nameof(updateWalkdifficultyRequest.Code), $"{nameof(updateWalkdifficultyRequest.Code)}cannot be null or empty or white space");
                return false;
            }
            return true;
        }
        #endregion

    }
}
