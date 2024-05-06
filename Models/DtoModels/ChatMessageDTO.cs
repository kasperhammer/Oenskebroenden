using Models.EntityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DtoModels
{
    public class ChatMessageDTO
    {
        public int Id { get; set; }
        public UserDTO Sender { get; set; }
        public string Message { get; set; }
        public DateTime MessageTime { get; set; }
        public ChatLobbyDTO Lobby { get; set; }
    }
}
