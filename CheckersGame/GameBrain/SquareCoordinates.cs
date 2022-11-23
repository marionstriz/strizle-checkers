namespace GameBrain
{
    public class SquareCoordinates
    {
        public char X { get; }
        public int Y { get; }
    
        public SquareCoordinates(char x, int y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || !(obj.GetType() == GetType()))
            {
                return false;
            }
            var other = (SquareCoordinates) obj;
            return other.X == X && other.Y == Y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
    }
}
