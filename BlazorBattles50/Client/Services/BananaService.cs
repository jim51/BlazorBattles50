using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorBattles50.Client.Services
{
    public class BananaService : IBananaService
    {
        public int Bananas { get; set; } = 1000;

        public event Action OnChange;

        public void EatBananas(int amouont)
        {
            Bananas -= amouont;
            BananasChanged();
        }

        void BananasChanged() => OnChange.Invoke();
    }
}
