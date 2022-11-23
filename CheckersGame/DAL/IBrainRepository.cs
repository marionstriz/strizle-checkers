using GameBrain;

namespace DAL;

public interface IBrainRepository
{
    List<string> GetBrainFileNames();
    CheckersBrain GetBrain(string name);
    void SaveBrain(CheckersBrain brain, string name);
    void DeleteBrain(string name);
}