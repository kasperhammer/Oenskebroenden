using Microsoft.AspNetCore.SignalR;
using Models.Forms;

namespace API.Hubs
{
    public class ChatHub : Hub
    {
        public async Task JoinLobby(string lobbyId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, lobbyId);
     
        }

        public async Task LeaveRoom(string lobbyId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, lobbyId);
        }

        public async Task SendMessage(ChatMessageForm message)
        {
            await Clients.Group(message.LobbyId.ToString()).SendAsync("ReciveMessage",message);
        }
    }
}
