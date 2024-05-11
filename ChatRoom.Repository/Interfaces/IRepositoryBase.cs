using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.Repository.Interfaces {
    public interface IRepositoryBase<Entity> {

        int Commit();

        void Add(Entity entity);
        void Update(Entity entity);
        void Delete(Entity entity);

        IEnumerable<Entity> Get(Expression<Func<Entity, bool>> where, bool tracking = false);
        Entity? GetFirstOrDefault(Expression<Func<Entity, bool>> where, bool tracking = false);

    }
}
