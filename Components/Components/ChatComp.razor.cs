using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Models.DtoModels;
using Models.EntityModels;
using Models.Forms;
using Repository;
using System;
using System.Collections.Generic;
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
        public WishListDTO WishList { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                Messages.notification += ReciveMessage;
                //Get the Existing chat of the wishlist
            }
        }

        public async void ReciveMessage(object sender, ChatMessageForm message)
        {

        }

        public async Task SendMessageAsync(ChatMessageForm message)
        {
            await Message.InvokeAsync(message);
        }

    }
}
