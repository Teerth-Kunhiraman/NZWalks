using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using System.Reflection.Metadata.Ecma335;

namespace NZWalks.API.Repositories
{
    public class RegionRepository : IRegionRepository
    {
        private readonly NZWalksDbContext nZWalksDbContext;
        public RegionRepository(NZWalksDbContext nZWalksDbContext)
        {
            this.nZWalksDbContext = nZWalksDbContext;
        }

       

        public async Task<IEnumerable<Region>> GetAllAsync()
        {
            return await nZWalksDbContext.Regions.ToListAsync();
        }

        //public Task<IEnumerable<Region>> GetAllAsync()
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<Region> GetAsync(Guid id)
        {
            return await nZWalksDbContext.Regions.FirstOrDefaultAsync(u => u.Id == id); 
        }

        public async Task<Region> AddSync(Region region)
        {
            region.Id = Guid.NewGuid();
            await nZWalksDbContext.AddAsync(region);
            await nZWalksDbContext.SaveChangesAsync();
            return region;
        }

        public async Task<Region> DeleteAsync(Guid id)
        {
            var region =  await nZWalksDbContext.Regions.FirstOrDefaultAsync(u => u.Id == id);
            if (region != null)
            {
                nZWalksDbContext.Regions.Remove(region);
                await nZWalksDbContext.SaveChangesAsync();
                return region;
            }
            else
            {
                return null;
            }

        }

        public async Task<Region> UpdateAsync(Guid id, Region region)
        {
           var Existingregion = await  nZWalksDbContext.Regions.FirstOrDefaultAsync(u => u.Id == id);
            if (Existingregion != null)
            {
                Existingregion.Code= region.Code;
                Existingregion.Name= region.Name;
                Existingregion.Area= region.Area;
                Existingregion.Lat= region.Lat;
                Existingregion.Long = region.Long;  
                Existingregion.Population= region.Population;
                await nZWalksDbContext.SaveChangesAsync();
                return Existingregion;
            }
            else
            {
                return null;
            }

        }
    }
}
