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
using System.Threading.Tasks;

namespace BlazorBattles50.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BattleController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IUtilityService _utilityService;

        public BattleController(DataContext context, IUtilityService utilityService)
        {
            this._context = context;
            this._utilityService = utilityService;
        }

        [HttpPost]
        public async Task<IActionResult> StartBattle([FromBody] int opponentId)
        {
            var attacker = await _utilityService.GetUser();
            var opponent = await _context.Users.FindAsync(opponentId);
            if(opponent==null || opponent.IsDeleted)
            {
                return NotFound("使手者不存在!");
            }

            var result = new BattleResult();
            result.Log = new List<string>();
            await Fight(attacker, opponent, result);
            return Ok(result);
        }

        private async Task Fight(User attacker, User opponent, BattleResult result)
        {
            var attackerArmy = await _context.UserUnits
                .Where(x => x.UserId == attacker.Id && x.HitPoints > 0)
                .Include(x=>x.Unit)
                .ToListAsync();
            var opponentArmy= await _context.UserUnits
                .Where(x => x.UserId == opponent.Id && x.HitPoints > 0)
                .Include(x => x.Unit)
                .ToListAsync();
            var attackerDamageSum = 0;
            var opponentDamageSum = 0;
            int currentRound = 0;
            while (attackerArmy.Count>0 && opponentArmy.Count > 0)
            {
                currentRound++;
                if (currentRound % 2 != 0)
                    attackerDamageSum += FightRound(attacker, opponent, attackerArmy, opponentArmy, result);
                else
                    opponentDamageSum += FightRound(opponent,attacker , opponentArmy, attackerArmy,  result); 
            }
            result.IsVictory = opponentArmy.Count() == 0;
            result.RoundsFought = currentRound;
            if (result.RoundsFought > 0)
                await FinishFight(attacker, opponent, result, attackerDamageSum, opponentDamageSum);
        }

        private  async Task FinishFight(User attacker, User opponent, BattleResult result, int attackerDamageSum, int opponentDamageSum)
        {
            result.AttackerDamageSum = attackerDamageSum;
            result.OpponentDamageSum = opponentDamageSum;
            attacker.Battles++;
            opponent.Battles++;
            if (result.IsVictory)
            {
                attacker.Victories++;
                opponent.Defeats++;
                attacker.Bananas += opponentDamageSum;
                opponent.Bananas += attackerDamageSum * 10;
            }
            else
            {
                opponent.Victories++;
                attacker.Defeats++;
                opponent.Bananas += opponentDamageSum;
                attacker.Bananas += attackerDamageSum * 10;
            }
            await StoreBattleHistory(attacker, opponent, result);
            await _context.SaveChangesAsync();
        }

        private int FightRound(User attacker, User opponent, List<UserUnit> attackerArmy, List<UserUnit> opponentArmy, BattleResult result)
        {
            int randomAttackerIndex = new Random().Next(attackerArmy.Count());
            int randomOpponentIndex = new Random().Next(opponentArmy.Count());
            var randomAttacker = attackerArmy[randomAttackerIndex];
            var randomOpponent = opponentArmy[randomOpponentIndex];
            var damage = new Random().Next(randomAttacker.Unit.Attack) - new Random().Next(randomOpponent.Unit.Defense);
            if (damage < 0) damage = 0;
            if (damage <= randomOpponent.HitPoints)
            {
                randomOpponent.HitPoints -= damage;
                result.Log.Add(
                    $"{attacker.Username}'s {randomAttacker.Unit.Title} attacks " +
                    $"{opponent.Username}'s {randomOpponent.Unit.Title} with {damage} damage.");
                return damage;
            }
            else
            {
                damage = randomOpponent.HitPoints;
                randomOpponent.HitPoints = 0;
                opponentArmy.Remove(randomOpponent);
                result.Log.Add(
                    $"{attacker.Username}'s {randomAttacker.Unit.Title} kills " +
                    $"{opponent.Username}'s {randomOpponent.Unit.Title}!");
                return damage;
            }
        }

        private async Task StoreBattleHistory(User attacker, User opponent, BattleResult result)
        {
            var battle = new Battle();
            battle.Attacker = attacker;
            battle.Opponent = opponent;
            battle.RoundsFought = result.RoundsFought;
            battle.WinnerDamage = result.IsVictory ? result.AttackerDamageSum : result.OpponentDamageSum;
            battle.Winner = result.IsVictory ? attacker : opponent;

            await _context.Battles.AddAsync(battle);
        }

        [HttpGet("History")]
        public async Task<IActionResult> GetHistory()
        {
            var user = await _utilityService.GetUser();
            var battles = await _context.Battles
                .Where(x => x.AttackerId == user.Id || x.OpponentId == user.Id)
                .Include(x => x.Attacker)
                .Include(x => x.Opponent)
                .Include(x => x.Winner)
                .ToListAsync();
            var history = battles.Select(x => new BattleHistoryEntry()
            {
                BattleDate=x.BattleDate,
                AttackerId=x.AttackerId,
                AttackerName=x.Attacker.Username,
                BattleId=x.Id,
                OpponentId=x.OpponentId,
                OpponentName=x.Opponent.Username,
                RoundsFought=x.RoundsFought,
                YouWon=x.WinnerId==user.Id,
                WinnerDamageDealt=x.WinnerDamage
            });
            return Ok(history.OrderByDescending(h=>h.BattleDate).ToList());
        }
    }
}
