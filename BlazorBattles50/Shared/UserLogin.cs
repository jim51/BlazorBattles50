using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorBattles50.Shared
{
    public class UserLogin
    {
        [Required(ErrorMessage ="請輸入Email")]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
