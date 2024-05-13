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

        // Metode til at oprette en bruger ved at sende en POST-anmodning med brugeroplysninger til en given URL.
        public async Task<UserDTO> CreateUser(UserCreateForm person)
        {
            // Kontrollerer om person-objektet er gyldigt og ikke er null.
            if (person != null)
            {
                try
                {
                    // Konverterer person-objektet til JSON-format.
                    string jsonData = JsonConvert.SerializeObject(person);
                    // Opretter en HTTP-anmodningsindhold med JSON-data.
                    StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    // Opretter en ny HTTP-klient.
                    HttpClient client = new();
                    // Sender en POST-anmodning med JSON-data til URL'en med endepunktet "CreateAccount".
                    HttpResponseMessage response = await client.PostAsync(URL + "CreateAccount", content);

                    // Kontrollerer om anmodningen blev udført succesfuldt.
                    if (response.IsSuccessStatusCode)
                    {
                        // Konverterer svaret fra JSON-format til en UserDTO-objekt.
                        UserDTO user = JsonConvert.DeserializeObject<UserDTO>(await response.Content.ReadAsStringAsync());
                        // Kontrollerer om brugeren blev oprettet korrekt.
                        if (user != null)
                        {
                            return user; // Returnerer det oprettede brugerobjekt.
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

        // Metode til at logge en bruger ind ved at sende en POST-anmodning med brugernavn og adgangskode til en given URL.
        public async Task<UserDTO> LoginAsync(string userName, string passWord)
        {
            // Kontrollerer om brugernavn og adgangskode er gyldige og ikke er tomme.
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(passWord))
            {
                try
                {
                    // Opretter en ny HTTP-klient.
                    HttpClient client = new();
                    // Sender en POST-anmodning med brugernavn og adgangskode til URL'en med endepunktet "Login".
                    HttpResponseMessage response = await client.PostAsync(URL + $"Login?userName={userName}&password={passWord}", null);
                    // Læser svaret som en streng.
                    string responseContent = await response.Content.ReadAsStringAsync();

                    // Konverterer svaret fra JSON-format til en UserDTO-objekt og returnerer det.
                    return JsonConvert.DeserializeObject<UserDTO>(responseContent);
                }
                catch
                {
                    // Håndterer fejl, hvis der opstår en under login-processen.
                }
            }
            return null; // Returnerer null, hvis login-processen fejler eller brugernavn/adgangskode er tomme.
        }

        // Metode til at validere gyldigheden af et token ved at sende en GET-anmodning med token til en given URL.
        public async Task<bool> ValidateToken(string token)
        {
            // Kontrollerer om token er gyldigt og ikke er tom.
            if (!string.IsNullOrEmpty(token))
            {
                try
                {
                    // Opretter en ny HTTP-klient.
                    HttpClient client = new();
                    // Tilføjer autorisationsheaderen til anmodningen med det givne token.
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    // Sender en GET-anmodning til URL'en med endepunktet "Validate".
                    HttpResponseMessage response = await client.GetAsync(URL + "Validate");

                    // Kontrollerer om anmodningen blev udført succesfuldt.
                    if (response.IsSuccessStatusCode)
                    {
                        return true; // Returnerer sandt hvis token er gyldigt.
                    }
                }
                catch
                {
                    // Håndterer fejl, hvis der opstår en under valideringen af tokenet.
                }
            }
            return false; // Returnerer falsk hvis valideringen fejler eller token er tomt.
        }

        // Metode til at opdatere et token ved at sende en POST-anmodning med brugeroplysninger og det nuværende token til en given URL.
        public async Task<UserDTO> RefreshTokenAsync(UserDTO person)
        {
            // Kontrollerer om person-objektet er gyldigt og ikke er null.
            if (person != null)
            {
                try
                {
                    person.ConnectionId = "";
                    // Konverterer person-objektet til JSON-format.
                    string jsonData = JsonConvert.SerializeObject(person);
                    // Opretter en HTTP-anmodningsindhold med JSON-data.
                    StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    // Opretter en ny HTTP-klient.
                    HttpClient client = new();
                    // Tilføjer autorisationsheaderen til anmodningen med det givne token.
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", person.Token);
                    // Sender en POST-anmodning med brugeroplysninger og det nuværende token til URL'en med endepunktet "RefreshToken".
                    HttpResponseMessage response = await client.PostAsync(URL + "RefreshToken", content);

                    // Kontrollerer om anmodningen blev udført succesfuldt.
                    if (response.IsSuccessStatusCode)
                    {
                        // Opretter et nyt UserDTO-objekt og udfylder det med svaret fra serveren.
                        UserDTO newPerson = JsonConvert.DeserializeObject<UserDTO>(await response.Content.ReadAsStringAsync());
                        // Kontrollerer om det nye brugerobjekt blev opdateret korrekt.
                        if (newPerson != null)
                        {
                            return newPerson; // Returnerer det opdaterede brugerobjekt.
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
