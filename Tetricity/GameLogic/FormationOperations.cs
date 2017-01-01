using System;
using System.Collections.Generic;
using System.Linq;

namespace Tetricity
{
	public class FormationOperations
	{
		static FormationOperations _Instance;

		public static FormationOperations Instance
		{
			get 
			{
				if (_Instance == null)
				{
					_Instance = new FormationOperations();
				}

				return _Instance;
			}
		}

		public IList<CellEntity> GenerateNewFormation(ref IBlock[,] board, int blockWidth, int blockHeight)
		{
			IList<CellEntity> newFormation = new List<CellEntity>();
			Random random = new Random(DateTime.Now.Millisecond);
			BlockType blockType = BlockType.Empty;

			switch (random.Next(0, 4))
			{
				case 0: // Line
					blockType = BlockType.Blue;
					newFormation.Add(new CellEntity { BlockType = blockType, Orientation = FormationOrientation.Vertical1, X = 6, Y = 0 });
					newFormation.Add(new CellEntity { BlockType = blockType, Orientation = FormationOrientation.Vertical1, X = 6, Y = 1 });
					newFormation.Add(new CellEntity { BlockType = blockType, Orientation = FormationOrientation.Vertical1, X = 6, Y = 2 });
					newFormation.Add(new CellEntity { BlockType = blockType, Orientation = FormationOrientation.Vertical1, X = 6, Y = 3 });
					break;

				case 1: // Left L
					blockType = BlockType.Red;
					newFormation.Add(new CellEntity { BlockType = blockType, Orientation = FormationOrientation.Vertical1, X = 6, Y = 0 });
					newFormation.Add(new CellEntity { BlockType = blockType, Orientation = FormationOrientation.Vertical1, X = 6, Y = 1 });
					newFormation.Add(new CellEntity { BlockType = blockType, Orientation = FormationOrientation.Vertical1, X = 6, Y = 2 });
					newFormation.Add(new CellEntity { BlockType = blockType, Orientation = FormationOrientation.Vertical1, X = 5, Y = 2 });
					break;

				case 2: // Right L
					blockType = BlockType.Purple;
					newFormation.Add(new CellEntity { BlockType = blockType, Orientation = FormationOrientation.Vertical1, X = 6, Y = 0 });
					newFormation.Add(new CellEntity { BlockType = blockType, Orientation = FormationOrientation.Vertical1, X = 6, Y = 1 });
					newFormation.Add(new CellEntity { BlockType = blockType, Orientation = FormationOrientation.Vertical1, X = 6, Y = 2 });
					newFormation.Add(new CellEntity { BlockType = blockType, Orientation = FormationOrientation.Vertical1, X = 7, Y = 2 });
					break;

				case 3: // Square
					blockType = BlockType.Green;
					newFormation.Add(new CellEntity { BlockType = blockType, Orientation = FormationOrientation.Vertical1, X = 6, Y = 0 });
					newFormation.Add(new CellEntity { BlockType = blockType, Orientation = FormationOrientation.Vertical1, X = 6, Y = 1 });
					newFormation.Add(new CellEntity { BlockType = blockType, Orientation = FormationOrientation.Vertical1, X = 5, Y = 0 });
					newFormation.Add(new CellEntity { BlockType = blockType, Orientation = FormationOrientation.Vertical1, X = 5, Y = 1 });
					break;
			}

			foreach (var block in newFormation)
			{
				board[block.X, block.Y] = new Block(blockType, blockWidth, blockHeight, 6 * blockWidth, 0);
			}

			return newFormation;
		}

