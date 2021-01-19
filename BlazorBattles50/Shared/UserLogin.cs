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
        [Required(ErrorMessage ="請輸入帳號")]
        public string username { get; set; }
        [Required]
        public string password { get; set; }
    }
}
