using WebApiSignalR.Data;
using WebApiSignalR.Entities;

namespace WebApiSignalR.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly RoomDbContext _dbContext;

        public RoomRepository(RoomDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Room Get(string name)
        {
            return _dbContext.Rooms.SingleOrDefault(a=>a.Name==name);
        }

        public IEnumerable<Room> Getall()
        {
            return _dbContext.Rooms;
        }

        public void Update(Room room)
        {
           _dbContext.Rooms.Update(room);
            _dbContext.SaveChanges();
        }
    }
}
