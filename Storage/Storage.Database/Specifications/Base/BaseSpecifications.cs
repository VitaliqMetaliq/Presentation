using System.Linq.Expressions;

namespace Storage.Database.Specifications.Base;

public abstract class BaseSpecifications<T> : IBaseSpecifications<T>
{
    private readonly List<Expression<Func<T, object>>> _includeCollection = new ();

    public Expression<Func<T, bool>> FilterCondition { get; private set; }
    public List<Expression<Func<T, object>>> Includes => _includeCollection;

    protected BaseSpecifications(Expression<Func<T, bool>> filterCondition)
    {
        FilterCondition = filterCondition;
    }

    protected BaseSpecifications()
    {
    }

    protected void AddInclude(Expression<Func<T, object>> includeExpression)
    {
        Includes.Add(includeExpression);
    }

    protected void SetFilterCondition(Expression<Func<T, bool>> filterExpression)
    {
        FilterCondition = filterExpression;
    }
}
