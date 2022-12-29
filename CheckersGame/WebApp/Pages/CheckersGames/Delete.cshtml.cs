using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using DAL.DTO;

namespace WebApp.Pages.CheckersGames
{
    public class DeleteModel : PageModel
    {
        private readonly IGameDbRepository _repository;

        public DeleteModel(IGameDbRepository repository)
        {
            _repository = repository;
        }

        [BindProperty]
      public CheckersGame CheckersGame { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var checkersGame = await _repository.GetByIdAsync(id.Value);

            if (checkersGame == null)
            {
                return NotFound();
            }
            CheckersGame = checkersGame;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            await _repository.DeleteByIdAsync(id.Value);

            return RedirectToPage("./Index");
        }
    }
}
