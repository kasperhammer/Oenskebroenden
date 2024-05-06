using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.EntityModels
{
    public class Wish
    {
        [Key]
        public int Id { get; set; }

        public int WishListId { get; set; }

        [ForeignKey(nameof(WishListId))]
        public WishList WishList { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public string PictureURL { get; set; }
        public int? ReservedUserId { get; set; }

        [ForeignKey(nameof(ReservedUserId))]
        public User? ReservedUser { get; set; }

    }
}
