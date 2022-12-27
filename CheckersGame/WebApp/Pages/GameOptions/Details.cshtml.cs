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
    public class DetailsModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public DetailsModel(DAL.AppDbContext context)
        {
            _context = context;
        }

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
    }
}
