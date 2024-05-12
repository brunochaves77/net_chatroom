using ChatRoom.Domain.Entities;
using ChatRoom.Domain.Interfaces;

namespace ChatRoom.Repository.Repositories {
    public class UserRepository : RepositoryBase<User>, IRepositoryBase<User>  {

        public UserRepository(AppDataContext context) : base(context) { }

    }
}
