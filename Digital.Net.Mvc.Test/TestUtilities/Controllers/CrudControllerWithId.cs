using Digital.Net.Entities.Services;
using Digital.Net.Mvc.Controllers.Crud;
using Digital.Net.Mvc.Test.TestUtilities.Models;
using Microsoft.AspNetCore.Http;

namespace Digital.Net.Mvc.Test.TestUtilities.Controllers;

public class CrudControllerWithId(
    IHttpContextAccessor contextAccessor,
    IEntityService<TestIdEntity> entityService
) : CrudController<TestIdEntity, TestIdEntityDto, TestIdEntityPayload>(contextAccessor, entityService);