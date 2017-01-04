using System.Collections.Generic;

namespace Tetricity
{
	public interface IBoardOperations
	{
		/// <summary>
		/// Reset the specified game board to default empty fields.
		/// </summary>
		/// <param name="board">Game board.</param>
		/// <param name="boardWidth">Game board width.</param>
		/// <param name="boardHeight">Game board height.</param>
		/// <param name="blockWidth">Block width.</param>
		/// <param name="blockHeight">Block height.</param>
		/// <param name="boardX">Game board starting x location.</param>
		/// <param name="boardY">Game board starting y location.</param>
		/// <param name="score">Score.</param>
		/// <param name="rowsCompleted">Rows completed.</param>
		void Reset(ref IBlock[,] board, int boardWidth, int boardHeight, int blockWidth, int blockHeight, int boardX, int boardY, out int score, out int rowsCompleted);

		/// <summary>
		/// Check for rows that are completed.
		/// </summary>
		/// <returns>The row number for the completed rows.</returns>
		/// <param name="board">Game board.</param>
		IEnumerable<int> CheckRowsCompleted(ref IBlock[,] board);

		/// <summary>
		/// Removes the specified row.
		/// </summary>
		/// <returns>The score accumulated from removing the row.</returns>
		/// <param name="board">Game board.</param>
		/// <param name="row">Row to be removed.</param>
		/// <param name="blockWidth">Block width.</param>
		/// <param name="blockHeight">Block height.</param>
		/// <param name="boardX">Game board starting x location.</param>
		/// <param name="boardY">Game board starting y location.</param>
		int RemoveRow(ref IBlock[,] board, int row, int blockWidth, int blockHeight, int boardX, int boardY);

		/// <summary>
		/// Checks to see if game is over when placing new formation.
		/// </summary>
		/// <returns><c>true</c>, if game over due to blocked beginning move, <c>false</c> otherwise.</returns>
		/// <param name="board">Game board.</param>
		/// <param name="formation">New formation being placed.</param>
		bool IsGameOver(ref IBlock[,] board, IList<CellEntity> formation);
	}
}
