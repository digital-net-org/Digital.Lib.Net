using Digital.Lib.Net.Core.Messages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Digital.Lib.Net.Authentication.Extensions;

public static class AuthorizationFilterResult
{
    public static Result RejectAuthorization(this AuthorizationFilterContext context, int statusCode)
    {
        var result = new Result();
        result.AddError(new UnauthorizedAccessException());
        context.Result = new JsonResult(result) { StatusCode = statusCode };
        return result;
    }
}