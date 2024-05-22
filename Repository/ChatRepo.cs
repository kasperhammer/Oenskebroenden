using Models.DtoModels;
using Models.Forms;
using ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class ChatRepo : IChatRepo
    {
        ChatService service;
        public ITokenRepo tokenRepo;

        public ChatRepo(ITokenRepo tokenRepo)
        {
            this.tokenRepo = tokenRepo;
            service = new();

        }


        public async Task<bool> SendMessageAsync(UserDTO cookie, ChatMessageForm message)
        {
            cookie = await tokenRepo.TokenValidationPackageAsync(cookie);
            if (cookie != null & message != null)
            {
                return await service.AddMessageAsync(message, cookie.Token);
            }
            return false;
        }

        public async Task<ChatLobbyDTO> GetChatLobbyAsync(UserDTO cookie, int wishListId)
        {
            cookie = await tokenRepo.TokenValidationPackageAsync(cookie);
            if (cookie != null & wishListId != 0)
            {
                return await service.GetChatAsync(wishListId, cookie.Token);
            }
            return null;
        }

    }
}
