using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using DAL.DTO;

namespace WebApp.Pages.GameOptions
{
    public class DeleteModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public DeleteModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
      public CheckersGameOptions CheckersGameOptions { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.GameOptions == null)
            {
                return NotFound();
            }

            var checkersgameoptions = await _context.GameOptions.FirstOrDefaultAsync(m => m.Id == id);

            if (checkersgameoptions == null)
            {
                return NotFound();
            }
            else 
            {
                CheckersGameOptions = checkersgameoptions;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.GameOptions == null)
            {
                return NotFound();
            }
            var checkersgameoptions = await _context.GameOptions.FindAsync(id);

            if (checkersgameoptions != null)
            {
                CheckersGameOptions = checkersgameoptions;
                _context.GameOptions.Remove(CheckersGameOptions);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
