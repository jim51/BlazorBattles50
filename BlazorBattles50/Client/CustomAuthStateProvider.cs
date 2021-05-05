using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlazorBattles50.Client
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            //回傳一個空身分(導入至Login)
            //return Task.FromResult(new AuthenticationState(new System.Security.Claims.ClaimsPrincipal()));
            //建立一個新的身分
            var identity = new ClaimsIdentity(
                new[]
                {
                     new Claim(ClaimTypes.Name,"Jim")
                }, "test authentication type");
            var user = new ClaimsPrincipal(identity);
            return Task.FromResult(new AuthenticationState(user));
        }
    }
}
