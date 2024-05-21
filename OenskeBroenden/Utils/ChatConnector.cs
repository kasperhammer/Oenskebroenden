using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace OenskeBroenden.Utils
{
    public class ChatConnector : ComponentBase, IAsyncDisposable
    {
        public HubConnection? hubConnection;

        public ChatConnector()
        {

            hubConnection = new HubConnectionBuilder()
                       .WithUrl("https://localhost:7212/chathub")
                       .Build();

            Connect();
        }

        public async Task Connect()
        {
            await hubConnection.StartAsync();
        }

        public bool IsConnected =>
  hubConnection?.State == HubConnectionState.Connected;



        public async ValueTask DisposeAsync()
        {
            if (hubConnection is not null)
            {
                await hubConnection.DisposeAsync();
            }
        }


    }
}
