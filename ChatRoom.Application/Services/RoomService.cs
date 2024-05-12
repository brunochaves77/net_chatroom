using ChatRoom.Domain.Entities;
using ChatRoom.Domain.Interfaces;
using ChatRoom.Repository.Repositories;

namespace ChatRoom.Application.Services {
    public class RoomService : ServiceBase<Room>, IServiceBase<Room> {

        private readonly RoomRepository _roomRepository;

        public RoomService(RoomRepository roomRepository) : base(roomRepository) {
            _roomRepository = roomRepository;
        }

    }
}
