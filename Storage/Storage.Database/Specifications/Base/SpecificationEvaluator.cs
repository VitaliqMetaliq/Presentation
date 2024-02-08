using Microsoft.EntityFrameworkCore;

namespace Storage.Database.Specifications.Base;

public class SpecificationEvaluator<T> where T : class
{
    public static IQueryable<T> GetQuery(IQueryable<T> query, IBaseSpecifications<T> specifications)
    {
        if (specifications == null)
        {
            return query;
        }

        if (specifications.FilterCondition != null)
        {
            query = query.Where(specifications.FilterCondition);
        }

        query = specifications.Includes.
            Aggregate(query, (current, include) => current.Include(include));

        return query;
    }
}
