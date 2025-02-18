using Digital.Lib.Net.Entities.Repositories;
using Digital.Lib.Net.Mvc.Controllers.Pagination;
using Digital.Lib.Net.Mvc.Test.TestUtilities.Context;

namespace Digital.Lib.Net.Mvc.Test.TestUtilities.Controllers;

public class PaginationControllerWithId(IRepository<TestIdEntity, MvcTestContext> repository)
    : PaginationController<TestIdEntity, MvcTestContext, TestIdEntityDto, TestIdEntityQuery>(repository);