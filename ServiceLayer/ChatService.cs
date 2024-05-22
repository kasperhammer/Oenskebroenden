using Models.DtoModels;
using Models.Forms;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class ChatService
    {
        protected string URL = "https://localhost:7212/api/Chat/";

        // Metode til at oprette en bruger ved at sende en POST-anmodning med brugeroplysninger til en given URL.
        public async Task<bool> AddMessageAsync(ChatMessageForm message,string token)
        {
            // Kontrollerer om person-objektet er gyldigt og ikke er null.
            if (message != null)
            {
                try
                {
                    // Konverterer person-objektet til JSON-format.
                    string jsonData = JsonConvert.SerializeObject(message);
                    // Opretter en HTTP-anmodningsindhold med JSON-data.
                    StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    // Opretter en ny HTTP-klient.
                    HttpClient client = new();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    // Sender en POST-anmodning med JSON-data til URL'en med endepunktet "CreateAccount".
                    HttpResponseMessage response = await client.PostAsync(URL + "AddMessage", content);

                    // Kontrollerer om anmodningen blev udført succesfuldt.
                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                }
                catch
                {
                    // Håndterer fejl, hvis der opstår en under oprettelsen af brugeren.
                }
            }
            return false; // Returnerer null, hvis oprettelsen fejler eller person-objektet er null.
        }

        public async Task<ChatLobbyDTO> GetChatAsync(int wishListId,string token)
        {
            // Kontrollerer om person-objektet er gyldigt og ikke er null.
            if (wishListId != 0)
            {
                try
                {
                 
                    // Opretter en ny HTTP-klient.
                    HttpClient client = new();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    // Sender en POST-anmodning med JSON-data til URL'en med endepunktet "CreateAccount".
                    HttpResponseMessage response = await client.GetAsync(URL + "GetChat?wishListId="+wishListId);

                    // Kontrollerer om anmodningen blev udført succesfuldt.
                    if (response.IsSuccessStatusCode)
                    {
                        ChatLobbyDTO lobby = JsonConvert.DeserializeObject<ChatLobbyDTO>(await response.Content.ReadAsStringAsync());
                        if (lobby != null)
                        {
                            return lobby;
                        }
                    }
                }
                catch
                {
                    // Håndterer fejl, hvis der opstår en under oprettelsen af brugeren.
                }
            }
            return null; // Returnerer null, hvis oprettelsen fejler eller person-objektet er null.
        }

    }
}
