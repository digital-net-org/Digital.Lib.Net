using Digital.Lib.Net.Entities.Repositories;
using Digital.Lib.Net.Mvc.Controllers.Pagination;
using InternalTestProgram.Models;

namespace Digital.Lib.Net.Mvc.Test.TestUtilities.Controllers;

public class PaginationControllerWithId(IRepository<TestIdEntity> repository) : PaginationController<
    TestIdEntity, TestIdEntityDto, TestIdEntityQuery>(repository);