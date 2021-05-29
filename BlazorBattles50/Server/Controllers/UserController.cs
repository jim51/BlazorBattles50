using BlazorBattles50.Server.Data;
using BlazorBattles50.Server.Services;
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
        private readonly IUtilityService _utilityService;

        public UserController(DataContext context, IUtilityService utilityService)
        {
            _context = context;
            _utilityService = utilityService;
        }

        //private int GetUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        //private async Task<User> GetUser() => await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());

        [HttpGet("GetBananas")]
        public async Task<IActionResult> GetBananas()
        {
            User user = await _utilityService.GetUser();
            return Ok(user.Bananas);
        }



        [HttpPut("AddBananas")]
        public async Task<IActionResult> AddBananas([FromBody] int bananas)
        {
            var user = await _utilityService.GetUser();
            user.Bananas += bananas;
            await _context.SaveChangesAsync();
            return Ok(user.Bananas);
        }

        [HttpGet("LeaderBoard")]
        public async Task<IActionResult> GetLeaderBoard()
        {
            var users = await _context.Users.Where(x => !x.IsDeleted).ToListAsync();

            users = users.OrderBy(x => x.Victories).ThenBy(x => x.Defeats).ThenBy(x => x.DateCreate).ToList();
            int rank = 1;
            var response = users.Select(
                x => new UserStatistic()
                {
                    Defeats = x.Defeats,
                    Battles = x.Battles,
                    Rank = rank++,
                    UserId = x.Id,
                    Username = x.Username,
                    Victories = x.Victories
                }).ToList();
            return Ok(response);
        }
    }
}
