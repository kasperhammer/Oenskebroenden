using DbAccess;
using Microsoft.EntityFrameworkCore;
using Models.DtoModels;
using Models.EntityModels;
using Models.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repo
{
    public class ChatRepo : IChatRepo
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

        public async Task<ChatLobbyDTO> GetChatLobbyAsync(int wishListId)
        {
            if (wishListId != 0)
            {
                //Select LobbyID where WishLsits Id is = wishlist id
                try
                {
                    ChatLobby lobby = await dbLayer.WishLists.Select(X => X.Chat).FirstOrDefaultAsync(X => X.Id == wishListId);
           
                    if (lobby != null)
                    {
                        lobby.Messages = await dbLayer.Messages.Where(x => x.LobbyId == lobby.Id).Include(x=> x.Sender).Select(x => new ChatMessage
                        {
                            Id = x.Id,
                            LobbyId = x.LobbyId,
                            Message = x.Message,
                            MessageTime = x.MessageTime,
                            SenderId = x.SenderId,
                            Sender = new User
                            {
                                Id = x.Sender.Id,
                                Name = x.Sender.Name
                            }
                        }).ToListAsync();
                        ChatLobbyDTO lobbyDTO = autoMapper.mapper.Map<ChatLobbyDTO>(lobby);
                        return lobbyDTO;
                    }
                    else
                    {
                        WishList wishList = await dbLayer.WishLists.FirstOrDefaultAsync(x => x.Id == wishListId);
                        wishList.Chat = new();
                        dbLayer.WishLists.Update(wishList);
                        await dbLayer.SaveChangesAsync();
                        lobby = await dbLayer.WishLists.Select(X => X.Chat).FirstOrDefaultAsync(X => X.Id == wishListId);
                        lobby.Messages = new();

                    }
                }
                catch (Exception ex)
                {

                    
                }
            }
            return null;
        }
    }
}
