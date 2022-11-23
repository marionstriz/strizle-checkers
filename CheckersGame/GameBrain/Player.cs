using System.Text.Json.Serialization;
using Domain;

namespace GameBrain
{
    public class Player
    {
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
            Name = dPlayer.Name;
            Color = dPlayer.Color;
            IsCurrent = dPlayer.IsCurrent;
        }

        [JsonConstructor]
        public Player(string name, EColor color, bool isCurrent)
        {
            Name = name;
            Color = color;
            IsCurrent = isCurrent;
        }

        public Domain.Player ToDomainPlayer()
        {
            var dPlayer = new Domain.Player
            {
                Name = Name,
                Color = Color,
                IsCurrent = IsCurrent
            };

            return dPlayer;
        }
    }
}