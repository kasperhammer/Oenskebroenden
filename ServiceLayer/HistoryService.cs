using Models.DtoModels;
using Models.Forms;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class HistoryService
    {
        protected string URL = "https://localhost:7212/api/History/";

        // Metode til at oprette en bruger ved at sende en POST-anmodning med brugeroplysninger til en given URL.
        public async Task<bool> AddHistory(int wishListId,string token)
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
                    HttpResponseMessage response = await client.PostAsync(URL + "AddHistory?wishListId="+wishListId,null);

                    // Kontrollerer om anmodningen blev udført succesfuldt.
                    if (response.IsSuccessStatusCode)
                    {
                        
                            return true; // Returnerer det oprettede brugerobjekt.
                        
                    }
                }
                catch
                {
                    // Håndterer fejl, hvis der opstår en under oprettelsen af brugeren.
                }
            }
            return false; // Returnerer null, hvis oprettelsen fejler eller person-objektet er null.
        }
    }
}
