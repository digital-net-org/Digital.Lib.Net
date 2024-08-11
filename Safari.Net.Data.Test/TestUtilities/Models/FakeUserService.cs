using System.Linq.Expressions;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Safari.Net.Core.Messages;
using Safari.Net.Core.Predicates;
using Safari.Net.Data.Entities;
using Safari.Net.Data.Repositories;

namespace Safari.Net.Data.Test.TestUtilities.Models;

public class FakeUserService(IRepository<FakeUser> repository)
    : EntityService<FakeUser, FakeUserQuery>(repository)
{
    protected override Expression<Func<FakeUser, bool>> Filter(FakeUserQuery query)
    {
        var filter = PredicateBuilder.New<FakeUser>();
        if (!string.IsNullOrWhiteSpace(query.Username))
            filter = filter.Add(x => x.Username.StartsWith(query.Username));
        if (!string.IsNullOrWhiteSpace(query.Email))
            filter = filter.Add(x => x.Email.StartsWith(query.Email));
        if (query.Role != null)
            filter = filter.Add(x => x.Role == query.Role);

        return filter;
    }

    protected override void ValidatePatch(Operation<FakeUser> patch)
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
