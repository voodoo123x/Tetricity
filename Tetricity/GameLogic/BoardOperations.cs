using System.Collections.Generic;
using System.Linq;

namespace Tetricity
{
	public class BoardOperations : IBoardOperations
	{
		static BoardOperations _Instance;

		public static BoardOperations Instance
		{
			get 
			{
				if (_Instance == null)
				{
					_Instance = new BoardOperations();
				}

				return _Instance;
			}
		}

		public void Reset(ref IBlock[,] board, int boardWidth, int boardHeight, int blockWidth, int blockHeight, int boardX, int boardY, out int score, out int rowsCompleted)
		{
			for (int i = 0; i < boardWidth; i++)
			{
				for (int j = 0; j < boardHeight; j++)
				{
					BlockType blockType = BlockType.Empty;

					if (i == 0 || i == boardWidth - 1 || j == boardHeight - 1)
					{
						blockType = BlockType.Wall;
					}

					int x = (i * blockWidth) + boardX;
					int y = (j * blockHeight) + boardY;
					board[i, j] = new Block(blockType, blockHeight, blockWidth, x, y);
				}
			}

			score = 0;
			rowsCompleted = 0;
		}

		public IEnumerable<int> CheckRowsCompleted(ref IBlock[,] board)
		{
			IList<int> completedRows = new List<int>();

			for (int i = 0; i < board.GetLength(1) - 1; i++)
			{
				for (int j = 1; j < board.GetLength(0) - 1; j++)
				{
					if (board[j, i].GetBlockType() == BlockType.Empty)
					{
						break;
					}
					else if (board[j, i].GetBlockType() != BlockType.Empty && j == board.GetLength(0) - 2)
					{
						completedRows.Add(i);
					}
				}
			}

			if (completedRows.Count == 0)
			{
				completedRows.Add(-1);
			}

			return completedRows.OrderByDescending(r => r);
		}

		public int RemoveRow(ref IBlock[,] board, int row, int blockWidth, int blockHeight, int boardX, int boardY)
		{
			int score = 0;

			for (int i = row; i > 0; i--)
			{
				for (int j = 1; j < board.GetLength(0) - 1; j++)
				{
					if (i == row)
					{
						int x = (j * blockWidth) + boardX;
						int y = (i * blockHeight) + boardY;
						board[j, i] = new Block(BlockType.Empty, x, y, boardX, boardY);
						score += 100;
					}
					else if (board[j, i].GetBlockType() != BlockType.Empty)
					{
						BlockType blockType = board[j, i].GetBlockType();
						int x = (j * blockWidth) + boardX;
						int y = (i * blockHeight) + boardY;
						board[j, i] = new Block(BlockType.Empty, blockWidth, blockHeight, x, y);

						x = (j * blockWidth) + boardX;
						y = ((i + 1) * blockHeight) + boardY;
						board[j, i + 1] = new Block(blockType, blockWidth, blockHeight, x, y);
					}
				}
			}

			return score;
		}

		public bool IsGameOver(ref IBlock[,] board, IList<CellEntity> formation)
		{
			bool unableToPlace = false;

			foreach (var cell in formation)
			{
				if (board[cell.X, cell.Y].GetBlockType() != BlockType.Empty)
				{
					unableToPlace = true;
					break;
				}
			}

			return unableToPlace;
		}
	}
}
