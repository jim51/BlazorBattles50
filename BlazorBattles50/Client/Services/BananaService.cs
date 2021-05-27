﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazorBattles50.Client.Services
{
    public class BananaService : IBananaService
    {
        private readonly HttpClient _http;

        public BananaService(HttpClient http)
        {
            _http = http;
        }
        public int Bananas { get; set; } = 0;

        public event Action OnChange;

        public async Task AddBananas(int amount)
        {
            var result = await _http.PutAsJsonAsync<int>("/api/user/AddBananas", amount);
            Bananas = await result.Content.ReadFromJsonAsync<int>();
            BananasChanged();
        }

        public void EatBananas(int amount)
        {
            Bananas -= amount;
            BananasChanged();
        }

        public async Task GetBananas()
        {
            Bananas = await _http.GetFromJsonAsync<int>("/api/user/getBananas");
            BananasChanged();
        }

        void BananasChanged() => OnChange.Invoke();
    }
}
