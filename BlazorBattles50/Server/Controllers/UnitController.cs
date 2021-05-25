using BlazorBattles50.Server.Data;
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
    public class UnitController : ControllerBase
    {
        private readonly DataContext _context;

        //public IList<Unit> Units { get; } = new List<Unit>
        //{
        //    new Unit(){Id=1,Title="騎士", Attack=10,Defense=10,BananaCost=100},
        //    new Unit(){Id=2,Title="弓手", Attack=15,Defense=5,BananaCost=50},
        //    new Unit(){Id=3,Title="魔法師", Attack=20,Defense=1,BananaCost=200},
        //};

        public UnitController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetUnits()
        {
            var units = await _context.Units.ToListAsync();
            return Ok(units);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUnit(int id,Unit unit)
        {
            Unit dbUnit = await _context.Units.FirstOrDefaultAsync(x => x.Id == id);
            if (dbUnit == null)
            {
                return NotFound("Unit with the given id doesn't exits.");
            }
            dbUnit.HitPoints = unit.HitPoints;
            dbUnit.Title = unit.Title;
            dbUnit.Attack = unit.Attack;
            dbUnit.BananaCost = unit.BananaCost;
            dbUnit.Defense = unit.Defense;
            await _context.SaveChangesAsync();

            return Ok(dbUnit);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUnit(int id)
        {
            Unit dbUnit = await _context.Units.FirstOrDefaultAsync(x => x.Id == id);
            if (dbUnit == null)
            {
                return NotFound("Unit with the given id doesn't exits.");
            }
            _context.Units.Remove(dbUnit);
         
            await _context.SaveChangesAsync();

            return Ok(await _context.Units.ToListAsync());
        }
    }
}
