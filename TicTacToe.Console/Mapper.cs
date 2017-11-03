using System.Collections.Generic;
using System.Text;

namespace SandTigerShark.TicTacToe.App
{
    internal class Mapper
    {
        private static IReadOnlyDictionary<int, char> mapToTicTacToe = new Dictionary<int, char>
        {
            { 0, ' ' },
            { 1, 'O' },
            { 2, 'X' }
        };

        public static string[] ToVisualBoard(int[] board)
        {
            var visualboard = new string[3];
            visualboard[0] = GetVisualLine(board, 0, 2);
            visualboard[1] = GetVisualLine(board, 3, 5);
            visualboard[2] = GetVisualLine(board, 6, 8);

            return visualboard;
        }

        private static string GetVisualLine(
            int[] board,
            int startIndex, 
            int endIndex)
        {
            var line = new StringBuilder("|");

            for (int i = startIndex; i <= endIndex; i++)
            {
                line.Append($"{mapToTicTacToe[board[i]]}|");
            }
            return line.ToString();
        }
    }
}
