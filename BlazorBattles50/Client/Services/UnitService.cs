using BlazorBattles50.Shared;
using Blazored.Toast.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorBattles50.Client.Services
{
    public class UnitService : IUnitService
    {

        public UnitService(IToastService toastService)
        {
            _toastService = toastService;
        }
        public IList<Unit> Units { get; } = new List<Unit>
        {
            new Unit(){Id=1,Title="騎士", Attack=10,Defense=10,BananaCost=100},
            new Unit(){Id=2,Title="弓手", Attack=15,Defense=5,BananaCost=50},
            new Unit(){Id=3,Title="魔法師", Attack=20,Defense=1,BananaCost=200},
        };

        public IList<UserUnit> MyUnits { get; set; } = new List<UserUnit>();
        public IToastService _toastService { get; }

        public void AddUnit(int unitId)
        {
            Unit unit = Units.First(unit => unit.Id == unitId);
            MyUnits.Add(new UserUnit() { UnitId = unit.Id, HitPoints = unit.HitPoints });
            _toastService.ShowSuccess($"Your  {unit.Title} has been built!", "Unit built!");
            //Console.WriteLine($"{unit.Title} was built!");
            //Console.WriteLine($"Your army size: {MyUnits.Count}");
        }
    }
}
