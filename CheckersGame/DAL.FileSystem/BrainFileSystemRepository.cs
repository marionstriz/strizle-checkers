using System.Text.Json;
using GameBrain;

namespace DAL.FileSystem
{
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
            var dBrain = JsonSerializer.Deserialize<Domain.CheckersBrain>(fileContent);
            if (dBrain == null)
            {
                throw new NullReferenceException($"Could not deserialize: {fileContent}");
            }
            return new CheckersBrain(dBrain);
        }

        public void SaveBrain(CheckersBrain brain, string name)
        {
            CheckOrCreateDirectory();
            if (!name.Equals(brain.FileName))
            {
                brain.FileName = name;
            }
            var fileContent = JsonSerializer.Serialize(brain.ToDomainBrain(true), new JsonSerializerOptions
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
}