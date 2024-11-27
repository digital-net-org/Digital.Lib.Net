using System.Text.Json;
using Digital.Net.Core.Extensions.HttpUtilities;
using Digital.Net.Core.Messages;
using Digital.Net.Core.Models;
using Digital.Net.Entities.Models;
using Digital.Net.Entities.Services;
using Digital.Net.Mvc.Formatters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Digital.Net.Mvc.Controllers;

[Route("[controller]")]
public abstract class CrudController<T, TDto, TDtoLight, TQuery, TPayload>(
    IHttpContextAccessor contextAccessor,
    IEntityService<T, TQuery> entityService
) : ControllerBase
    where T : EntityBase
    where TDto : class
    where TDtoLight : class
    where TQuery : Query
    where TPayload : class
{
    [HttpGet("{id}")]
    public ActionResult<Result<TDto>> GetById(string id) // TODO: Test this
    {
        OnAuthorize(contextAccessor.GetContext());
        var result = new Result<TDto>();

        if (Guid.TryParse(id, out var guidId))
            result = entityService.Get<TDto>(guidId);
        else if (int.TryParse(id, out var intId))
            result = entityService.Get<TDto>(intId);
        else
            result.AddError(new KeyNotFoundException("Entity not found."));

        return result.HasError ? NotFound(result) : Ok(result);
    }

    [HttpGet("")]
    public ActionResult<QueryResult<TDtoLight>> Get([FromQuery] TQuery query)
    {
        OnAuthorize(contextAccessor.GetContext());
        return Ok(entityService.Get<TDtoLight>(query));
    }

    [HttpPost("")]
    public async Task<ActionResult<Result>> Post([FromBody] TPayload payload)
    {
        OnAuthorize(contextAccessor.GetContext());
        var result = await entityService.Create(Mapper.Map<TPayload, T>(payload));
        return result.HasError ? BadRequest(result) : Ok(result);
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult<Result>> Patch(string id, [FromBody] JsonElement patch)
    {
        OnAuthorize(contextAccessor.GetContext());
        var result = new Result();

        if (Guid.TryParse(id, out var guidId))
            result = await entityService.Patch(JsonFormatter.GetPatchDocument<T>(patch), guidId);
        else if (int.TryParse(id, out var intId))
            result = await entityService.Patch(JsonFormatter.GetPatchDocument<T>(patch), intId);
        else
            result.AddError(new KeyNotFoundException("Entity not found."));

        if (result.HasError && result.Errors[0].GetType() == typeof(KeyNotFoundException))
            return NotFound(result);
        if (result.HasError && result.Errors[0].GetType() == typeof(InvalidOperationException))
            return BadRequest(result);

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Result>> Delete(string id)
    {
        OnAuthorize(contextAccessor.GetContext());
        var result = new Result();

        if (Guid.TryParse(id, out var guidId))
            result = await entityService.Delete(guidId);
        else if (int.TryParse(id, out var intId))
            result = await entityService.Delete(intId);
        else
            result.AddError(new KeyNotFoundException("Entity not found."));

        return result.HasError ? NotFound(result) : Ok(result);
    }

    public virtual void OnAuthorize(HttpContext context) { }
}