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
    //[Authorize]
    public class WishController : ControllerBase
    {

        public WishRepo repo;

        
        public WishController()
        { repo = new(); }

      

        [HttpGet("GetWishlists")]
        public async Task<IActionResult> GetWishlistsFromUser(int userId)
        {
            List<WishListDTO> rWishlists = await repo.GetWishlistsFromUser(userId);


            if(rWishlists != null && rWishlists.Count > 0)
            {
                return Ok(rWishlists);
            }
                       
            return BadRequest();
        }

        [HttpPost("CreateWishList")]
        public async Task<IActionResult> CreateWishListAsync(WishListDTO wishlist)
        {
            var claims = User.Claims;
            string idFromToken = claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            wishlist.OwnerId = Convert.ToInt32(idFromToken);
            if(wishlist.OwnerId<=0) return BadRequest();

            if (await repo.CreateWishlistAsync(wishlist))
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpPost("CreateWish")]
        public async Task<IActionResult> CreateWishAsync(WishDTO wishDTO)
        {

            if (await repo.CreateWishAsync(wishDTO))
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpDelete("DeleteWishlist")]
        public async Task<IActionResult> DeleteWishlist(int wishlistId)
        {

            if (await repo.DeleteWishListAsync(wishlistId))
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpDelete("DeleteWish")]
        public async Task<IActionResult> DeleteWish(int wishId)
        {

            if (await repo.DeleteWishAsync(wishId))
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}
