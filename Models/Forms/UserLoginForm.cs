using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Forms
{
    public class UserLoginForm
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [MinLength(5)]
        public string Password { get; set; }
    }
}
