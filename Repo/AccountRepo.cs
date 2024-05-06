using DbAccess;
using Microsoft.EntityFrameworkCore;
using Models.DtoModels;
using Models.EntityModels;
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

        public async Task<bool> CreateAccountAsync(UserDTO userDto)
        {
            if (userDto != null)
            {
                User user = autoMapper.mapper.Map<User>(userDto);
                if (user != null)
                {
                    //check if user already exist
                    if (await DoesUserExistAsync(user) == false)
                    {
                        try
                        {
                            user.Password = PasswordHash.HashPassword(user.Password);
                            await dBLayer.Users.AddAsync(user);
                            return await dBLayer.SaveChangesAsync() > 0;
                        }
                        catch
                        { } 
                    }
                }
            }
            return false;
        }

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
