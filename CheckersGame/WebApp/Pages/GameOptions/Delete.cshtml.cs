using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DAL;
using DAL.DTO;

namespace WebApp.Pages.GameOptions
{
    public class DeleteModel : PageModel
    {
        private readonly IOptionsDbRepository _optionsRepository;

        public DeleteModel(IGameDbRepository gameRepository)
        {
            _optionsRepository = gameRepository.GetOptionsRepository();
        }

        [BindProperty]
      public CheckersGameOptions CheckersGameOptions { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var checkersGameOptions = await _optionsRepository.GetByIdAsync(id.Value);

            if (checkersGameOptions == null)
            {
                return NotFound();
            }
            CheckersGameOptions = checkersGameOptions;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await _optionsRepository.DeleteByIdAsync(id.Value);

            return RedirectToPage("./Index");
        }
    }
}
