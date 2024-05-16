using Models.DtoModels;
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

        HistoryService Service { get; set; }
        public ITokenRepo tokenRepo;

        public HistoryRepo(ITokenRepo tokenRepo)
        {
            this.tokenRepo = tokenRepo;
            Service = new();

        }

        public async Task<bool> AddHistoryAsync(UserDTO cookie, int wishListId)
        {
            cookie = await tokenRepo.TokenValidationPackage(cookie);

            // Tjekker om token valideringen var succesfuld.
            if (cookie != null && wishListId != 0)
            {
                return await Service.AddHistory(wishListId, cookie.Token);
            }

            return false;
        }
    }
}
