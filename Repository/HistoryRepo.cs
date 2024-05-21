using Models.DtoModels;
using Models.EntityModels;
using ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class HistoryRepo : IHistoryRepo
    {

        HistoryService service;
        public ITokenRepo tokenRepo;

        public HistoryRepo(ITokenRepo tokenRepo)
        {
            this.tokenRepo = tokenRepo;
            service = new();

        }

        //Denne metode bliver brugt til at tilføje en ønskeliste til en brugers historik.
        public async Task<bool> AddHistoryAsync(UserDTO cookie, int wishListId)
        {
            cookie = await tokenRepo.TokenValidationPackageAsync(cookie);

            // Tjekker om token valideringen var succesfuld.
            if (cookie != null && wishListId != 0)
            {
               
                return await service.AddHistoryAsync(wishListId, cookie.Token);
            }

            return false;
        }

        // Denne metode bliver brugt til at hente en given brugers tidligere ønskelister han har besøgt
        public async Task<List<HistoryDTO>> GetHistoryDTOsAsync(UserDTO cookie)
        {
            cookie = await tokenRepo.TokenValidationPackageAsync(cookie);

            // Tjekker om token valideringen var succesfuld.
            if (cookie != null)
            {
                return await service.GetHistoryAsync(cookie.Token);
            }

            return null;
        }
    }
}
