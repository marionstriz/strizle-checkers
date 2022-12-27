using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using DAL.DTO;

namespace WebApp.Pages.GameStates
{
    public class IndexModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public IndexModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        public IList<CheckersGameState> CheckersGameState { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.GameStates != null)
            {
                CheckersGameState = await _context.GameStates
                .Include(c => c.CheckersGame).ToListAsync();
            }
        }
    }
}
