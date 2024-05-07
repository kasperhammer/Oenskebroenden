using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.EntityModels
{
    public class WishList
    {
        [Key]
        public int Id { get; set; }
        public int OwnerId { get; set; }
        [ForeignKey(nameof(OwnerId))]
        public User Owner { get; set; }
        public List<Wish> Wishes { get; set; }
        public int LobbyId { get; set; }
        [ForeignKey(nameof(LobbyId))]
        public ChatLobby Chat { get; set; }

        public string Name { get; set; }
        public string Emoji { get; set; }

    }
}
