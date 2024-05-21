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
                //jeg henter min brugers ID fra min JWT token
                var claims = User.Claims;
                string idFromToken = claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                int idFromTokenInt = Convert.ToInt32(idFromToken);
                //Jeg kalder mit Repo og sender historikken med
                if (await repo.AddHistoryAsync(new HistoryForm { UserId = idFromTokenInt, WishListId = wishListId }))
                {
                    return Ok();
                }
            }

            return BadRequest(); // Returnerer fejlmeddelelse, hvis oprettelsen mislykkes
        }

        [HttpGet("GetHistory")]
        public async Task<IActionResult> GetHistores()
        {
            //jeg henter min brugers ID fra min JWT token
            var claims = User.Claims;
            string idFromToken = claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            int idFromTokenInt = Convert.ToInt32(idFromToken);

            //jeg henter brugerens historik ud fra hans Id
            List<HistoryDTO> histories = await repo.GetHistoryAsync(idFromTokenInt);
            if (histories != null)
            {
                if (histories.Count != 0)
                {
                    //Såfremt histokrikken ikke er tom retunere jeg den.
                    return Ok(histories);
                }
            }
            return BadRequest(); // Returnerer fejlmeddelelse, hvis oprettelsen mislykkes
        }
    }
}
