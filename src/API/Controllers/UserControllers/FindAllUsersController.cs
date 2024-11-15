using Microsoft.AspNetCore.Mvc;
using UserCrud.Domain.Entities;

namespace UserCrud.API.Controllers.UserControllers
{
    [ApiController]
    [Route("api/user")]
    public class FindAllUsersController : ControllerBase
    {
        private readonly ILogger<FindUserByIdController> _logger;

        public FindAllUsersController(ILogger<FindUserByIdController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<User>> Handle()
        {
            throw new NotImplementedException("Route not implemented");
        }
    }
}