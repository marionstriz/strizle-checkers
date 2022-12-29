using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using DAL.DTO;

namespace WebApp.Pages.Players
{
    public class IndexModel : PageModel
    {
        private readonly IPlayerDbRepository _playerRepository;
        
        public IndexModel(IGameDbRepository gameDbRepository)
        {
            _playerRepository = gameDbRepository.GetPlayerRepository();
        }
        public IList<Player> Player { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Player = await _playerRepository.GetAllAsync();
        }
    }
}
