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

namespace WebApp.Pages.GameOptions
{
    public class EditModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public EditModel(DAL.AppDbContext context)
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

            var checkersgameoptions =  await _context.GameOptions.FirstOrDefaultAsync(m => m.Id == id);
            if (checkersgameoptions == null)
            {
                return NotFound();
            }
            CheckersGameOptions = checkersgameoptions;
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

            _context.Attach(CheckersGameOptions).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CheckersGameOptionsExists(CheckersGameOptions.Id))
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

        private bool CheckersGameOptionsExists(int id)
        {
          return (_context.GameOptions?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
