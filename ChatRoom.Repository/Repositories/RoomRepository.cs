using ChatRoom.Domain.Entities;
using ChatRoom.Domain.Interfaces;
using ChatRoom.Domain.Entities;
using ChatRoom.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ChatRoom.Repository.Repositories {
    public class RoomRepository : RepositoryBase<Room>, IRepositoryBase<Room> {

        public RoomRepository(AppDataContext context) : base(context) { }

    }
}
