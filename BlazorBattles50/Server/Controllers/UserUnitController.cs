using BlazorBattles50.Server.Data;
using BlazorBattles50.Server.Services;
using BlazorBattles50.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorBattles50.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserUnitController : ControllerBase
    {
        private readonly IUtilityService _utilityService;
        private readonly DataContext _context;

        public UserUnitController(IUtilityService utilityService,DataContext dataContext)
        {
            _utilityService = utilityService;
            _context = dataContext;
        }

        [HttpPost]
        public async Task<IActionResult> BildUserUnit([FromBody] int unitId)
        {
            var unit = await _context.Units.FirstOrDefaultAsync(u => u.Id == unitId);
            var user = await _utilityService.GetUser();
            if (user.Bananas < unit.BananaCost)
            {
                return BadRequest("香蕉不足!");
            }

            user.Bananas -= unit.BananaCost;
            UserUnit userUnit = new UserUnit()
            {
                UnitId = unit.Id,
                UserId = user.Id,
                HitPoints = unit.HitPoints
            };

            await _context.UserUnits.AddAsync(userUnit);
            await _context.SaveChangesAsync();
            return Ok(userUnit);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserUnits()
        {
            var user = await _utilityService.GetUser();
            var userUnits = await _context.UserUnits.Where(u => u.UserId == user.Id).ToListAsync();
            var response = userUnits.Select(x => new UserUnitResponse()
            {
                HitPoints = x.HitPoints,
                UnitId = x.UnitId
            }).ToList();

            return Ok(response);
        }


        [HttpPost("revive")]
        public async Task<IActionResult> ReviveArmy()
        {
            var user = await _utilityService.GetUser();
            var userUnits = await _context.UserUnits
                .Where(u => u.UserId == user.Id)
                .Include(u => u.Unit)
                .ToListAsync();
            int bananaCost = 1000;
            if (user.Bananas < bananaCost)
            {
                return BadRequest($"香蕉不足!你需要{bananaCost}香蕉.");
            }
            bool armyAlereadAlive = true;
            foreach (var userUnit in userUnits)
            {
                if (userUnit.HitPoints <= 0)
                {
                    armyAlereadAlive = false;
                    userUnit.HitPoints = new Random().Next(1, userUnit.Unit.HitPoints);
                }
            }
            if (armyAlereadAlive)
                return Ok("你的軍隊全部存活!");
            user.Bananas -= bananaCost;
            await _context.SaveChangesAsync();

            return Ok("軍隊復活!");
        }
    }
}
