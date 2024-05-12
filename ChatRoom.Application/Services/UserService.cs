using ChatRoom.Domain.Entities;
using ChatRoom.Domain.Interfaces;
using ChatRoom.Repository.Repositories;

namespace ChatRoom.Application.Services {
    public class UserService : ServiceBase<User>, IServiceBase<User> {

        private readonly UserRepository _userRepository;

        public UserService(UserRepository userRepository) : base(userRepository) { 
            _userRepository = userRepository;
        }

    }
}
