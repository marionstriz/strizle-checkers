using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DAL;
using DAL.DTO;

namespace WebApp.Pages.GameStates
{
    public class CreateModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public CreateModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["CheckersGameId"] = new SelectList(_context.CheckersGames, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public CheckersGameState CheckersGameState { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.GameStates == null || CheckersGameState == null)
            {
                return Page();
            }

            _context.GameStates.Add(CheckersGameState);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
