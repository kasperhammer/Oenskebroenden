using Models.EntityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DtoModels
{
    public class WishDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public string PictureURL { get; set; }
        public int WishListId { get; set; }
        public int? ReservedUserId { get; set; }
    }
}