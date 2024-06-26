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
       

        public async Task<bool> CreateWishListAsync(WishlistCreateForm wishList, UserDTO user)
        {
            apiEndpoint = "CreateWishList";
            using (HttpClient client = new HttpClient())
            {
                // Set JWT token in the Authorization header
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);
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
                catch{}
          

               
            }
            return false;
        }


        public async Task<List<WishListDTO>> GetWishlistsFromUserAsync(UserDTO person)
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
                    HttpResponseMessage response = await client.GetAsync(apiUrl + "GetWishlists");

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


        public async Task<WishListDTO> GetOneWishListASync(string token, int wishListId)
        {
            if (!string.IsNullOrEmpty(token) && wishListId != 0)
            {
                try
                {
                    // Opretter en ny HTTP-klient.
                    HttpClient client = new();
                    // Tilføjer autorisationsheaderen til anmodningen med det givne token.
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    // Sender en POST-anmodning med brugeroplysninger og det nuværende token til URL'en med endepunktet "RefreshToken".
                    HttpResponseMessage response = await client.GetAsync(apiUrl + "GetOneWishList?wishListId=" + wishListId);

                    // Kontrollerer om anmodningen blev udført succesfuldt.
                    if (response.IsSuccessStatusCode)
                    {
                        // Opretter et nyt UserDTO-objekt og udfylder det med svaret fra serveren.
                        WishListDTO wishlist = JsonConvert.DeserializeObject<WishListDTO>(await response.Content.ReadAsStringAsync());
                        // Kontrollerer om det nye brugerobjekt blev opdateret korrekt.
                        if (wishlist != null)
                        {
                            return wishlist; // Returnerer det opdaterede brugerobjekt.
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


        public async Task<bool> CreateWishAsync(WishCreateForm wish, UserDTO user)
        {
            apiEndpoint = "CreateWish";
            using (HttpClient client = new HttpClient())
            {
                if(wish.Link == null)
                {
                    wish.Link = "";
                }
                // Set JWT token in the Authorization header
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);
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
        public async Task<bool> ReserveWishAsync(string token, int wishId)
        {
            if (!string.IsNullOrEmpty(token) && wishId != 0)
            {
                try
                {
                    // Opretter en ny HTTP-klient.
                    HttpClient client = new();
                    // Tilføjer autorisationsheaderen til anmodningen med det givne token.
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    // Sender en POST-anmodning med brugeroplysninger og det nuværende token til URL'en med endepunktet "RefreshToken".
                    HttpResponseMessage response = await client.PutAsync(apiUrl + "ReserveWish?wishId="+wishId,null);

                    // Kontrollerer om anmodningen blev udført succesfuldt.
                    return response.IsSuccessStatusCode;
                    
                        
                    
                }
                catch
                {
                    // Håndterer fejl, hvis der opstår en under opdateringen af tokenet.
                }
            }

            return false; // Returnerer null, hvis opdateringen fejler eller person-objektet er null.

        }


        public async Task<bool> UpdateWishASync(WishCreateForm wish, string token)
        {
            // Kontrollerer om person-objektet er gyldigt og ikke er null.
            if (wish != null)
            {
                try
                {

                    // Opretter en ny HTTP-klient.
                    HttpClient client = new();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    var jsonContent = JsonConvert.SerializeObject(wish);
                    var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                    // Sender en POST-anmodning med JSON-data til URL'en med endepunktet "CreateAccount".
                    HttpResponseMessage response = await client.PutAsync(apiUrl + "UpdateWish", httpContent);

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

        public async Task<bool> DeleteWishAsync(string token,int wishId)
        {
            // Kontrollerer om person-objektet er gyldigt og ikke er null.
            if (wishId != 0)
            {
                try
                {

                    // Opretter en ny HTTP-klient.
                    HttpClient client = new();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                  
                    // Sender en POST-anmodning med JSON-data til URL'en med endepunktet "CreateAccount".
                    HttpResponseMessage response = await client.DeleteAsync(apiUrl + "DeleteWish?wishId="+wishId);

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

        public async Task <WishCreateForm> GetWishFromUrl(string token, string url)
        {
            try
            {
                // Opretter en ny HTTP-klient.
                HttpClient client = new();
                // Tilføjer autorisationsheaderen til anmodningen med det givne token.
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                // Sender en POST-anmodning med brugeroplysninger og det nuværende token til URL'en med endepunktet "RefreshToken".
                HttpResponseMessage response = await client.GetAsync(apiUrl + "GetWishFromWeb?url=" + url);

                // Kontrollerer om anmodningen blev udført succesfuldt.
                if (response.IsSuccessStatusCode)
                {
                    WishCreateForm wish = JsonConvert.DeserializeObject<WishCreateForm>(await response.Content.ReadAsStringAsync());
                    return wish;
                }
            }
            catch(Exception ex)
            {
                // Håndterer fejl, hvis der opstår en under opdateringen af tokenet.
            }
            return null;
        }

    }
}
