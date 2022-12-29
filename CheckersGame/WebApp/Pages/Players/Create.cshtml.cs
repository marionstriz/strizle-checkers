using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DAL;
using DAL.DTO;

namespace WebApp.Pages.Players
{
    public class CreateModel : PageModel
    {
        private readonly IPlayerDbRepository _playerRepository;
        
        public CreateModel(IGameDbRepository gameDbRepository)
        {
            _playerRepository = gameDbRepository.GetPlayerRepository();
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Player Player { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        { 
            if (await _playerRepository.GetByNameLazyAsync(Player.Name) != null)
            {
                ModelState.AddModelError("unique-name", "Name must be unique");
            }
            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            await _playerRepository.AddAsync(Player);

            return RedirectToPage("./Index");
        }
    }
}
