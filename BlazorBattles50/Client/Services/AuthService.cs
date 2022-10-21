using BlazorBattles50.Shared;
using Blazored.LocalStorage;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazorBattles50.Client.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _http;
        private readonly ILocalStorageService _localStorageService;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly IToastService _toastService;
        private readonly NavigationManager _navigationManager;

        public AuthService(HttpClient http, ILocalStorageService localStorageService
            , AuthenticationStateProvider AuthStateProvider
            , IToastService toastService
            , NavigationManager navigationManager)
        {
            _http = http;
            _localStorageService = localStorageService;
            _authStateProvider = AuthStateProvider;
            _toastService = toastService;
            this._navigationManager = navigationManager;
        }

        public async Task Login(UserLogin request)
        {
            var result = await _http.PostAsJsonAsync("api/auth/login", request);
            var response = await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
            if (response.Success)
            {
                await _localStorageService.SetItemAsync<string>("authToken", response.Data);
                await _authStateProvider.GetAuthenticationStateAsync();
            }
            else
            {
                _toastService.ShowError(response.Message);
            }
        }

        public async Task Logout()
        {
            await _localStorageService.RemoveItemAsync("authToken");
            await _authStateProvider.GetAuthenticationStateAsync();
            _navigationManager.NavigateTo("/");
        }

        public async Task<ServiceResponse<int>> Register(UserRegister request)
        {
            var result = await _http.PostAsJsonAsync("api/Auth/register", request);
            return await result.Content.ReadFromJsonAsync<ServiceResponse<int>>();
        }
    }
}