		public bool CanPerformMove(ref IList<CellEntity> activeCells, ref IBlock[,] board, MoveType moveType)
		{
			bool canMove = false;
			int boardWidth = board.GetLength(0);
			int boardHeight = board.GetLength(1);

			switch (moveType)
			{
				case MoveType.Down:
					foreach (var cell in activeCells)
					{
						int newX = cell.X;
						int newY = cell.Y + 1;

						if (newX < boardWidth && newX > 0 && newY < boardHeight)
						{
							if (board[newX, newY].GetBlockType() == BlockType.Empty
								|| activeCells.Any(c => c.X == newX && c.Y == newY))
							{
								canMove = true;
							}
							else
							{
								canMove = false;
								break;
							}
						}
						else
						{
							canMove = false;
							break;
						}
					}
					break;

				case MoveType.Left:
					foreach (var cell in activeCells)
					{
						int newX = cell.X - 1;
						int newY = cell.Y;

						if (newX < boardWidth && newX > 0 && newY < boardHeight)
						{
							if (board[newX, newY].GetBlockType() == BlockType.Empty
								|| activeCells.Any(c => c.X == newX && c.Y == newY))
							{
								canMove = true;
							}
							else
							{
								canMove = false;
								break;
							}
						}
						else
						{
							canMove = false;
							break;
						}
					}
					break;

				case MoveType.Right:
					foreach (var cell in activeCells)
					{
						int newX = cell.X + 1;
						int newY = cell.Y;

						if (newX < boardWidth && newX > 0 && newY < boardHeight)
						{
							if (board[newX, newY].GetBlockType() == BlockType.Empty
								|| activeCells.Any(c => c.X == newX && c.Y == newY))
							{
								canMove = true;
							}
							else
							{
								canMove = false;
								break;
							}
						}
						else
						{
							canMove = false;
							break;
						}
					}
					break;
			}

			return canMove;
		}

		public void PerformMove(ref IBlock[,] board, BlockType blockType, int oldX, int oldY, int newX, int newY, int boardX, int boardY, int blockWidth, int blockHeight)
		{
			int x = (oldX * blockWidth) + boardX;
			int y = (oldY * blockHeight) + boardY;
			board[oldX, oldY] = new Block(BlockType.Empty, blockWidth, blockHeight, x, y);

			x = (newX * blockWidth) + boardX;
			y = (newY * blockHeight) + boardY;
			board[newX, newY] = new Block(blockType, blockWidth, blockHeight, x, y);
		}
	
