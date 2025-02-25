using System.Text.Json;
using Digital.Lib.Net.Core.Messages;
using Digital.Lib.Net.Core.Models;
using Digital.Lib.Net.Entities.Models;
using Digital.Lib.Net.Entities.Services;
using Digital.Lib.Net.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Digital.Lib.Net.Mvc.Controllers.Crud;

[ApiController, Route("[controller]")]
public abstract class CrudController<T, TContext, TDto, TPayload>(
    IEntityService<T, TContext> entityService
) : ControllerBase
    where T : Entity
    where TContext : DbContext
    where TDto : class
    where TPayload : class
{
    [HttpGet("schema")]
    public virtual ActionResult<Result<List<SchemaProperty<T>>>> GetSchema() =>
        Ok(new Result<List<SchemaProperty<T>>>(entityService.GetSchema()));

    [HttpGet("{id}")]
    public virtual ActionResult<Result<TDto>> GetById(string id)
    {
        var result = new Result<TDto>();

        if (Guid.TryParse(id, out var guidId))
            result = entityService.Get<TDto>(guidId);
        else if (int.TryParse(id, out var intId))
            result = entityService.Get<TDto>(intId);
        else
            result.AddError(new KeyNotFoundException("Entity not found."));

        return result.HasError() ? NotFound(result) : Ok(result);
    }

    [HttpPost("")]
    public virtual async Task<ActionResult<Result>> Post([FromBody] TPayload payload)
    {
        var result = await entityService.Create(Mapper.Map<TPayload, T>(payload));
        return result.HasError() ? BadRequest(result) : Ok(result);
    }

    [HttpPatch("{id}")]
    public virtual async Task<ActionResult<Result>> Patch(string id, [FromBody] JsonElement patch)
    {
        var result = new Result();

        if (Guid.TryParse(id, out var guidId))
            result = await entityService.Patch(JsonFormatter.GetPatchDocument<T>(patch), guidId);
        else if (int.TryParse(id, out var intId))
            result = await entityService.Patch(JsonFormatter.GetPatchDocument<T>(patch), intId);
        else
            result.AddError(new KeyNotFoundException("Entity not found."));

        if (result.HasError() && result.HasError<KeyNotFoundException>())
            return NotFound(result);
        if (result.HasError() && result.HasError<InvalidOperationException>())
            return BadRequest(result);

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public virtual async Task<ActionResult<Result>> Delete(string id)
    {
        var result = new Result();

        if (Guid.TryParse(id, out var guidId))
            result = await entityService.Delete(guidId);
        else if (int.TryParse(id, out var intId))
            result = await entityService.Delete(intId);
        else
            result.AddError(new KeyNotFoundException("Entity not found."));

        return result.HasError() ? NotFound(result) : Ok(result);
    }

    [NonAction]
    public bool IsGetSchemaExecution() => ControllerContext.ActionDescriptor.ActionName == "GetSchema";

    [NonAction]
    public bool IsGetExecution() => ControllerContext.ActionDescriptor.ActionName == "GetById";

    [NonAction]
    public bool IsPostExecution() => ControllerContext.ActionDescriptor.ActionName == "Post";

    [NonAction]
    public bool IsPatchExecution() => ControllerContext.ActionDescriptor.ActionName == "Patch";

    [NonAction]
    public bool IsDeleteExecution() => ControllerContext.ActionDescriptor.ActionName == "Delete";
}