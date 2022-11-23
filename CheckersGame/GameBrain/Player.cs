namespace GameBrain
{
    public class Player
    {
        private int? Id { get; }
        public string Name { get; }
        public EColor Color { get; }
        public bool IsCurrent { get; set; }
    
        public Player(string name, EColor color)
        {
            Name = name;
            Color = color;
        }
        
        public Player(Domain.Player dPlayer)
        {
            if (dPlayer.Color is not (0 or 1))
            {
                throw new ArgumentException(
                    "Unable to initialize player from Domain object - invalid enum values.");
            }
            Id = dPlayer.Id;
            Name = dPlayer.Name;
            Color = (EColor) dPlayer.Color;
            IsCurrent = dPlayer.IsCurrent;
        }
        
        public Domain.Player ToDomainPlayer()
        {
            var dPlayer = new Domain.Player
            {
                Name = Name,
                Color = (int) Color,
                IsCurrent = IsCurrent
            };
            if (Id != null)
            {
                dPlayer.Id = (int) Id;
            }

            return dPlayer;
        }
    }
}