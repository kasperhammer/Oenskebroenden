using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Forms
{
    public  class WishlistCreateForm
    {
        [Required]
        [MinLength(1)]
        public string Name { get; set; }
        public int OwnerId { get; set; }
        public string Emoji { get; set; }




    }
}
