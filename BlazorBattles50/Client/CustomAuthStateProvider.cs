using Blazored.LocalStorage;
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
        private readonly ILocalStorageService _localStorageService;

        public CustomAuthStateProvider(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var state = new AuthenticationState(new System.Security.Claims.ClaimsPrincipal());
            if (await _localStorageService.GetItemAsync<bool>("IsAuthenticated"))
            {
                var identity = new ClaimsIdentity(
                new[]
                {
                        new Claim(ClaimTypes.Name,"Jim")
                }, "test authentication type");
                var user = new ClaimsPrincipal(identity);
                state = new AuthenticationState(user);
             
                //return Task.FromResult(new AuthenticationState(user));
            }
            NotifyAuthenticationStateChanged(Task.FromResult(state));
            return state;
        }
    }
}
