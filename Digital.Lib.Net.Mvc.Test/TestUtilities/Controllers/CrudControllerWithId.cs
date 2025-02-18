using Digital.Lib.Net.Entities.Services;
using Digital.Lib.Net.Mvc.Controllers.Crud;
using Digital.Lib.Net.Mvc.Test.TestUtilities.Context;

namespace Digital.Lib.Net.Mvc.Test.TestUtilities.Controllers;

public class CrudControllerWithId(IEntityService<TestIdEntity, MvcTestContext> entityService)
    : CrudController<TestIdEntity, MvcTestContext, TestIdEntityDto, TestIdEntityPayload>(entityService);