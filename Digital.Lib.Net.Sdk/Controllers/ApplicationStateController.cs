using Digital.Lib.Net.Sdk.Services.Application;
using Microsoft.AspNetCore.Mvc;

namespace Digital.Lib.Net.Sdk.Controllers;

[ApiController]
public class ApplicationStateController(
    IApplicationService applicationService
) : ControllerBase
{
    [HttpGet("/")]
    public ActionResult<ApplicationVersion> GetVersion() => Ok(applicationService.GetVersion());
}