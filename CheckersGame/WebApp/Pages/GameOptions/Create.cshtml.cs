using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DAL;
using DAL.DTO;

namespace WebApp.Pages.GameOptions
{
    public class CreateModel : PageModel
    {
        private readonly IOptionsDbRepository _optionsRepository;

        public CreateModel(IGameDbRepository gameRepository)
        {
            _optionsRepository = gameRepository.GetOptionsRepository();
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public CheckersGameOptions CheckersGameOptions { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        { 
            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            await _optionsRepository.AddAsync(CheckersGameOptions);
            
            return RedirectToPage("./Index");
        }
    }
}
