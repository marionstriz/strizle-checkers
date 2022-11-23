using GameBrain;

namespace DAL;

public interface IBrainRepository
{
    ESaveType GetSaveType();
    List<string> GetBrainFileNames();
    CheckersBrain GetBrain(string name);
    void SaveBrain(CheckersBrain brain, string name);
    void DeleteBrain(string name);
}