using Microsoft.AspNetCore.Mvc.RazorPages;
using DAL;
using DAL.DTO;

namespace WebApp.Pages.GameOptions
{
    public class IndexModel : PageModel
    {
        private readonly IOptionsDbRepository _optionsRepository;

        public IndexModel(IGameDbRepository gameRepository)
        {
            _optionsRepository = gameRepository.GetOptionsRepository();
        }

        public IList<CheckersGameOptions> CheckersGameOptions { get;set; } = default!;

        public async Task OnGetAsync()
        {
            CheckersGameOptions = await _optionsRepository.GetAllAsync();
        }
    }
}
