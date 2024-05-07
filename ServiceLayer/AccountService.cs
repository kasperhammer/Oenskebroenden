using Models.DtoModels;
using Models.Forms;
using Newtonsoft.Json;
using System.Text;

namespace ServiceLayer
{
    public class AccountService
    {
        protected string URL = "https://localhost:7212/Account/";

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
    }
}
