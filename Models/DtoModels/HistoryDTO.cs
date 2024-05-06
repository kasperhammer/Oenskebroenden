using Models.EntityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DtoModels
{
    public class HistoryDTO
    {
        public int Id { get; set; }
        
        public User User { get; set; }

        public WishList WishList { get; set; }
    }
}
