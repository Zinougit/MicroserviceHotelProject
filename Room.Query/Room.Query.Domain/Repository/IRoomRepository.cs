using Room.Query.Domain.Entities;
using System.Runtime.InteropServices;

namespace Room.Query.Domain.Repository
{
    public interface IRoomRepository
    {
        Task CreatAsync(RoomEntity room);
        Task UpdateAsync(RoomEntity room);
        Task DeleteAsync(Guid id);
        Task<RoomEntity> GetByIdAsync(Guid id);
        Task<RoomEntity> GetByNumberAsync(string RoomNumber);
        Task<List<RoomEntity>> GetAllAsync();
        Task<List<RoomEntity>> GetByPrice(int min, int max);
    }
}
