namespace GameBrain
{
    public class Player
    {
        public string Name { get; }
        public EColor Color { get; }
        public bool IsCurrent { get; set; }
        public int ButtonCount { get; set; }

        public Player(string name, EColor color, bool isCurrent, int buttonCount)
        {
            Name = name;
            Color = color;
            IsCurrent = isCurrent;
            ButtonCount = buttonCount;
        }
    }
}