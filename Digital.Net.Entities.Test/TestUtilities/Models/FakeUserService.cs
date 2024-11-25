using System.Linq.Expressions;
using Digital.Net.Core.Predicates;
using Digital.Net.Entities.Entities;
using Digital.Net.Entities.Repositories;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace Digital.Net.Entities.Test.TestUtilities.Models;

public class FakeUserService(IRepository<FakeUser> repository)
    : EntityService<FakeUser, FakeUserQuery>(repository)
{
    protected override Expression<Func<FakeUser, bool>> Filter(Expression<Func<FakeUser, bool>> predicate,
        FakeUserQuery query)
    {
        if (!string.IsNullOrWhiteSpace(query.Username))
            predicate = predicate.Add(x => x.Username.StartsWith(query.Username));
        if (!string.IsNullOrWhiteSpace(query.Email))
            predicate = predicate.Add(x => x.Email.StartsWith(query.Email));
        if (query.Role != null)
            predicate = predicate.Add(x => x.Role == query.Role);
        return predicate;
    }

    protected override void ValidatePatch(Operation<FakeUser> patch, FakeUser entity)
    {
        if (patch.path == "/Id")
            throw new InvalidOperationException("Id cannot be updated.");
        if (patch.path == "/Password")
            throw new InvalidOperationException("Password cannot be updated using PATCH endpoint.");
        if (patch.path == "/Username" && string.IsNullOrWhiteSpace(patch.value?.ToString()))
            throw new InvalidOperationException("Username cannot be empty.");
        if (patch.path == "/Email" && string.IsNullOrWhiteSpace(patch.value?.ToString()))
            throw new InvalidOperationException("Email cannot be empty.");
    }
}
