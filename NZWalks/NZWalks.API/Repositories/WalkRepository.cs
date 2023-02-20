using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class WalkRepository : IWalkRepository
    {
        private readonly NZWalksDbContext nZWalksDbContext;
        public WalkRepository(NZWalksDbContext nZWalksDbContext)
        {
            this.nZWalksDbContext = nZWalksDbContext;
        }

        public async Task<Walk> AddAsync(Walk walk)
        {
            //assig new id
            walk.Id = Guid.NewGuid();
            await nZWalksDbContext.Walks.AddAsync(walk);
            await nZWalksDbContext.SaveChangesAsync();
            return walk; 
        }

        public async Task<Walk> DeleteAsync(Guid id)
        {
            var ExistingWalk = await nZWalksDbContext.Walks.FindAsync(id);
            if (ExistingWalk != null)
            {
                nZWalksDbContext.Walks.Remove(ExistingWalk);
                await nZWalksDbContext.SaveChangesAsync();
                return ExistingWalk;
            }
            else
            {
                return null;
            }
        }

        public async Task<IEnumerable<Walk>> GetAllAsync()
        {
           return  await nZWalksDbContext.Walks
                .Include(x=> x.Region)
                .Include(x=> x.WalkDifficulty)
                .ToListAsync();
        }

        public  Task<Walk> GetAsync(Guid id)
        {
           return nZWalksDbContext.Walks
                .Include(x => x.Region)
                .Include(x => x.WalkDifficulty)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Walk> UpdateAsync(Guid id, Walk walk)
        {
            var ExistingWalk = await nZWalksDbContext.Walks.FindAsync(id);
            if (ExistingWalk != null)
            {
                ExistingWalk.Length = walk.Length;
                ExistingWalk.Name = walk.Name;
                ExistingWalk.WalkDifficultyId = walk.WalkDifficultyId;
                ExistingWalk.RegionId = walk.RegionId;
                await nZWalksDbContext.SaveChangesAsync();
                return ExistingWalk;
            }
            else
            {
                return null;
            }
        }
    }
}
