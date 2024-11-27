using Microsoft.EntityFrameworkCore;
using Room.Query.Domain.Entities;
using Room.Query.Domain.Repository;
using Room.Query.Infrastructure.DataAccess;

namespace Room.Query.Infrastructure.Repository
{
    public class RoomRepository : IRoomRepository
    {
        private readonly DataBaseContextFactory _contextFactory;
        public RoomRepository(DataBaseContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task CreatAsync(RoomEntity room)
        {
            using DataBaseContext context = _contextFactory.CreateDbContext();
            try
            {
                context.Add(room);
                await context.SaveChangesAsync();
            }
            catch (Exception ex) {
                throw;
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            using DataBaseContext context = _contextFactory.CreateDbContext();
            var Room = await GetByIdAsync(id);
            if(Room == null)
            {
                return;
            }
            try
            {
                context.Remove(Room);
                await context.SaveChangesAsync();
            }
            catch (Exception ex) {
                throw;
            }

        }

        public async Task<List<RoomEntity>> GetAllAsync()
        {
            using DataBaseContext context = _contextFactory.CreateDbContext();
            return await context.Rooms.ToListAsync();
        }

        public async Task<RoomEntity> GetByIdAsync(Guid id)
        {
            using DataBaseContext context = _contextFactory.CreateDbContext();
            return await context.Rooms.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<RoomEntity> GetByNumberAsync(string RoomNumber)
        {
            using DataBaseContext context = _contextFactory.CreateDbContext();
            return await context.Rooms.FirstOrDefaultAsync(r => r.RoomNumber == RoomNumber);
        }

        public Task<List<RoomEntity>> GetByPrice(int min, int max)
        {
            // to do
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(RoomEntity room)
        {
            using DataBaseContext context = _contextFactory.CreateDbContext();
            context.Update(room);
            try
            {
                await context.SaveChangesAsync();
            }
            catch (Exception ex) {
                throw;
            }
        }
    }
}
