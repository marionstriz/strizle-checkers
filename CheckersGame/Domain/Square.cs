namespace Domain;

public class Square
{
    public int Id { get; set; }
    public char X { get; set; }
    public int Y { get; set; }
    
    public int ButtonId { get; set; }
    public Button? Button { get; set; }
    
    public int BoardId { get; set; }
    public Board? Board { get; set; }
}