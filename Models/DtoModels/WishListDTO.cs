using Models.EntityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DtoModels
{
    public class WishListDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Emoji { get; set; }

        public int OwnerId { get; set; }

        public List<WishDTO>? Wishes { get; set; }

        public ChatLobbyDTO? Chat { get; set; }
    }
}
