using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Forms
{
    public class HistoryForm
    {
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int WishListId { get; set; }
    }
}
