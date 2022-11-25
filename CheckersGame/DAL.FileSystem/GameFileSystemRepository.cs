using System.Text.Json;
using GameBrain;

namespace DAL.FileSystem
{
    public class GameFileSystemRepository : IGameRepository
    {
        private const string FileExtension = "json";
        private readonly string _brainsDirectory = "." + Path.DirectorySeparatorChar + "Saves";
        private const ESaveType SaveType = ESaveType.File;

        public List<string> GetGameFileNames() => GetListFromGameSaveFiles(_ => true);

        public List<string> GetGameFileNamesContaining(string substring) =>
            GetListFromGameSaveFiles(s => s.Contains(substring));

        public ESaveType GetSaveType() => SaveType;

        public CheckersGame GetGameByName(string name)
        {
            var fileContent = File.ReadAllText(GetFileName(name));
            var gameDto = JsonSerializer.Deserialize<DAL.DTO.CheckersGame>(fileContent);
            if (gameDto == null)
            {
                throw new FileLoadException($"Could not deserialize: {fileContent}");
            }
            return new CheckersGame(gameDto, SaveType);
        }

        public void SaveGame(CheckersGame game, string name)
        {
            CheckOrCreateDirectory();
            if (game.SaveOptions == null
                || !game.SaveOptions.Name.Equals(name)
                || !game.SaveOptions.SaveType.Equals(SaveType))
            {
                game.SaveOptions = new SaveOptions(name, SaveType);
            }
            var fileContent = JsonSerializer.Serialize(game.ToDto());
            File.WriteAllText(GetFileName(name), fileContent);
        }

        public void DeleteGame(string name)
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

        private List<string> GetListFromGameSaveFiles(Predicate<string> p)
        {
            CheckOrCreateDirectory();
            var res = new List<string>();
            var jsonFiles = Directory.GetFileSystemEntries(_brainsDirectory, "*." + FileExtension);
            foreach (var file in jsonFiles)
            {
                var fileName = Path.GetFileNameWithoutExtension(file);
                if (p.Invoke(fileName)) res.Add(fileName);
            }
            res = res.Order().ToList();
            return res;
        }
    }
}