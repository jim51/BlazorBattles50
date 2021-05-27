using BlazorBattles50.Server.Data;
using BlazorBattles50.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlazorBattles50.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly DataContext _context;

        public UserController(DataContext context)
        {
            _context = context;
        }

        private int GetUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        private async Task<User> GetUser() => await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());

        [HttpGet("GetBananas")]
        public async Task<IActionResult> GetBananas()
        {
            User user = await GetUser();
            return Ok(user.Bananas);
        }

       

        [HttpPut("AddBananas")]
        public async Task<IActionResult> AddBananas([FromBody] int bananas)
        {
            var user = await GetUser();
            user.Bananas += bananas;
            await _context.SaveChangesAsync();
            return Ok(user.Bananas);
        }
    }
}
