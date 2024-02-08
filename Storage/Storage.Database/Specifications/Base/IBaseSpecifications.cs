using System.Linq.Expressions;

namespace Storage.Database.Specifications.Base;

public interface IBaseSpecifications<T>
{
    Expression<Func<T, bool>> FilterCondition { get; }
    List<Expression<Func<T, object>>> Includes { get; }
}
