using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Models.DtoModels;
using Models.EntityModels;
using Models.Forms;
using Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComponentLib.Components
{
    public partial class ChatComp
    {
        [Inject]
        public SignalREvents Messages { get; set; }

        [Parameter]
        public EventCallback<ChatMessageForm> Message { get; set; }

        [Parameter]
        public UserDTO Cookie { get; set; }

        [Parameter]
        public bool Owner { get; set; } = true;

        private WishListDTO _wishListDTO;

        [Parameter]
        public WishListDTO WishList
        {
            get => _wishListDTO;
            set
            {
                if (_wishListDTO != null)
                {
                    if (_wishListDTO.Id != 0)
                    {
                        ChangeList(_wishListDTO.Id, value.Id);
                    }
                }

                _wishListDTO = value;

            }
        }



        private string messageText = "";

        public bool toggle = true;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                Messages.notification += ReciveMessage;
                //Get the Existing chat of the wishlist
            }
        }

        public async void ChangeList(int oldId, int newId)
        {
            if (oldId != newId)
            {
                await Message.InvokeAsync(new ChatMessageForm { SenderId = 0, LobbyId = oldId });
            }
        }

        public async void ReciveMessage(object sender, ChatMessageForm message)
        {
            WishList.Chat.Messages.Add(
                new ChatMessageDTO
                {
                    Message = message.Message,
                    MessageTime = message.MessageTime,

                    Sender = new UserDTO
                    { Id = message.SenderId, Name = message.SenderName }

                }
            );
            this.InvokeAsync(() => this.StateHasChanged());

        }

        public async Task SendMessageAsync()
        {

            await Message.InvokeAsync(new ChatMessageForm { LobbyId = WishList.Chat.Id, Message = messageText, MessageTime = DateTime.Now, SenderId = Cookie.Id, SenderName = Cookie.Name });
            messageText = "";
         
        }

        public async Task Toggle()
        {
            toggle = !toggle;
            StateHasChanged();
        }



    }


}
