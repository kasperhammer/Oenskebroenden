using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DtoModels;
using Models.Forms;
using Repo;
using System.Security.Claims;

namespace API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class HistoryController : ControllerBase
    {
        private readonly IHistoryRepo repo;

        public HistoryController(IHistoryRepo repo)
        {
            this.repo = repo;
        }

        [HttpPost("AddHistory")]
        public async Task<IActionResult> AddHistory(int wishListId)
        {
            if (wishListId != null)
            {
                var claims = User.Claims;
                string idFromToken = claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                int idFromTokenInt = Convert.ToInt32(idFromToken);

                if (await repo.AddHistory(new HistoryForm { UserId = idFromTokenInt, WishListId = wishListId }))
                {
                    return Ok();
                }
            }

            return BadRequest(); // Returnerer fejlmeddelelse, hvis oprettelsen mislykkes
        }
    }
}
