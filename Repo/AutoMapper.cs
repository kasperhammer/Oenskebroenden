using AutoMapper;
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
    public class AutoMapper
    {
        public IMapper mapper;
        public AutoMapper()
        {
            MapperConfiguration conf = new MapperConfiguration(cfg => {
                cfg.CreateMap<User,UserDTO>();
                cfg.CreateMap<UserDTO, User>();
                cfg.CreateMap<UserCreateForm, UserDTO>();
                cfg.CreateMap<UserDTO, UserCreateForm>();
                cfg.CreateMap<User, UserCreateForm>();
                cfg.CreateMap<UserCreateForm, User>();

                cfg.CreateMap<Wish, WishDTO>();
                cfg.CreateMap<WishDTO, Wish>();
                cfg.CreateMap<WishList, WishListDTO>();
                cfg.CreateMap<WishListDTO, WishList>();
                cfg.CreateMap<WishCreateForm, WishDTO>();
                cfg.CreateMap<WishDTO, WishCreateForm>();
                cfg.CreateMap<Wish, WishCreateForm>();
                cfg.CreateMap<WishCreateForm, Wish>();

                cfg.CreateMap<History, HistoryDTO>();
                cfg.CreateMap<HistoryDTO, History>();
                cfg.CreateMap<HistoryForm, HistoryDTO>();
                cfg.CreateMap<HistoryDTO, HistoryForm>();
                cfg.CreateMap<HistoryForm, History>();
                cfg.CreateMap<History, HistoryForm>();


                cfg.CreateMap<ChatMessage, ChatMessageDTO>();
                cfg.CreateMap<ChatMessageDTO, ChatMessage>();
                cfg.CreateMap<ChatLobby, ChatLobbyDTO>();
                cfg.CreateMap<ChatLobbyDTO, ChatLobby>();
                cfg.CreateMap<ChatMessageForm, ChatMessage>();
                cfg.CreateMap<ChatMessage, ChatMessageForm>();
            });

            mapper = conf.CreateMapper();

        }
         
    }
}
