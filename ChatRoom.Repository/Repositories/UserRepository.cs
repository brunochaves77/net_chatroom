using ChatRoom.Domain.Entities;
using ChatRoom.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.Repository.Repositories {
    public class UserRepository : RepositoryBase<User>, IRepositoryBase<User>  {

        public UserRepository(AppDataContext context) : base(context) { }

    }
}
