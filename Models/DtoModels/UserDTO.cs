using Models.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DtoModels
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<WishListDTO> WishLists { get; set; }
        public List<History> WishListHistory { get; set; }


        public string Token { get; set; }
        public DateTime? TokenExpires { get; set; }

        public string ConnectionId { get; set; }
  


    }
}
