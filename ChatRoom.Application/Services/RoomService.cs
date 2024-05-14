using ChatRoom.Domain.Entities;
using ChatRoom.Domain.Interfaces;
using ChatRoom.Repository.Repositories;

namespace ChatRoom.Application.Services {
    public class RoomService : ServiceBase<Room>, IServiceBase<Room> {

        private readonly RoomRepository _roomRepository;

        public RoomService(RoomRepository roomRepository) : base(roomRepository) {
            _roomRepository = roomRepository;
        }
        
        public IEnumerable<Room> GetAllRooms() 
        {
            return this.GetAll();
        }

        public Room? GetRoomById(Guid id) 
        {
            return  GetFirstOrDefault(x => x.Id == id);
        }

        public void CreateRoom(Room room) 
        {
            Add(room);
            Commit();
        }

        public void DeleteRoom(Room room) 
        {
            Delete(room);
            Commit();
        }

    }
}
