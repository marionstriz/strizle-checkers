using GameBrain;

namespace DAL;

public interface IGameRepository
{
    ESaveType GetSaveType();
    List<string> GetGameFileNames();
    List<string> GetGameFileNamesContaining(string substring);
    CheckersGame GetGameByName(string name);
    void SaveGame(CheckersGame game, string name);
    void DeleteGame(string name);
}