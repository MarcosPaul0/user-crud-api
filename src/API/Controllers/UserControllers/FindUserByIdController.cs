using Microsoft.AspNetCore.Mvc;
using UserCrud.Domain.Entities;

namespace UserCrud.API.Controllers.UserControllers
{
    [ApiController]
    [Route("api/user")]
    public class FindUserByIdController : ControllerBase
    {
        private readonly ILogger<FindUserByIdController> _logger;

        public FindUserByIdController(ILogger<FindUserByIdController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{userId}")]
        public async Task<User> Handle(string userId)
        {
            throw new NotImplementedException("Route not implemented");
        }

    }
}