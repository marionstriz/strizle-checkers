namespace GameBrain;

public class SaveOptions
{
    public string Name { get; set; }
    public ESaveType SaveType { get; set; }

    public SaveOptions(string name, ESaveType saveType)
    {
        Name = name;
        SaveType = saveType;
    }
}