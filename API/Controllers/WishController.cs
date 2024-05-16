using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DtoModels;
using Models.EntityModels;
using Models.Forms;
using Repo;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WishController : ControllerBase
    {

        public WishRepo repo;
        public WishController()
        { repo = new(); }


        // Metode til at hente ønskelister til en bestemt bruger
        [HttpGet("GetWishlists")]
        public async Task<IActionResult> GetWishlistsFromUser(int userId)
        {
            List<WishListDTO> rWishlists = await repo.GetWishlistsFromUser(userId);

            if (rWishlists != null && rWishlists.Count > 0)
            {
                return Ok(rWishlists); // Returnerer OK med ønskelister, hvis der er nogen
            }

            return BadRequest(); // Returnerer fejlmeddelelse, hvis ingen ønskelister blev fundet
        }

        [HttpGet("GetOneWishList")]
        public async Task<IActionResult> GetOneWishList(int wishListId)
        {
            WishListDTO rWishlists = await repo.GetOneWishList(wishListId);

            if (rWishlists != null)
            {
                return Ok(rWishlists); // Returnerer OK med ønskelister, hvis der er nogen
            }

            return BadRequest(); // Returnerer fejlmeddelelse, hvis ingen ønskelister blev fundet
        }


        // Metode til at oprette en ny ønskeliste
        [HttpPost("CreateWishList")]
        public async Task<IActionResult> CreateWishListAsync(WishListDTO wishlist)
        {
            wishlist.OwnerId = GetUserIdFromClaims(); // Tildeler ejerens ID fra JWT-claims
            if (wishlist.OwnerId <= 0) return BadRequest(); // Returnerer fejlmeddelelse, hvis ejerens ID ikke er gyldigt

            if (await repo.CreateWishlistAsync(wishlist))
            {
                return Ok(); // Returnerer OK, hvis oprettelsen lykkes
            }

            return BadRequest(); // Returnerer fejlmeddelelse, hvis oprettelsen mislykkes
        }


        // Metode til at oprette et nyt ønske
        [HttpPost("CreateWish")]
        public async Task<IActionResult> CreateWishAsync(WishDTO wishDTO)
        {

            if (await repo.CreateWishAsync(wishDTO))
            {
                return Ok(); // Returnerer OK, hvis oprettelsen lykkes
            }

            return BadRequest(); // Returnerer fejlmeddelelse, hvis oprettelsen mislykkes
        }


        // Metode til at slette en ønskeliste
        [HttpDelete("DeleteWishlist")]
        public async Task<IActionResult> DeleteWishlist(int wishlistId)
        {
            if (await repo.DeleteWishListAsync(wishlistId))
            {
                return Ok(); // Returnerer OK, hvis sletningen lykkes
            }

            return BadRequest(); // Returnerer fejlmeddelelse, hvis sletningen mislykkes
        }


        // Metode til at slette et ønske
        [HttpDelete("DeleteWish")]
        public async Task<IActionResult> DeleteWish(int wishId)
        {
            if (await repo.DeleteWishAsync(wishId))
            {
                return Ok(); // Returnerer OK, hvis sletningen lykkes
            }

            return BadRequest(); // Returnerer fejlmeddelelse, hvis sletningen mislykkes
        }


        // Metode til at reservere et ønske
        [HttpPut("ReserveWish")]
        public async Task<IActionResult> ReserveWishAsync(int wishId)
        {
            int userId = GetUserIdFromClaims(); // Henter brugerens ID fra JWT-claims
            if (userId <= 0) return BadRequest(); // Returnerer fejlmeddelelse, hvis brugerens ID ikke er gyldigt

            if (await repo.ReserveWishAsync(wishId, userId))
            {
                return Ok(); // Returnerer OK, hvis reservationen lykkes
            }
            return BadRequest(); // Returnerer fejlmeddelelse, hvis reservationen mislykkes
        }



        /// <summary>
        /// // Hjælpemetode til at hente brugerens ID fra JWT-claims
        /// </summary>
        /// <returns>Retunere brugerens id eller -1 hvis ERROR</returns>
        private int GetUserIdFromClaims()
        {
            var claims = User.Claims;
            string idFromToken = claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            int id = -1;
            int.TryParse(idFromToken, out id);

            return id;
        
        }
    }
}
