using System.Collections.Generic;

namespace Tetricity
{
	public interface IBoardOperations
	{
		void Reset(ref IBlock[,] board, int boardWidth, int boardHeight, int blockWidth, int blockHeight, int boardX, int boardY, out int score);

		IEnumerable<int> CheckRowsCompleted(ref IBlock[,] board);

		int RemoveRow(ref IBlock[,] board, int row, int blockWidth, int blockHeight, int boardX, int boardY);
	}
}
