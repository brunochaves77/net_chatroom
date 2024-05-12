using System.Linq.Expressions;

namespace ChatRoom.Domain.Interfaces {
    public interface IServiceBase<Entity> {

        int Commit();

        void Add(Entity entity);
        void Update(Entity entity);
        void Delete(Entity entity);

        IEnumerable<Entity> Get(Expression<Func<Entity, bool>> where, bool tracking = false);
        Entity? GetFirstOrDefault(Expression<Func<Entity, bool>> where, bool tracking = false);

    }
}
