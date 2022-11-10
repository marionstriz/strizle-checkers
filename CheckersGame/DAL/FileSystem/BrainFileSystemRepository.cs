using System.Text.Json;
using GameBrain;
using GameBrain.DTO;

namespace DAL.FileSystem;

public class BrainFileSystemRepository : IBrainRepository
{
    private const string FileExtension = "json";
    private readonly string _brainsDirectory = "." + Path.DirectorySeparatorChar + "Saves";

    public List<string> GetBrainFileNames()
    {
        CheckOrCreateDirectory();
        
        var res = new List<string>();
        
        foreach (var fileName in Directory.GetFileSystemEntries(_brainsDirectory, "*." + FileExtension))
        {
            res.Add(Path.GetFileNameWithoutExtension(fileName));
        }
        return res;
    }

    public CheckersBrain GetBrain(string name)
    {
        var fileContent = File.ReadAllText(GetFileName(name));
        var brainDto = JsonSerializer.Deserialize<CheckersBrainDto>(fileContent);
        if (brainDto == null)
        {
            throw new NullReferenceException($"Could not deserialize: {fileContent}");
        }

        var brain = brainDto.GetBrain();
        Console.WriteLine(brain.Board.PlayerOne.Name);
        return brainDto.GetBrain();
    }

    public void SaveBrain(CheckersBrain brain, string name)
    {
        CheckOrCreateDirectory();
        var brainDto = new CheckersBrainDto(brain);
        var fileContent = JsonSerializer.Serialize(brainDto, new JsonSerializerOptions
        {
            WriteIndented = true
        });
        File.WriteAllText(GetFileName(name), fileContent);
    }

    public void DeleteBrain(string name)
    {
        File.Delete(GetFileName(name));
    }
    
    private string GetFileName(string name)
    {
        return _brainsDirectory + Path.DirectorySeparatorChar + name + "." + FileExtension;
    }

    private void CheckOrCreateDirectory()
    {
        if (!Directory.Exists(_brainsDirectory))
        {
            Directory.CreateDirectory(_brainsDirectory);
        }
    }
}