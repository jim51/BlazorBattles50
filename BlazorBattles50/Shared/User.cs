using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorBattles50.Shared
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public int Bananas { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool IsComfirmed { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DateCreate { get; set; } = DateTime.Now;
        public List<UserUnit> Units { get; set; }
        public int Battles { get; set; }
        public int Victories { get; set; }
        public int Defeats { get; set; }
    }
}
