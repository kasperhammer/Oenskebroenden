using DbAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Models.DtoModels;
using Models.EntityModels;
using Models.Forms;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Repo
{
    public class AccountRepo : IAccountRepo
    {

        EntityContext dbLayer;
        AutoMapper autoMapper;
        readonly TokenGeneration Token;
        int tokenLiveTime = 120;
        int refreshTokenliveTime = 120;
        readonly IConfiguration _configuration;
        public AccountRepo(IConfiguration configuration)
        {
            autoMapper = new();
            dbLayer = new();
            Token = new TokenGeneration();
            _configuration = configuration;
        }




        //Denne Metode retunere en Persons EntityModel ud fra deres navn.
        private async Task<User> GetPersonByNameAsync(string userName)
        {
            if (!string.IsNullOrEmpty(userName))
            {
                try
                {
                    User person = await dbLayer.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Name == userName);
                    if (person != null)
                    {
                        return person;
                    }
                }
                catch (Exception ex)
                {

                }
            }
            return null;
        }
        public async Task<UserDTO> CreateAccountAsync(UserCreateForm userDto)
        {
            if (userDto != null)
            {
                //Jeg anvender min AutoMapper til at konvertere min DTO model om til en EntityModel
                User user = autoMapper.mapper.Map<User>(userDto);
                if (user != null)
                {
                    //check if user already exist
                    if (await DoesUserExistAsync(user) == false)
                    {
                        try
                        {
                            //jeg anvender BCrypt til at Hashe brugeres password så det ikke ligger exposed i vores Db
                            user.Password = PasswordHash.HashPassword(user.Password);
                            //Derefter forsøger jeg at tilføje min bruger til min Db og retuenre så om det var en success eller ej.
                            await dbLayer.Users.AddAsync(user);

                            if (await dbLayer.SaveChangesAsync() > 0)
                            {
                                User userEntity = await dbLayer.Users.FirstOrDefaultAsync(x => x.Name == user.Name && x.Email == user.Email);
                                if (userEntity != null)
                                {
                                    UserDTO completeModel = autoMapper.mapper.Map<UserDTO>(userEntity);
                                    return completeModel;
                                }

                            }



                        }
                        catch
                        { }
                    }
                }
            }
            return null;
        }

        //mine 2 DoesUSerExist Metoder tjekker på om der allerede findes en bruger i system med entet samme navn eller mail
        public async Task<bool> DoesUserExistAsync(User user)
        {
            if (user != null)
            {
                try
                {
                    User loadedUser = await dbLayer.Users.FirstOrDefaultAsync(x => x.Name == user.Name || x.Email == user.Email);
                    if (loadedUser != null)
                    {
                        return true;
                    }
                }
                catch (Exception ex)
                {


                }
            }
            return false;
        }

        public async Task<bool> DoesUserExistAsync(UserDTO user)
        {
            if (user != null)
            {
                User loadedUser = await dbLayer.Users.FirstOrDefaultAsync(x => x.Name == user.Name || x.Email == user.Email);
                if (loadedUser != null)
                {
                    return true;
                }
            }
            return false;
        }



        #region JWTStuff
        //i denne metode prøver vi at logge en bruger ind det forgår ved at vi tager en persons UserName + Password
        //og tjekker om der findes en bruger med samme brugernavn. Derefter henter vi den bruger.
        //efter der tjekker vi så om det Password brugeren har skrevet ind matcher med det hashet password
        //som vi har hentet fra brugeren med samme navn i vores Database.
        public async Task<UserDTO> LoginAsync(UserDTO person)
        {
            if (person != null)
            {
                if (!string.IsNullOrEmpty(person.Name) && !string.IsNullOrEmpty(person.Password))
                {
                    User personDb = await GetPersonByNameAsync(person.Name);
              
                    try
                    {

                        UserDTO personDbConvert = autoMapper.mapper.Map<UserDTO>(personDb);

                        if (personDb != null)
                        {
                            //såfremt at det er samme password bliver der generet en ny JWT token som så bliver udstedt til brugeren.
                            if (PasswordHash.ValidatePassword(person.Password, personDbConvert.Password))
                            {
                                JwtSecurityToken token = await GenerateToken(personDbConvert);
                                personDbConvert.Token = new JwtSecurityTokenHandler().WriteToken(token);
                                personDbConvert.TokenExpires = token.ValidTo;
                                return personDbConvert;
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                        throw;
                    }
                }
            }
            return null;
        }

        //Refresh token fungere på samme måde som Login bortset fra at brugeren allerede er logget ind. 
        //der eneste vi tjekker per er om personen vi finder i databasen nu også er den samme person som
        //beder om at få udstet en ny token. såfremt det er et Match usteer vi en ny token
        public async Task<UserDTO> RefreshTokenAsync(UserDTO person)
        {
            if (person != null)
            {
                if (!string.IsNullOrEmpty(person.Token))
                {
                    User personDb = await GetPersonByNameAsync(person.Name);
                    UserDTO personDbConvert = autoMapper.mapper.Map<UserDTO>(personDb);
                    if (personDb != null)
                    {
                        if (personDb.Name == person.Name & person.Email == person.Email & person.Password == person.Password)
                        {
                            JwtSecurityToken token = await GenerateToken(personDbConvert);
                            personDbConvert.Token = new JwtSecurityTokenHandler().WriteToken(token);
                            personDbConvert.TokenExpires = token.ValidTo;
                            return personDbConvert;
                        }
                    }
                }
            }

            return null;
        }

        public async Task<byte[]> GetSecretKeyAsync()
        {
            //henter en genereret secret key som er vores Validator af tokens fra vores generator.
            return await Token.GetSecretKeyAsync();
        }

        //Generato tokens jeg tager imod en usermodel hvor jeg inkludere userens navn og Id som Claims, og retunere en JWT Token
        private async Task<JwtSecurityToken> GenerateToken(UserDTO userDTO)
        {
            var securityKey = new SymmetricSecurityKey(await GetSecretKeyAsync());
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userDTO.Name), // Add a claim for the username
                new Claim(ClaimTypes.NameIdentifier, userDTO.Id.ToString()) // Add a claim for the Age
            };

            // As mentioned above i am using the _configuration file to access Jwt values
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(tokenLiveTime),
                signingCredentials: credentials

            );

            return token;
            //return new JwtSecurityTokenHandler().WriteToken(token);
        }

        #endregion

    }
}
