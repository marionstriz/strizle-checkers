using System.Text;
using DAL;
using GameBrain;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CheckersGame = GameBrain.CheckersGame;

namespace WebApp.Pages.CheckersGames;

public class PlayModel : PageModel
{
    private readonly IGameDbRepository _repository;

    public PlayModel(IGameDbRepository repository)
    {
        _repository = repository;
    }

    public CheckersGame Game { get; set; } = default!;
    public int Id { get; set; }
    public Player Player { get; set; } = default!;
    public Player Opponent { get; set; } = default!;
    public string? Error { get; set; }
    private string OnClick { get; set; } = "";

    public List<int> ValidMoves { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id, int? player, int? source, int? dest)
    {
        if (id == null || player == null || player != 1 && player != 2) return NotFound();
        
        Id = id.Value;
        var gameDto = await _repository.GetByIdAsync(Id);
        
        if (gameDto == null) return NotFound();

        if (gameDto.GameWonByPlayer != null) return RedirectToPage("/CheckersGames/GameOver", new {id});
        
        Game = new CheckersGame(gameDto, ESaveType.Database);

        if (player == 1)
        {
            Player = Game.PlayerOne;
            Opponent = Game.PlayerTwo;
        }
        else
        {
            Player = Game.PlayerTwo;
            Opponent = Game.PlayerOne;
        }
        
        SetOnClick(source);

        List<Move> sourceMovesList;
        if (Game.NextMoves != null) {

            sourceMovesList = Game.NextMoves;
            ValidMoves = sourceMovesList!.Select(m => m.Destination).ToList();
        }
        else
        {
            var movesDict = Game.GetCurrentPlayerMoves();
            var keyList = movesDict.Keys.ToList();
        
            if (source == null)
            {
                ValidMoves = keyList;
                return Page();
            }
            if (!movesDict.ContainsKey(source.Value))
            {
                ValidMoves = keyList;
                return ValidSquareError();
            }
            sourceMovesList = movesDict.GetValueOrDefault(source.Value)!;
            ValidMoves = sourceMovesList.Select(m => m.Destination).ToList();
        }

        if (dest == null) return Page();
        if (!ValidMoves.Contains(dest.Value)) return ValidSquareError();
        
        Game.MakeMove(sourceMovesList.Find(m => m.Destination == dest.Value)!);

        await _repository.SaveBrainGameAsync(Game);
        
        return RedirectToPage("/CheckersGames/Play", new {id, player});
    }

    public int PlayerNr()
    {
        return Game.PlayerOne == Player ? 1 : 2;
    }

    private PageResult ValidSquareError()
    {
        return SetError("Please select a valid square");
    }

    private PageResult SetError(string error)
    {
        Error = error;
        return Page();
    }

    private void SetOnClick(int? source)
    {
        if (!IsCurrentPlayer()) return;
        
        var url = Request.GetDisplayUrl();
        var onclick = $"onclick=window.location='{url}";
        if (source == null && Game.NextMoves == null) onclick += "&source=";
        else {
            if (onclick.Contains("&dest=")) onclick = onclick.Split("&dest=")[0]; 
            onclick += "&dest=";
        }

        OnClick = onclick;
    }

    public string GetOnClickWithIndex(int index)
    {
        if (!IsCurrentPlayer()) return OnClick;
        return OnClick + index + "'";
    }

    public bool IsCurrentPlayer()
    {
        return Game.GetCurrentTurnPlayer() == Player;
    }

    public string GetSquareClasses(int index)
    {
        var square = Game.Board.Squares[index];
        var sb = new StringBuilder();

        if (square.IsMovableSquare())
        {
            if (IsCurrentPlayer() && ValidMoves.Contains(index)) sb.Append("highlighted-square");
            else sb.Append("movable-square");
        }
        if (!square.HasButton()) return sb.ToString();
        
        if (square.IsColorButtonSquare(EColor.Black))
        {
            sb.Append(square.Button!.IsSupermario() ?
                " black-king-button-square" : " black-button-square");
        }
        else sb.Append(square.Button!.IsSupermario() ?
            " white-king-button-square" : " white-button-square");
        
        return sb.ToString();
    }
}