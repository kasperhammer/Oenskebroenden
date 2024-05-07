using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DtoModels;
using Models.EntityModels;
using Repo;

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

      

        [HttpGet]
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
        public async Task<IActionResult> CreateWishListAsync(WishListDTO wishListDTO)
        {
            if (await repo.CreateWishlistAsync(wishListDTO))
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
    }
}
