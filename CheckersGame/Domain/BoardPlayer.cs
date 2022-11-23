using System.Text.Json.Serialization;

namespace Domain;

public class BoardPlayer
{
    public int Id { get; set; }
    
    public int BoardId { get; set; }
    public Board? Board { get; set; }
    
    public int PlayerId { get; set; }
    public Player? Player { get; set; }
}