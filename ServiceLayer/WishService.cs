using Models.DtoModels;
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


        //public async Task<List<WishListDTO>> GetWishlistsFromUser(int userId)
        //{
        //    apiEndpoint = "GetWishlistsFromUser";
        //    HttpClient httpClient = new HttpClient();
        //    var response = await httpClient.GetAsync(apiUrl + apiEndpoint);

        //    if (response.IsSuccessStatusCode)
        //    {
        //        var jsonContent = await response.Content.ReadAsStringAsync();
        //        return JsonConvert.DeserializeObject<List<WishListDTO>>(jsonContent);
        //    }
        //    else
        //    {
        //        throw new Exception($"Failed to fetch wishlists for user {userId}. Status code: {response.StatusCode}");
        //    }

        //}



        //public async Task<bool> CreateWishAsync(WishDTO wish)
        //{
        //    apiEndpoint = "CreateWishAsync";
        //    using (HttpClient client = new HttpClient())
        //    {

        //        // Serialize the object to JSON
        //        var jsonContent = JsonConvert.SerializeObject(wish);
        //        var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        //        // Call the API
        //        var response = await client.PostAsync(apiUrl + apiEndpoint, httpContent);

        //        if (response.IsSuccessStatusCode)
        //        {
        //            Console.WriteLine("Wish created successfully!");
        //        }
        //        else
        //        {
        //            Console.WriteLine("Failed to create wish. Status code: " + response.StatusCode);
        //        }
        //        return response.IsSuccessStatusCode;
        //    }
        //}
    }
}
