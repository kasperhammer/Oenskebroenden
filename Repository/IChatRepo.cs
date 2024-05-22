using Models.DtoModels;
using Models.Forms;

namespace Repository
{
    public interface IChatRepo
    {
        Task<ChatLobbyDTO> GetChatLobbyAsync(UserDTO cookie, int wishListId);
        Task<bool> SendMessageAsync(UserDTO cookie, ChatMessageForm message);
    }
}