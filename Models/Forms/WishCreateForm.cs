using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Forms
{
    public class WishCreateForm
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Link { get; set; }
        [Required]
        public double Price { get; set; }
        public string Description { get; set; }
        public string PictureURL { get; set; }
        [Required]
        public int WishListId { get; set; }
    }
}

