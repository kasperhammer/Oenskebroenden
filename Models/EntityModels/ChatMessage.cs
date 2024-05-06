using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.EntityModels
{
    public class ChatMessage
    {
        [Key]
        public int Id { get; set; }
        public int SenderId { get; set; }
        [ForeignKey(nameof(SenderId))]
        public User Sender { get; set; }

        public string Message { get; set; }
        public DateTime MessageTime { get; set; } = DateTime.Now;

        public int LobbyId { get; set; }
        [ForeignKey(nameof(LobbyId))]
        public ChatLobby Lobby { get; set; }
    }
}
