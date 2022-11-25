using System.Text.Json.Serialization;

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
        
        public Player(DAL.DTO.Player dto)
        {
            if (dto.Color is not (0 or 1))
            {
                throw new ArgumentException(
                    $"Unable to create player from DTO - color value must be 0 or 1, was {dto.Color}.");
            }
            Name = dto.Name;
            Color = (EColor) dto.Color;
            IsCurrent = dto.IsCurrent;
        }

        [JsonConstructor]
        public Player(string name, EColor color, bool isCurrent)
        {
            Name = name;
            Color = color;
            IsCurrent = isCurrent;
        }

        public DAL.DTO.Player ToDto()
        {
            return new DAL.DTO.Player
            {
                Name = Name,
                Color = (int) Color,
                IsCurrent = IsCurrent
            };
        }
    }
}