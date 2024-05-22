using Models.DtoModels;
using Models.EntityModels;
using Models.Forms;

namespace Repo
{
    public interface IChatRepo
    {
        Task<bool> AddMessageAsync(ChatMessageForm message);
        Task<ChatLobbyDTO> GetChatLobbyAsync(int wishListId);
    }
}