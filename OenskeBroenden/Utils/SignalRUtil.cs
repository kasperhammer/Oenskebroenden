﻿using Microsoft.AspNetCore.SignalR.Client;
using Models.DtoModels;
using Models.Forms;
using Repository;

namespace OenskeBroenden.Utils
{
    public class SignalRUtil
    {
        public readonly ChatConnector sigConnector;

        private readonly SignalREvents events;
        public SignalRUtil(ChatConnector sigConnector,SignalREvents events )
        {
            this.sigConnector = sigConnector;
            this.events = events;
        }

        public async Task<UserDTO> ConnectAsync(UserDTO person,int lobbyId)
        {
            person.ConnectionId = sigConnector.hubConnection.ConnectionId;
            await sigConnector.hubConnection.SendAsync("JoinLobby", lobbyId.ToString());
            await NotificationListener();
            await SendMessage(new ChatMessageForm { LobbyId = lobbyId, Message = "Someone Joined the Chat", MessageTime = DateTime.Now, SenderId = person.Id,SenderName = person.Name });
            return person;
        }

        public async Task SendMessage(ChatMessageForm message)
        {
            await sigConnector.hubConnection.SendAsync("SendMessage", message);
        }



        public async Task NotificationListener()
        {
            try
            {
                sigConnector.hubConnection.On<ChatMessageForm>("ReciveMessage", (request) =>
                {
                    //Invoke Event
                    events.RaiseEvent(request);

                });
            }
            catch { }
        }
    }
}
