using Digital.Lib.Net.Entities.Services;
using Digital.Lib.Net.Mvc.Controllers.Crud;
using InternalTestProgram.Models;

namespace Digital.Lib.Net.Mvc.Test.TestUtilities.Controllers;

public class CrudControllerWithGuid(IEntityService<TestGuidEntity> entityService)
    : CrudController<TestGuidEntity, TestGuidEntityDto, TestGuidEntityPayload>(entityService);