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
    public class DeleteModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public DeleteModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
      public CheckersGameState CheckersGameState { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.GameStates == null)
            {
                return NotFound();
            }

            var checkersgamestate = await _context.GameStates.FirstOrDefaultAsync(m => m.Id == id);

            if (checkersgamestate == null)
            {
                return NotFound();
            }
            else 
            {
                CheckersGameState = checkersgamestate;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.GameStates == null)
            {
                return NotFound();
            }
            var checkersgamestate = await _context.GameStates.FindAsync(id);

            if (checkersgamestate != null)
            {
                CheckersGameState = checkersgamestate;
                _context.GameStates.Remove(CheckersGameState);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
