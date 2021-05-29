using BlazorBattles50.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorBattles50.Server.Services
{
    public interface IUtilityService
    {
        Task<User> GetUser();
    }
}
