using System.Collections.Generic;

namespace Tetricity
{
	public interface IFormationOperations
	{
		/// <summary>
		/// Checks destination cells to validate that move is possible.
		/// </summary>
		/// <returns><c>true</c>, if perform move is possible, <c>false</c> otherwise.</returns>
		/// <param name="activeCells">Active formation cells.</param>
		/// <param name="board">Game board..</param>
		/// <param name="moveType">Formation move type.</param>
		bool CanPerformMove(ref IList<CellEntity> activeCells, ref IBlock[,] board, MoveType moveType);

		/// <summary>
		/// Performs the cell move.
		/// </summary>
		/// <param name="board">Game board.</param>
		/// <param name="blockType">Block type.</param>
		/// <param name="oldX">Old cell x.</param>
		/// <param name="oldY">Old cell y.</param>
		/// <param name="newX">New cell x.</param>
		/// <param name="newY">New cell y.</param>
		/// <param name="boardX">Game board starting x location.</param>
		/// <param name="boardY">Game board starting y location.</param>
		/// <param name="blockWidth">Block width.</param>
		/// <param name="blockHeight">Block height.</param>
		void PerformMove(ref IBlock[,] board, BlockType blockType, int oldX, int oldY, int newX, int newY, int boardX, int boardY, int blockWidth, int blockHeight);

		/// <summary>
		/// Generates a new block formation.
		/// </summary>
		/// <returns>The new block formation.</returns>
		/// <param name="board">Game board.</param>
		/// <param name="blockWidth">Block width.</param>
		/// <param name="blockHeight">Block height.</param>
		IList<CellEntity> GenerateNewFormation(ref IBlock[,] board, int blockWidth, int blockHeight);

		/// <summary>
		/// Rotates the block formation.
		/// </summary>
		/// <param name="board">Game board.</param>
		/// <param name="activeCells">Active block cells.</param>
		/// <param name="boardX">Game board starting x location.</param>
		/// <param name="boardY">Game board starting y location.</param>
		/// <param name="blockWidth">Block width.</param>
		/// <param name="blockHeight">Block height.</param>
		void RotateFormation(ref IBlock[,] board, ref IList<CellEntity> activeCells, int boardX, int boardY, int blockWidth, int blockHeight);
	}
}