		public void RotateFormation(ref IBlock[,] board, ref IList<CellEntity> activeCells, int boardX, int boardY, int blockWidth, int blockHeight)
		{
			bool canRotate = true;
			IList<CellEntity> newCells = new List<CellEntity>();
			BlockType blockType = activeCells.First().BlockType;

			switch (blockType)
			{
				case BlockType.Blue:  // Line
					for (int i = 0; i < activeCells.Count; i++)
					{
						int newX = 0, newY = 0;

						if (activeCells[i].Orientation == FormationOrientation.Vertical1
						   || activeCells[i].Orientation == FormationOrientation.Vertical2)
						{
							switch (i)
							{
								case 0:
									newX = activeCells[i].X - 2;
									newY = activeCells[i].Y - 2;
									break;

								case 1:
									newX = activeCells[i].X - 1;
									newY = activeCells[i].Y - 1;
									break;

								case 2:
									newX = activeCells[i].X;
									newY = activeCells[i].Y;
									break;

								case 3:
									newX = activeCells[i].X + 1;
									newY = activeCells[i].Y + 1;
									break;
							}
						}
						else if (activeCells[i].Orientation == FormationOrientation.Horizontal1
						        || activeCells[i].Orientation == FormationOrientation.Horizontal2)
						{
							switch (i)
							{
								case 0:
									newX = activeCells[i].X + 2;
									newY = activeCells[i].Y + 2;
									break;

								case 1:
									newX = activeCells[i].X + 1;
									newY = activeCells[i].Y + 1;
									break;

								case 2:
									newX = activeCells[i].X;
									newY = activeCells[i].Y;
									break;

								case 3:
									newX = activeCells[i].X - 1;
									newY = activeCells[i].Y - 1;
									break;
							}
						}

						if (newX > 0 && newX < board.GetLength(0) - 1 && newY > 0 && newY < board.GetLength(1) - 1)
						{
							if (board[newX, newY].GetBlockType() == BlockType.Empty || activeCells.Any(c => c.X == newX && c.Y == newY))
							{
								FormationOrientation orientation = FormationOrientation.Horizontal1;
								if (activeCells[i].Orientation == FormationOrientation.Horizontal1)
								{
									orientation = FormationOrientation.Vertical1;
								}
								else if (activeCells[i].Orientation == FormationOrientation.Vertical1)
								{
									orientation = FormationOrientation.Horizontal2;
								}
								else if (activeCells[i].Orientation == FormationOrientation.Horizontal2)
								{
									orientation = FormationOrientation.Vertical2;
								}
								else if (activeCells[i].Orientation == FormationOrientation.Vertical2)
								{
									orientation = FormationOrientation.Horizontal1;
								}


								newCells.Add(new CellEntity { BlockType = BlockType.Blue, Orientation = orientation, X = newX, Y = newY });
							}
						}
						else
						{
							canRotate = false;
							break;
						}
					}

					break;

				case BlockType.Purple:  // Right L
					for (int i = 0; i < activeCells.Count; i++)
					{
						int newX = 0, newY = 0;

						if (activeCells[i].Orientation == FormationOrientation.Vertical1)
						{
							switch (i)
							{
								case 0:
									newX = activeCells[i].X - 1;
									newY = activeCells[i].Y - 1;
									break;

								case 1:
									newX = activeCells[i].X - 2;
									newY = activeCells[i].Y;
									break;

								case 2:
									newX = activeCells[i].X;
									newY = activeCells[i].Y;
									break;

								case 3:
									newX = activeCells[i].X + 1;
									newY = activeCells[i].Y + 1;
									break;
							}
						}
						else if (activeCells[i].Orientation == FormationOrientation.Horizontal2)
						{
							switch (i)
							{
								case 0:
									newX = activeCells[i].X + 1;
									newY = activeCells[i].Y - 1;
									break;

								case 1:
									newX = activeCells[i].X;
									newY = activeCells[i].Y - 2;
									break;

								case 2:
									newX = activeCells[i].X;
									newY = activeCells[i].Y;
									break;

								case 3:
									newX = activeCells[i].X - 1;
									newY = activeCells[i].Y + 1;
									break;
							}
						}
						else if (activeCells[i].Orientation == FormationOrientation.Vertical2)
						{
							switch (i)
							{
								case 0:
									newX = activeCells[i].X + 1;
									newY = activeCells[i].Y + 1;
									break;

								case 1:
									newX = activeCells[i].X + 2;
									newY = activeCells[i].Y;
									break;

								case 2:
									newX = activeCells[i].X;
									newY = activeCells[i].Y;
									break;

								case 3:
									newX = activeCells[i].X - 1;
									newY = activeCells[i].Y - 1;
									break;
							}
						}
						else if (activeCells[i].Orientation == FormationOrientation.Horizontal1)
						{
							switch (i)
							{
								case 0:
									newX = activeCells[i].X - 1;
									newY = activeCells[i].Y + 1;
									break;

								case 1:
									newX = activeCells[i].X;
									newY = activeCells[i].Y + 2;
									break;

								case 2:
									newX = activeCells[i].X;
									newY = activeCells[i].Y;
									break;

								case 3:
									newX = activeCells[i].X + 1;
									newY = activeCells[i].Y - 1;
									break;
							}
						}

						if (newX > 0 && newX < board.GetLength(0) - 1 && newY > 0 && newY < board.GetLength(1) - 1)
						{
							if (board[newX, newY].GetBlockType() == BlockType.Empty || activeCells.Any(c => c.X == newX && c.Y == newY))
							{
								FormationOrientation orientation = FormationOrientation.Horizontal1;
								if (activeCells[i].Orientation == FormationOrientation.Horizontal1)
								{
									orientation = FormationOrientation.Vertical1;
								}
								else if (activeCells[i].Orientation == FormationOrientation.Vertical1)
								{
									orientation = FormationOrientation.Horizontal2;
								}
								else if (activeCells[i].Orientation == FormationOrientation.Horizontal2)
								{
									orientation = FormationOrientation.Vertical2;
								}
								else if (activeCells[i].Orientation == FormationOrientation.Vertical2)
								{
									orientation = FormationOrientation.Horizontal1;
								}


								newCells.Add(new CellEntity { BlockType = BlockType.Purple, Orientation = orientation, X = newX, Y = newY });
							}
						}
						else
						{
							canRotate = false;
							break;
						}
					}

					break;

				case BlockType.Red:  // Left L
					for (int i = 0; i < activeCells.Count; i++)
					{
						int newX = 0, newY = 0;

						if (activeCells[i].Orientation == FormationOrientation.Vertical1)
						{
							switch (i)
							{
								case 0:
									newX = activeCells[i].X;
									newY = activeCells[i].Y - 2;
									break;

								case 1:
									newX = activeCells[i].X - 1;
									newY = activeCells[i].Y - 1;
									break;

								case 2:
									newX = activeCells[i].X;
									newY = activeCells[i].Y;
									break;

								case 3:
									newX = activeCells[i].X + 1;
									newY = activeCells[i].Y + 1;
									break;
							}
						}
						else if (activeCells[i].Orientation == FormationOrientation.Horizontal2)
						{
							switch (i)
							{
								case 0:
									newX = activeCells[i].X + 2;
									newY = activeCells[i].Y;
									break;

								case 1:
									newX = activeCells[i].X + 1;
									newY = activeCells[i].Y - 1;
									break;

								case 2:
									newX = activeCells[i].X;
									newY = activeCells[i].Y;
									break;

								case 3:
									newX = activeCells[i].X - 1;
									newY = activeCells[i].Y + 1;
									break;
							}
						}
						else if (activeCells[i].Orientation == FormationOrientation.Vertical2)
						{
							switch (i)
							{
								case 0:
									newX = activeCells[i].X;
									newY = activeCells[i].Y + 2;
									break;

								case 1:
									newX = activeCells[i].X + 1;
									newY = activeCells[i].Y + 1;
									break;

								case 2:
									newX = activeCells[i].X;
									newY = activeCells[i].Y;
									break;

								case 3:
									newX = activeCells[i].X - 1;
									newY = activeCells[i].Y - 1;
									break;
							}
						}
						else if (activeCells[i].Orientation == FormationOrientation.Horizontal1)
						{
							switch (i)
							{
								case 0:
									newX = activeCells[i].X - 2;
									newY = activeCells[i].Y;
									break;

								case 1:
									newX = activeCells[i].X - 1;
									newY = activeCells[i].Y + 1;
									break;

								case 2:
									newX = activeCells[i].X;
									newY = activeCells[i].Y;
									break;

								case 3:
									newX = activeCells[i].X + 1;
									newY = activeCells[i].Y - 1;
									break;
							}
						}

						if (newX > 0 && newX < board.GetLength(0) - 1 && newY > 0 && newY < board.GetLength(1) - 1)
						{
							if (board[newX, newY].GetBlockType() == BlockType.Empty || activeCells.Any(c => c.X == newX && c.Y == newY))
							{
								FormationOrientation orientation = FormationOrientation.Horizontal1;
								if (activeCells[i].Orientation == FormationOrientation.Horizontal1)
								{
									orientation = FormationOrientation.Vertical1;
								}
								else if (activeCells[i].Orientation == FormationOrientation.Vertical1)
								{
									orientation = FormationOrientation.Horizontal2;
								}
								else if (activeCells[i].Orientation == FormationOrientation.Horizontal2)
								{
									orientation = FormationOrientation.Vertical2;
								}
								else if (activeCells[i].Orientation == FormationOrientation.Vertical2)
								{
									orientation = FormationOrientation.Horizontal1;
								}


								newCells.Add(new CellEntity { BlockType = BlockType.Red, Orientation = orientation, X = newX, Y = newY });
							}
						}
						else
						{
							canRotate = false;
							break;
						}
					}

					break;
			}

			if (canRotate && newCells.Count > 0)
			{
				foreach (var cell in activeCells)
				{
					int x = (cell.X * blockWidth) + boardX;
					int y = (cell.Y * blockHeight) + boardY;
					board[cell.X, cell.Y] = new Block(BlockType.Empty, blockWidth, blockHeight, x, y);
				}

				foreach (var cell in newCells)
				{
					int x = (cell.X * blockWidth) + boardX;
					int y = (cell.Y * blockHeight) + boardY;
					board[cell.X, cell.Y] = new Block(cell.BlockType, blockWidth, blockHeight, x, y);
				}

				activeCells = newCells;
			}
		}
	}
}
