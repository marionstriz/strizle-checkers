using System.Text.Json;
using GameBrain;

namespace DAL.FileSystem
{
    public class BrainFileSystemRepository : IBrainRepository
    {
        private const string FileExtension = "json";
        private readonly string _brainsDirectory = "." + Path.DirectorySeparatorChar + "Saves";
        private const ESaveType SaveType = ESaveType.File;

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
        
        public ESaveType GetSaveType() => SaveType;

        public CheckersBrain GetBrain(string name)
        {
            var fileContent = File.ReadAllText(GetFileName(name));
            var brain = JsonSerializer.Deserialize<CheckersBrain>(fileContent);
            if (brain == null)
            {
                throw new NullReferenceException($"Could not deserialize: {fileContent}");
            }
            return brain;
        }

        public void SaveBrain(CheckersBrain brain, string name)
        {
            CheckOrCreateDirectory();
            if (brain.SaveOptions == null
                || !brain.SaveOptions.Name.Equals(name)
                || !brain.SaveOptions.SaveType.Equals(SaveType))
            {
                brain.SaveOptions = new SaveOptions(name, SaveType);
            }
            var fileContent = JsonSerializer.Serialize(brain);
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
}