using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.CheckersGames;

public class GameOver : PageModel
{
    private IGameDbRepository GameRepository { get; }

    public GameOver(IGameDbRepository gameDbRepository)
    {
        GameRepository = gameDbRepository;
    }

    public string GameOverHeading { get; set; } = default!;
    
    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null) return NotFound();
        
        var gameDto = await GameRepository.GetByIdLazyAsync(id.Value);
        
        if (gameDto == null) return NotFound();

        GameOverHeading = gameDto.GameWonByPlayer == null ?
            "Game not over yet, please head to play page to continue playing." :
            $"Congratulations, {gameDto.GameWonByPlayer} has won the game!";

        return Page();
    }
}