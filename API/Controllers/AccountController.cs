using Microsoft.AspNetCore.Mvc;
using Models.DtoModels;
using Models.Forms;
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
        public async Task<IActionResult> CreateAccountAsync(UserCreateForm user)
        {
            //Jeg for kaldt min CreateAccount Metode med den UserModel som bliver sent med fra brugeren
            //Derefter retunere min metode en boolsk verdi der fortæller mig om det var en success eller ej.
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
