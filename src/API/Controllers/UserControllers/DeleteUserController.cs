using Microsoft.AspNetCore.Mvc;

namespace UserCrud.API.Controllers.UserControllers
{
    [ApiController]
    [Route("api/user")]
    public class DeleteUserController : ControllerBase
    {
        private readonly ILogger<FindUserByIdController> _logger;

        public DeleteUserController(ILogger<FindUserByIdController> logger)
        {
            _logger = logger;
        }

        [HttpDelete("{userId}")]
        public async Task Handle(string userId)
        {
            throw new NotImplementedException("Route not implemented");
        }
    }
}