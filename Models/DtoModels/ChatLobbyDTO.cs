using Models.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DtoModels
{
    public class ChatLobbyDTO
    {
        public int Id { get; set; }
        public List<ChatMessageDTO> Messages { get; set; }
    }
}
