using Models.DtoModels;
using Models.Forms;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace ServiceLayer
{
    public class AccountService
    {
        protected string URL = "https://localhost:7212/api/Account/";

        public async Task<UserDTO> CreateUser(UserCreateForm person)
        {
            if (person != null)
            {

                try
                {
                    string jsonData = JsonConvert.SerializeObject(person);
                    StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    HttpClient client = new();
                    HttpResponseMessage response = await client.PostAsync(URL + "CreateAccount", content);

                    if (response.IsSuccessStatusCode)
                    {
                        UserDTO user = JsonConvert.DeserializeObject<UserDTO>(await response.Content.ReadAsStringAsync());
                        if (user != null)
                        {
                            return user;
                        }
                    }
                }
                catch
                {


                }
            }
            return null;
        }

        public async Task<UserDTO> LoginAsync(string userName, string passWord)
        {
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(passWord))
            {
                try
                {
                    HttpClient client = new();
                    HttpResponseMessage response = await client.PostAsync(URL + $"Login?userName={userName}&password={passWord}", null);
                    string responseContent = await response.Content.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<UserDTO>(responseContent);
                    //JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

                    //// Read and validate the JWT token
                    //JwtSecurityToken jWTtoken = handler.ReadToken(tokenString) as JwtSecurityToken;

                    //var claim = jWTtoken.Claims.ElementAtOrDefault(1);
                    //if (claim != null)
                    //{
                    //    int userid = Convert.ToInt32(claim.Value);
                    //    UserDTO user = await GetUserAsync(userid, tokenString);
                    //    if (user != null)
                    //    {
                    //        return (user);
                    //    }
                    //}


                }
                catch { }
            }
            return null;
        }
        public async Task<bool> ValidateToken(string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                try
                {
                    HttpClient client = new();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage response = await client.GetAsync(URL + "Validate");
                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                }
                catch { }
            }
            return false;
        }

        public async Task<UserDTO> RefreshTokenAsync(UserDTO person)
        {
            if (person != null)
            {
                try
                {
                    string jsonData = JsonConvert.SerializeObject(person);
                    StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    HttpClient client = new();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", person.Token);
                    HttpResponseMessage response = await client.PostAsync(URL + "RefreshToken", content);
                    if (response.IsSuccessStatusCode)
                    {
                        UserDTO newPerson = new();
                        newPerson = JsonConvert.DeserializeObject<UserDTO>(await response.Content.ReadAsStringAsync());
                        if (newPerson != null)
                        {
                            return newPerson;
                        }
                    }

                }
                catch { }
            }

            return null;
        }

        public bool IsTokenAboutToExpire(DateTime tokenExpiration)
        {
            // Define the threshold for considering a token as "about to expire"
            TimeSpan threshold = TimeSpan.FromHours(1);

            // Calculate the difference between the current time and the token expiration time
            TimeSpan timeUntilExpiration = tokenExpiration - DateTime.UtcNow;

            // Check if the time until expiration is less than the threshold
            return timeUntilExpiration <= threshold;
        }
    }
}
