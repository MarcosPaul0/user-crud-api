using Microsoft.AspNetCore.Mvc;
using UserCrud.Domain.Entities;

namespace UserCrud.API.Controllers.UserControllers
{
    [ApiController]
    [Route("api/user")]
    public class CreateUserController : ControllerBase
    {
        private readonly ILogger<FindUserByIdController> _logger;

        public CreateUserController(ILogger<FindUserByIdController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public async Task Handle([FromBody] User user)
        {
            throw new NotImplementedException("Route not implemented");
        }
    }
}