using Microsoft.AspNetCore.Mvc;
using UserCrud.Domain.Entities;

namespace UserCrud.API.Controllers.UserControllers
{
    [ApiController]
    [Route("api/user")]
    public class UpdateUserController : ControllerBase
    {
        private readonly ILogger<FindUserByIdController> _logger;

        public UpdateUserController(ILogger<FindUserByIdController> logger)
        {
            _logger = logger;
        }

        [HttpPatch("{userId}")]
        public async Task Handle(string userId, [FromBody] User user)
        {
            throw new NotImplementedException("Route not implemented");
        }
    }
}