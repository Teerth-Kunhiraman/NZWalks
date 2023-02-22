using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalksController : Controller
    {
        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;
        private readonly IRegionRepository regionRepository;
        private readonly IWalkDifficultyRepository walkDifficultyRepository;
        public WalksController(IWalkRepository walkRepository, IMapper mapper,IRegionRepository regionRepository,IWalkDifficultyRepository walkDifficultyRepository)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllWalksAsync()
        {
            //Fetch data from database
            var walkDomain = await walkRepository.GetAllAsync();
            //Convert domain walk to dto walk
            var walkDTO = mapper.Map<List<Models.DTO.Walk>>(walkDomain);
            //return response
            return Ok(walkDTO);
        }
        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkAsync")]
        public async Task<IActionResult>  GetWalkAsync(Guid id)
        {
            var walkDomain = await walkRepository.GetAsync(id);
            var walkDTO = mapper.Map<Models.DTO.Walk>(walkDomain);
            return Ok(walkDTO);
        }
        [HttpPost]
        public async Task<IActionResult> AddWalkAsync([FromBody] Models.DTO.AddWalkRequest addWalkRequest)
        {
            //Validate kiya hai side mai reh jaa
            if(!(await ValidateAddWalkAsync(addWalkRequest)))
            {
                return BadRequest(ModelState);
            }
            //Convert DTO to Domain object
            var walkDomain = new Models.Domain.Walk
            {
                Length = addWalkRequest.Length,
                Name = addWalkRequest.Name,
                RegionId = addWalkRequest.RegionId,
                WalkDifficultyId = addWalkRequest.WalkDifficultyId
            };
            //Pass Domain object to Repository
             walkDomain = await walkRepository.AddAsync(walkDomain);
            //Convert Domain object to DTO
            var walkDTO = new Models.DTO.Walk
            {
                Id = walkDomain.Id,
                Length = walkDomain.Length,
                Name = walkDomain.Name,
                RegionId = walkDomain.RegionId,
                WalkDifficultyId = walkDomain.WalkDifficultyId

            };
            //SendResponsenback to client
            return CreatedAtAction(nameof(GetWalkAsync), new { id = walkDTO.Id }, walkDTO);

        }
        [HttpPut]
        public async Task<IActionResult> UpdateWalkAsync([FromRoute] Guid id, [FromBody] Models.DTO.UpdateWalkRequest updateWalkRequest)
        {
            //Validate
            if (!(await ValidateUpdateWalkAsync(updateWalkRequest)))
            {
                return BadRequest(ModelState);
            }
            else
            {
                //Convert Dto to domain object
                var walkDomain = new Models.Domain.Walk
                {

                    Length = updateWalkRequest.Length,
                    Name = updateWalkRequest.Name,
                    RegionId = updateWalkRequest.RegionId,
                    WalkDifficultyId = updateWalkRequest.WalkDifficultyId
                };
                //Pass details to repo
                walkDomain = await walkRepository.UpdateAsync(id, walkDomain);

                //Handel null
                if (walkDomain != null)
                {
                    //Convert back domain to dto
                    var walkDTO = new Models.DTO.Walk
                    {
                        Id = walkDomain.Id,
                        Length = walkDomain.Length,
                        Name = walkDomain.Name,
                        RegionId = walkDomain.RegionId,
                        WalkDifficultyId = walkDomain.WalkDifficultyId

                    };
                    return Ok(walkDTO);

                }
                else
                {
                    return NotFound();
                }
            }
           
        }
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkAsync(Guid id)
        {
            //call repo to delete walk;
            var walkDomain = await walkRepository.DeleteAsync(id);
            if(walkDomain!=null)
            {
                var walkDTO = mapper.Map<Models.DTO.Walk>(walkDomain);
                return Ok(walkDTO);
            }
            else
            {
                return NotFound();
            }
        }
        #region Private Methods
        private async Task<bool> ValidateAddWalkAsync(Models.DTO.AddWalkRequest addWalkRequest)
        {
            if(addWalkRequest == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest), $"{nameof(addWalkRequest)}It must be there");
                return false;
            }
            if(string.IsNullOrEmpty(addWalkRequest.Name))
            {
                ModelState.AddModelError(nameof(addWalkRequest.Name), $"{nameof(addWalkRequest.Name)}Name is must");
                return false;
            }
            if(addWalkRequest.Length>0)
            {
                ModelState.AddModelError(nameof(addWalkRequest.Length), $"{nameof(addWalkRequest.Length)}Length is must");
                return false;
            }
           var region = regionRepository.GetAsync(addWalkRequest.RegionId);
            if(region==null)
            {
                ModelState.AddModelError(nameof(addWalkRequest.RegionId), $"{nameof(addWalkRequest.RegionId)}RegionId is InValid");
                return false;
            }
            var difficulty = walkDifficultyRepository.GetAsync(addWalkRequest.WalkDifficultyId);
                if(difficulty==null)
            {
                ModelState.AddModelError(nameof(addWalkRequest.WalkDifficultyId), $"{nameof(addWalkRequest.WalkDifficultyId)}RegionId is InValid");
                return false;
            }
            return true;




        }
        private async  Task<bool> ValidateUpdateWalkAsync(Models.DTO.UpdateWalkRequest updateWalkRequest)
        {
            if (updateWalkRequest == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest), $"{nameof(updateWalkRequest)}It must be there");
                return false;
            }
            if (string.IsNullOrEmpty(updateWalkRequest.Name))
            {
                ModelState.AddModelError(nameof(updateWalkRequest.Name), $"{nameof(updateWalkRequest.Name)}Name is must");
                return false;
            }
            if (updateWalkRequest.Length < 0)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.Length), $"{nameof(updateWalkRequest.Length)}Length is must");
                return false;
            }
            var region = regionRepository.GetAsync(updateWalkRequest.RegionId);
            if (region == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.RegionId), $"{nameof(updateWalkRequest.RegionId)}RegionId is InValid");
                return false;
            }
            var difficulty = walkDifficultyRepository.GetAsync(updateWalkRequest.WalkDifficultyId);
            if (difficulty == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.WalkDifficultyId), $"{nameof(updateWalkRequest.WalkDifficultyId)}RegionId is InValid");
                return false;
            }
            return true;

        }
        #endregion
    }
}
