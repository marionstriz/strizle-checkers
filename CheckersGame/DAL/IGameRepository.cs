using GameBrain;

namespace DAL;

public interface IGameRepository
{
    ESaveType GetSaveType();
    List<string> GetGameFileNames();
    List<string> GetGameFileNamesContaining(string substring);
    CheckersGame? GetBrainGameByName(string name);
    void SaveBrainGameByName(CheckersGame game, string name);
    void DeleteByName(string name);
}