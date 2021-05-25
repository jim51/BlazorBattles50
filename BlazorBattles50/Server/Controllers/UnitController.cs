using BlazorBattles50.Server.Pages;
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
    }
}
