using DbAccess;
using Microsoft.EntityFrameworkCore;
using Models.DtoModels;
using Models.EntityModels;
using Models.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repo
{
    public class AccountRepo : IAccountRepo
    {

        EntityContext dBLayer;
        AutoMapper autoMapper;

        public AccountRepo()
        {
            dBLayer = new();
            autoMapper = new();
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
                            await dBLayer.Users.AddAsync(user);
                         
                            if (await dBLayer.SaveChangesAsync() > 0)
                            {
                                User userEntity = await dBLayer.Users.FirstOrDefaultAsync(x => x.Name == user.Name && x.Email == user.Email);
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
                    User loadedUser = await dBLayer.Users.FirstOrDefaultAsync(x => x.Name == user.Name || x.Email == user.Email);
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
                User loadedUser = await dBLayer.Users.FirstOrDefaultAsync(x => x.Name == user.Name || x.Email == user.Email);
                if (loadedUser != null)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
