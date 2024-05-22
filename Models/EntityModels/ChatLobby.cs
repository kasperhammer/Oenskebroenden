using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.EntityModels
{
    public class ChatLobby
    {
        [Key]
        public int Id { get; set; }
        public int WishListId { get; set; }
        [ForeignKey(nameof(WishListId))]
        public WishList WishList { get; set; }
        public List<ChatMessage> Messages { get; set; }
    }
}
