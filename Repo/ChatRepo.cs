using DbAccess;
using Microsoft.EntityFrameworkCore;
using Models.EntityModels;
using Models.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repo
{
    public class ChatRepo
    {
        EntityContext dbLayer;
        AutoMapper autoMapper;

        public ChatRepo()
        {
            dbLayer = new();
            autoMapper = new();
        }

        public async Task<bool> AddMessageAsync(ChatMessageForm message)
        {
            if (message != null)
            {
                ChatMessage messageEnity = autoMapper.mapper.Map<ChatMessage>(message);
                await dbLayer.Messages.AddAsync(messageEnity);
                return await dbLayer.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<ChatLobby> GetChatLobbyAsync(int wishListId)
        {
            if (wishListId != 0)
            {
                //Select LobbyID where WishLsits Id is = wishlist id
                ChatLobby lobby = await dbLayer.WishLists.Select(X => X.Chat).Include(x => x.Messages).FirstOrDefaultAsync(X => X.Id == wishListId);
                if (lobby != null)
                {
                    return lobby;
                }
            }
            return null;
        }
    }
}
