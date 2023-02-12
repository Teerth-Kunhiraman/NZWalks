using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using System.Runtime;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class RegionsController : Controller
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;
        public RegionsController(IRegionRepository regionRepository, IMapper mapper)
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
            //
        }
        [HttpGet]
        public async Task<IActionResult> GetAllRegionsAsync()
        {
            var regions = await regionRepository.GetAllAsync();

            // return DTO regions
            //var regionsDTO = new List<Models.DTO.Region>();
            //regions.ToList().ForEach(region =>
            //{
            //    var regionDTO = new Models.DTO.Region()
            //    {
            //        Id = region.Id,
            //        Code = region.Code,
            //        Name = region.Name,
            //        Area = region.Area,
            //        Lat = region.Lat,
            //        Long = region.Long,
            //        Population = region.Population,
            //    };
            //    regionsDTO.Add(regionDTO);
            //});
            var regionsDTO = mapper.Map<List<Models.DTO.Region>>(regions);
            return Ok(regionsDTO);

        }
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetRegionsAsync(Guid id)
        {
            var region = await regionRepository.GetAsync(id);
            if (region == null)
            {
                return NotFound();
            }

            var regionDTO = mapper.Map<Models.DTO.Region>(region);
            return Ok(regionDTO);

        }
        [HttpPost]
        public async Task<IActionResult> AddRegionAsync(Models.DTO.AddRegionRequest addRegion)
        {
            //Request to domain model
            var region = new Models.Domain.Region()
            {
                Code = addRegion.Code,
                Area = addRegion.Area,
                Lat = addRegion.Lat,
                Long = addRegion.Long,
                Name = addRegion.Name,
                Population = addRegion.Population,
            };
            //Pass details to repository
            region = await regionRepository.AddSync(region);

            //Convert to dto
            var regionDTO = new Models.DTO.Region
            {
                Id = region.Id,
                Code = region.Code,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Name = region.Name,
                Population = region.Population,
            };
            return NoContent();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteRegionAsync(Guid id)
        {
            //Get region from 
            var region = await regionRepository.DeleteAsync(id);
            // if null not found
            if(region == null)
            {
                return null;
            }
            var regionDTO = new Models.DTO.Region
            {
                Id = region.Id,
                Code = region.Code,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Name = region.Name,
                Population = region.Population,

            };
            return Ok(regionDTO);
            //convert back to dto
            //return okresponse

        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateRegionAsync(Guid id,[FromBody]Models.DTO.UpdateRegionRequest updateRegionRequest)
        {
            var region = new Models.Domain.Region()
            {
                Code = updateRegionRequest.Code,
                Area = updateRegionRequest.Area,
                Lat = updateRegionRequest.Lat,
                Long = updateRegionRequest.Long,
                Name = updateRegionRequest.Name,
                Population = updateRegionRequest.Population,
            };
            region = await regionRepository.UpdateAsync(id, region);
            if(region==null)
            {
                return null;
            }
            else
            {
                var regionDTO = new Models.DTO.Region
                {
                    Id = region.Id,
                    Code = region.Code,
                    Area = region.Area,
                    Lat = region.Lat,
                    Long = region.Long,
                    Name = region.Name,
                    Population = region.Population,
                };
                return Ok(regionDTO);
            }
           

        }


    }
}
