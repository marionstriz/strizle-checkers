using System.Text;

namespace GameBrain.Board;

public class Board
{
    public readonly char[] AlphabetChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
    public Square[,] Squares { get; }

    public Board(int width, int height)
    {
        Squares = new Square[height, width];

        int alphaIndex = 0;
        int coordNumber = height;

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (j == 0)
                {
                    alphaIndex = 0;
                }
                ESquareState squareState = ESquareState.Empty;
                int lowerLimit = height / 2;
                int upperLimit = height / 2 + (height % 2 == 1 ? 2 : 1);
                
                if (coordNumber < lowerLimit && IsButtonSquare(alphaIndex, coordNumber))
                {
                    squareState = ESquareState.Black;
                } else if (coordNumber > upperLimit && IsButtonSquare(alphaIndex, coordNumber))
                {
                    Console.WriteLine(upperLimit);
                    squareState = ESquareState.White;
                }
                Squares[i, j] = new Square(
                    new SquareCoordinates(AlphabetChars[alphaIndex], coordNumber), squareState);
                alphaIndex++;
            }
            coordNumber--;
        }
    }

    private bool IsButtonSquare(int coordAlphaIndex, int coordNumber)
    {
        return coordAlphaIndex % 2 == 0 && coordNumber % 2 == 1
               || coordAlphaIndex % 2 == 1 && coordNumber % 2 == 0;
    }
}