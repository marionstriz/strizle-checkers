using Microsoft.AspNetCore.Mvc.RazorPages;
using DAL;
using DAL.DTO;

namespace WebApp.Pages.CheckersGames;

public class IndexModel : PageModel
{
    private readonly IGameDbRepository _repository;

    public IndexModel(IGameDbRepository repository)
    {
        _repository = repository;
    }

    public IList<CheckersGame> CheckersGame { get;set; } = default!;

    public async Task OnGetAsync()
    {
        CheckersGame = await _repository.GetAllAsync();
    }
}