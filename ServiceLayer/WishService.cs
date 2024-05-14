﻿using Models.DtoModels;
using Models.EntityModels;
using Models.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;


namespace ServiceLayer
{
    public class WishService
    {
        string apiUrl = "https://localhost:7212/api/wish/";
        string apiEndpoint = "";
       

        public async Task<bool> CreateWishListAsync(WishlistCreateForm wishList, UserDTO cookie)
        {
            apiEndpoint = "CreateWishList";
            using (HttpClient client = new HttpClient())
            {
                // Set JWT token in the Authorization header
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", cookie.Token);
                // Serialize the object to JSON
                var jsonContent = JsonConvert.SerializeObject(wishList);
                var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
               

                try
                {
                    // Call the API
                    var response = await client.PostAsync(apiUrl + apiEndpoint, httpContent);
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Wish list created successfully!");
                    }
                    else
                    {
                        Console.WriteLine("Failed to create wish list. Status code: " + response.StatusCode);
                    }
                    return response.IsSuccessStatusCode;
                }
                catch (Exception e)
                {

                    throw;
                }
          

               
            }
        }


        public async Task<List<WishListDTO>> GetWishlistsFromUser(UserDTO person)
        {
            if (person != null)
            {
                try
                {
                    // Opretter en ny HTTP-klient.
                    HttpClient client = new();
                    // Tilføjer autorisationsheaderen til anmodningen med det givne token.
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", person.Token);
                    // Sender en POST-anmodning med brugeroplysninger og det nuværende token til URL'en med endepunktet "RefreshToken".
                    HttpResponseMessage response = await client.GetAsync(apiUrl + "GetWishlists?userId="+person.Id);

                    // Kontrollerer om anmodningen blev udført succesfuldt.
                    if (response.IsSuccessStatusCode)
                    {
                        // Opretter et nyt UserDTO-objekt og udfylder det med svaret fra serveren.
                        List<WishListDTO> wishlists = JsonConvert.DeserializeObject<List<WishListDTO>>(await response.Content.ReadAsStringAsync());
                        // Kontrollerer om det nye brugerobjekt blev opdateret korrekt.
                        if (wishlists != null)
                        {
                            return wishlists; // Returnerer det opdaterede brugerobjekt.
                        }
                    }
                }
                catch
                {
                    // Håndterer fejl, hvis der opstår en under opdateringen af tokenet.
                }
            }

            return null; // Returnerer null, hvis opdateringen fejler eller person-objektet er null.

        }



        public async Task<bool> CreateWishAsync(WishCreateForm wish, UserDTO cookie)
        {
            apiEndpoint = "CreateWish";
            using (HttpClient client = new HttpClient())
            {
                wish.ReservedUserId = 0;
                if(wish.Link == null)
                {
                    wish.Link = "";
                }
                // Set JWT token in the Authorization header
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", cookie.Token);
                // Serialize the object to JSON
                // Serialize the object to JSON
                var jsonContent = JsonConvert.SerializeObject(wish);
                var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                // Call the API
                var response = await client.PostAsync(apiUrl + apiEndpoint, httpContent);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Wish created successfully!");
                }
                else
                {
                    Console.WriteLine("Failed to create wish. Status code: " + response.StatusCode);
                }
                return response.IsSuccessStatusCode;
            }
        }
    }
}
