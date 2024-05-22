using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DtoModels;
using Models.Forms;
using Repo;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChatController : ControllerBase
    {
        private readonly IChatRepo repo;
        public ChatController(IChatRepo repo)
        {
            this.repo = repo;
        }

        [HttpPost("AddMessage")]
        public async Task<IActionResult> AddMessageAsync(ChatMessageForm message)
        {
            if (message != null)
            {

                if (await repo.AddMessageAsync(message))
                {
                    return Ok();
                }
            }
            return BadRequest();
        }

        [HttpGet("GetChat")]
        public async Task<IActionResult> GetChatAsync(int wishListId)
        {
            if (wishListId != 0)
            {
               ChatLobbyDTO lobby = await repo.GetChatLobbyAsync(wishListId);
                if (lobby != null)
                {
                    return Ok(lobby);
                }
            }
            return BadRequest();
        }
    }
}
