using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tetricity
{
	public class GameBoard
	{
		static GameBoard _Instance;
		static readonly int _BoardHeight = 20;
		static readonly int _BoardWidth = 12;
		readonly int _BlockWidth = 75;
		readonly int _BlockHeight = 75;
		readonly int _XLocation = 0;
		readonly int _YLocation = 0;
		int _score;
		Difficulty _Difficulty = Difficulty.Easy;
		IBlock[,] _Board = new Block[_BoardWidth, _BoardHeight];
		IList<CellEntity> _ActiveBlocks = new List<CellEntity>();
		float _TimeLastTick;
		float _TickCoolDown = 1000;

		GameBoard()
		{
			Reset();
	    }  

		public static GameBoard Instance
		{
			get
			{
				if (_Instance == null)
				{
					_Instance = new GameBoard();
				}

				return _Instance;
			}
		}

		public void Update(Keys[] keysPressed, GameTime gameTime)
		{
			if (_ActiveBlocks.Count <= 0)
			{
				_ActiveBlocks = GenerateNewFormation();
				_TimeLastTick = (float)gameTime.TotalGameTime.TotalMilliseconds;
			}

			if (TickCycleCompleted(gameTime))
			{
				if (keysPressed.Length <= 0 || keysPressed.Contains(Keys.Down))
				{
					if (CanPerformMove(_ActiveBlocks, MoveType.Down))
					{
						IEnumerable<CellEntity> temp = _ActiveBlocks.OrderByDescending(b => b.Y);
						IList<CellEntity> newActiveBlocks = new List<CellEntity>();

						foreach (var block in temp)
						{
							int newX = block.X;
							int newY = block.Y + 1;

							PerformMove(block.BlockType, block.X, block.Y, newX, newY);
							newActiveBlocks.Add(new CellEntity { BlockType = block.BlockType, X = newX, Y = newY });
						}

						_ActiveBlocks = newActiveBlocks;
					}
					else
					{
						_ActiveBlocks.Clear();
					}
				}
				else if (keysPressed.Contains(Keys.Left))
				{
					if (CanPerformMove(_ActiveBlocks, MoveType.Right))
					{
						IEnumerable<CellEntity> temp = _ActiveBlocks.OrderByDescending(b => b.Y);
						IList<CellEntity> newActiveBlocks = new List<CellEntity>();

						foreach (var block in temp)
						{
							int newX = block.X - 1;
							int newY = block.Y;

							PerformMove(block.BlockType, block.X, block.Y, newX, newY);
							newActiveBlocks.Add(new CellEntity { BlockType = block.BlockType, X = newX, Y = newY });
						}

						_ActiveBlocks = newActiveBlocks;
					}
					else
					{
						_ActiveBlocks.Clear();
					}
				}
				else if (keysPressed.Contains(Keys.Right))
				{
					if (CanPerformMove(_ActiveBlocks, MoveType.Right))
					{
						IEnumerable<CellEntity> temp = _ActiveBlocks.OrderByDescending(b => b.Y);
						IList<CellEntity> newActiveBlocks = new List<CellEntity>();

						foreach (var block in temp)
						{
							int newX = block.X + 1;
							int newY = block.Y;

							PerformMove(block.BlockType, block.X, block.Y, newX, newY);
							newActiveBlocks.Add(new CellEntity { BlockType = block.BlockType, X = newX, Y = newY });
						}

						_ActiveBlocks = newActiveBlocks;
					}
					else
					{
						_ActiveBlocks.Clear();
					}
				}
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			for (int i = 0; i < _BoardWidth; i++)
			{
				for (int j = 0; j < _BoardHeight; j++)
				{
					_Board[i, j].Draw(spriteBatch);
				}
			}
		}

		void Reset()
		{
			for (int i = 0; i < _BoardWidth; i++)
			{
				for (int j = 0; j < _BoardHeight; j++)
				{
					BlockType blockType = BlockType.Empty;

					if (i == 0 || i == _BoardWidth - 1 || j == _BoardHeight - 1)
					{
						blockType = BlockType.Wall;
					}

					int x = (i * _BlockWidth) + _XLocation;
					int y = (j * _BlockHeight) + _YLocation;
					_Board[i, j] = new Block(blockType, _BlockHeight, _BlockWidth, x, y);
				}
			}

			_score = 0;
		}

		IList<CellEntity> GenerateNewFormation()
		{
			IList<CellEntity> newFormation = new List<CellEntity>();
			Random random = new Random();
			BlockType blockType = BlockType.Empty;

			switch (random.Next(0, 4))
			{
				case 0:	// Line
					blockType = BlockType.Blue;
					newFormation.Add(new CellEntity { BlockType = blockType, X = 6, Y = 0 });
					newFormation.Add(new CellEntity { BlockType = blockType, X = 6, Y = 1 });
					newFormation.Add(new CellEntity { BlockType = blockType, X = 6, Y = 2 });
					newFormation.Add(new CellEntity { BlockType = blockType, X = 6, Y = 3 });
					break;
					
				case 1:	// Left L
					blockType = BlockType.Red;
					newFormation.Add(new CellEntity { BlockType = blockType, X = 6, Y = 0 });
					newFormation.Add(new CellEntity { BlockType = blockType, X = 6, Y = 1 });
					newFormation.Add(new CellEntity { BlockType = blockType, X = 6, Y = 2 });
					newFormation.Add(new CellEntity { BlockType = blockType, X = 5, Y = 2 });
					break;
					
				case 2: // Right L
					blockType = BlockType.Purple;
					newFormation.Add(new CellEntity { BlockType = blockType, X = 6, Y = 0 });
					newFormation.Add(new CellEntity { BlockType = blockType, X = 6, Y = 1 });
					newFormation.Add(new CellEntity { BlockType = blockType, X = 6, Y = 2 });
					newFormation.Add(new CellEntity { BlockType = blockType, X = 7, Y = 2 });
					break;	
					
				case 3: // Square
					blockType = BlockType.Green;
					newFormation.Add(new CellEntity { BlockType = blockType, X = 6, Y = 0 });
					newFormation.Add(new CellEntity { BlockType = blockType, X = 6, Y = 1 });
					newFormation.Add(new CellEntity { BlockType = blockType, X = 5, Y = 0 });
					newFormation.Add(new CellEntity { BlockType = blockType, X = 5, Y = 1 });
					break;
			}

			foreach (var block in newFormation)
			{
				_Board[block.X, block.Y] = new Block(blockType, _BlockWidth, _BlockHeight, 6 * _BlockWidth, 0);
			}

			return newFormation;
		}

		bool TickCycleCompleted(GameTime gameTime)
		{
			bool tickCompleted = false;
			float totalGameTime = (float)gameTime.TotalGameTime.TotalMilliseconds;
			float nextTick = _TimeLastTick + _TickCoolDown;

			if (_TimeLastTick <= 0 || nextTick < totalGameTime)
			{
				_TimeLastTick = totalGameTime;
				tickCompleted = true;
			}

			return tickCompleted;
		}

		void PerformMove(BlockType blockType, int oldX, int oldY, int newX, int newY)
		{
			int x = (oldX * _BlockWidth) + _XLocation;
			int y = (oldY * _BlockHeight) + _YLocation;
			_Board[oldX, oldY] = new Block(BlockType.Empty, _BlockWidth, _BlockHeight, x, y);

			x = (newX * _BlockWidth) + _XLocation;
			y = (newY * _BlockHeight) + _YLocation;
			_Board[newX, newY] = new Block(blockType, _BlockWidth, _BlockHeight, x, y);
		}

		bool CanPerformMove(IList<CellEntity> activeCells, MoveType moveType)
		{
			bool canMove = false;

			switch (moveType)
			{
				case MoveType.Down:
					foreach (var cell in activeCells)
					{
						int newX = cell.X;
						int newY = cell.Y + 1;

						if (newX < _BoardWidth && newX >= 0 && newY <= _BoardHeight)
						{
							if (_Board[newX, newY].GetBlockType() == BlockType.Empty 
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
					}
					break;

				case MoveType.Left:
					foreach (var cell in activeCells)
					{
						int newX = cell.X - 1;
						int newY = cell.Y;

						if (newX < _BoardWidth && newX >= 0 && newY <= _BoardHeight)
						{
							if (_Board[newX, newY].GetBlockType() == BlockType.Empty
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
					}
					break;
				
				case MoveType.Right:
					foreach (var cell in activeCells)
					{
						int newX = cell.X + 1;
						int newY = cell.Y;

						if (newX < _BoardWidth && newX >= 0 && newY <= _BoardHeight)
						{
							if (_Board[newX, newY].GetBlockType() == BlockType.Empty
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
					}
					break;
			}

			return canMove;
		}
	}

	public class CellEntity
	{
		public BlockType BlockType { get; set; }
		public int X { get; set; }
		public int Y { get; set; }
	}

	public enum BlockType
	{
		Blue,
		Empty,
		Green,
		Purple,
		Red,
		Yellow,
		Wall
	}

	public enum Difficulty
	{
		Easy,
		Medium,
		Hard
	}

	public enum MoveType
	{
		Down,
		Left,
		Right,
		Up
	}
}
