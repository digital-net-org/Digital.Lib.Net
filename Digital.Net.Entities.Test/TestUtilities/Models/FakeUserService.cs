using System.Linq.Expressions;
using Digital.Net.Core.Predicates;
using Digital.Net.Entities.Repositories;
using Digital.Net.Entities.Services;

namespace Digital.Net.Entities.Test.TestUtilities.Models;

public class FakeUserService(IRepository<FakeUser> repository)
    : EntityService<FakeUser, FakeUserQuery>(repository)
{
    protected override Expression<Func<FakeUser, bool>> Filter(
        Expression<Func<FakeUser, bool>> predicate,
        FakeUserQuery query
    )
    {
        if (!string.IsNullOrWhiteSpace(query.Username))
            predicate = predicate.Add(x => x.Username.StartsWith(query.Username));
        if (!string.IsNullOrWhiteSpace(query.Email))
            predicate = predicate.Add(x => x.Email.StartsWith(query.Email));
        if (query.Role != null)
            predicate = predicate.Add(x => x.Role == query.Role);
        return predicate;
    }
}
