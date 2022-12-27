using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL;
using DAL.DTO;

namespace WebApp.Pages.CheckersGames
{
    public class EditModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public EditModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public CheckersGame CheckersGame { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.CheckersGames == null)
            {
                return NotFound();
            }

            var checkersgame =  await _context.CheckersGames.FirstOrDefaultAsync(m => m.Id == id);
            if (checkersgame == null)
            {
                return NotFound();
            }
            CheckersGame = checkersgame;
           ViewData["GameOptionsId"] = new SelectList(_context.GameOptions, "Id", "Id");
           ViewData["PlayerOneId"] = new SelectList(_context.Players, "Id", "Name");
           ViewData["PlayerTwoId"] = new SelectList(_context.Players, "Id", "Name");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(CheckersGame).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CheckersGameExists(CheckersGame.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool CheckersGameExists(int id)
        {
          return (_context.CheckersGames?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
