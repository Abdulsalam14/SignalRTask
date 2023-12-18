using WebApiSignalR.Entities;

namespace WebApiSignalR.Repositories
{
    public interface IRoomRepository
    {
        public IEnumerable<Room> Getall();
        public Room Get(string name);
        public void Update(Room room);
    }
}
