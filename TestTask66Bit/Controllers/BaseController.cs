using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestTask66Bit.Filters;

namespace TestTask66Bit.Controllers
{
    [TypeFilter<ApiExceptionFilter>]
    public class BaseController : ControllerBase
    {

    }
}
