using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DtoModels;
using Models.Forms;
using Repo;

namespace API.Controllers
{

    /*

    Get User ID from Claims : 
    var claims = User.Claims;
    string idFromToken = claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
    int idFromTokenInt = Convert.ToInt32(idFromToken);

    Get User Name from Claims : 
    User.Identity.Name

    Get Users Token from Claims : 
    var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
    */



    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {

        public IAccountRepo repo;
        public AccountController(IAccountRepo repo)
        { this.repo = repo; }



        [HttpPost("CreateAccount")]
        public async Task<IActionResult> CreateAccountAsync(UserCreateForm user)
        {
            //Jeg for kaldt min CreateAccount Metode med den UserModel som bliver sent med fra brugeren
            //Derefter retunere min metode en boolsk verdi der fortæller mig om det var en success eller ej.
            if (user != null)
            {
                UserDTO userDTO = await repo.CreateAccountAsync(user);


                if (userDTO != null)
                {
                    return Ok(userDTO);
                }


            }
            return BadRequest();
        }


        [HttpPost("Login")]
        [AllowAnonymous]
        //Min Login Metode tager imod en persons username og password
        //og usteder så en UserModel med en JWT Token i såfremt at ens credentiels er valid
        public async Task<IActionResult> Login(string userName, string password)
        {
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
            {
                UserDTO person = await repo.LoginAsync(new UserDTO { Password = password, Name = userName });
                if (person != null)
                {
                    return Ok(person);
                }
            }
            return BadRequest();
        }

        [HttpGet("Validate")]
        //Denne metode bliver brugt til at validere om en person stadig har adgang til Apien, eller om de skal logge ind igen
        //siden at Controlleren har Authorize på for man kun en OK såfremt ens token stadig er Valid,
        public async Task<IActionResult> ValidateTokenAsync()
        {
            return Ok();
        }

        [HttpPost("RefreshToken")]
        //Denne metode udsteder ens userModel med en ny Token som har en ny levetid.
        public async Task<IActionResult> RefreshTokenAsync(UserDTO person)
        {
            if (person != null)
            {
                if (User.Identity.Name == person.Name)
                {
                    UserDTO personDb = await repo.RefreshTokenAsync(person);
                    if (personDb != null)
                    {
                        return Ok(personDb);

                    }
                }
            }
            return NotFound();
        }



    }
}
