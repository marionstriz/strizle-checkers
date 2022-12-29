using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DAL.DTO;

namespace WebApp.Pages.CheckersGames
{
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly IGameDbRepository _repository;

        public CreateModel(AppDbContext context, IGameDbRepository repository)
        {
            _context = context;
            _repository = repository;
        }

        public IActionResult OnGet()
        {
            OptionsSelectList = new SelectList(_context.GameOptions, "Id", "Id");
            PlayerSelectList = new SelectList(_context.Players, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public CheckersGame CheckersGame { get; set; } = default!;
        public SelectList OptionsSelectList { get; set; } = default!;
        public SelectList PlayerSelectList { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (await _repository.GetByNameLazyAsync(CheckersGame.Name) != null)
            {
                ModelState.AddModelError("unique-name", "Name must be unique");
            }
            if (!ModelState.IsValid)
            {
                return OnGet();
            }

            await _repository.AddAsync(CheckersGame);

            return RedirectToPage("./Play");
        }
    }
}
