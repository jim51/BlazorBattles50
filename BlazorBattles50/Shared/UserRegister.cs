using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorBattles50.Shared
{
    public class UserRegister
    {
        [Required, EmailAddress]
        public string Email { get; set; }
        [StringLength(16, ErrorMessage ="最多16個字元")]
        public string Username { get; set; }
        public string Bio { get; set; }
        [Required, StringLength(100, MinimumLength =6)]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage ="The password do not match.")]
        public string ConfirmPassword { get; set; }
        [Range(0, 100, ErrorMessage = "Please choose a number between 0 and 1000.")]
        public int Bananas { get; set; } = 100;
        public string StartUnitId { get; set; } = "1";
        public DateTime DateOfBirth { get; set; } = DateTime.Now;
        [Range(typeof(bool), "true", "true", ErrorMessage = "Only confirmed user can play!")]
        public bool IsConfirmed { get; set; } = true;
    }
}
