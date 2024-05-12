using ChatRoom.Domain.Entities;
using ChatRoom.Domain.Interfaces;

namespace ChatRoom.Repository.Repositories {
    public class RoomRepository : RepositoryBase<Room>, IRepositoryBase<Room> {

        public RoomRepository(AppDataContext context) : base(context) { }

    }
}
