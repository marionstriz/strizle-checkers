using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DAL;
using DAL.DTO;

namespace WebApp.Pages.Players
{
    public class DeleteModel : PageModel
    {
        private readonly IPlayerDbRepository _playerRepository;

        public DeleteModel(IGameDbRepository gameDbRepository)
        {
            _playerRepository = gameDbRepository.GetPlayerRepository();
        }

        [BindProperty] 
        public Player Player { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _playerRepository.GetByIdAsync(id.Value);

            if (player == null)
            {
                return NotFound();
            }
            Player = player;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await _playerRepository.DeleteByIdAsync(id.Value);

            return RedirectToPage("./Index");
        }
    }
}
