using System.Linq.Expressions;

namespace Interfaces;

public interface IRepository<T>
{
    Task<IEnumerable<T>> FindAll();
    Task<T?> FindOne(Expression<Func<T, bool>> predicate);
    Task Create(T item);
    Task<bool> Update(Expression<Func<T, bool>> predicate, T item);
    Task<bool> Delete(Expression<Func<T, bool>> predicate);
}
