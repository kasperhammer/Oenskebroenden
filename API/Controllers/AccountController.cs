using Microsoft.AspNetCore.Mvc;
using Models.DtoModels;
using Repo;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        public IAccountRepo repo;
        public AccountController(IAccountRepo repo)
        { this.repo = repo; }

        [HttpPost]
        public async Task<IActionResult> CreateAccountAsync(UserDTO user)
        {
            if (user  != null)
            {
                if (await repo.CreateAccountAsync(user))
                {
                    return Ok();
                }

            }
            return BadRequest();
        }

    }
}
