using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AMNSystemsERP.Api.Controllers
{
    //[EnableCors]
    [Authorize(AuthenticationSchemes = "ERPAuthScheme")]
    public abstract class ApiController : ControllerBase
    {
    }
}
