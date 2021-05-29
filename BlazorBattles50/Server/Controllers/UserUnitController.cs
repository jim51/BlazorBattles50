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
        private readonly DataContext _dataContext;

        public UserUnitController(IUtilityService utilityService,DataContext dataContext)
        {
            _utilityService = utilityService;
            _dataContext = dataContext;
        }

        [HttpPost]
        public async Task<IActionResult> BildUserUnit([FromBody] int unitId)
        {
            var unit = await _dataContext.Units.FirstOrDefaultAsync(u => u.Id == unitId);
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

            await _dataContext.UserUnits.AddAsync(userUnit);
            await _dataContext.SaveChangesAsync();
            return Ok(userUnit);
        }
    }
}
