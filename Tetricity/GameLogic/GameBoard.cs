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
		int _Score;
		IBlock[,] _Board = new Block[_BoardWidth, _BoardHeight];
		IList<CellEntity> _ActiveBlocks = new List<CellEntity>();
		float _TimeLastTick;
		float _TickCoolDown = 1000;
		float _TimeLastMove;
		float _MoveCoolDown = 100;
		float _TimeLastRotation;
		float _RotationCoolDown = 200;
		int _RowsCompleted = 0;
		bool _GamePaused = false;
		bool _GameOver = false;

		GameBoard()
		{
			BoardOperations.Instance.Reset(ref _Board, _BoardWidth, _BoardHeight, _BlockWidth, _BlockHeight, _XLocation, _YLocation, out _Score, out _RowsCompleted);
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
			bool moveTickCompleted = GameOperations.Instance.TickCycleCompleted(gameTime, _TimeLastMove, _MoveCoolDown);
			bool downTickCompleted = GameOperations.Instance.TickCycleCompleted(gameTime, _TimeLastTick, _TickCoolDown);
			bool rotationTickCompleted = GameOperations.Instance.TickCycleCompleted(gameTime, _TimeLastRotation, _RotationCoolDown);
			float totalTime = (float)gameTime.TotalGameTime.TotalMilliseconds;

			if (keysPressed.Contains(Keys.Space) && moveTickCompleted && !_GameOver)
			{
				_TimeLastMove = totalTime;
				_GamePaused = _GamePaused == false;
			}

			if (keysPressed.Contains(Keys.Enter) && _GameOver)
			{
				BoardOperations.Instance.Reset(ref _Board, _BoardWidth, _BoardHeight, _BlockWidth, _BlockHeight, _XLocation, _YLocation, out _Score, out _RowsCompleted);
				_GameOver = false;
				return;
			}

			if (_GamePaused || _GameOver)
			{
				return;
			}

			if (_ActiveBlocks.Count <= 0)
				CheckCompletedRows();

			if (_ActiveBlocks.Count <= 0)
			{
				_ActiveBlocks = FormationOperations.Instance.GenerateNewFormation(ref _Board, _BlockWidth, _BlockHeight, _XLocation, _YLocation);
				_ActiveBlocks = _ActiveBlocks.OrderByDescending(b => b.Y).ThenBy(b => b.X).ToList();
				_TimeLastTick = (float)gameTime.TotalGameTime.TotalMilliseconds;

				if (_ActiveBlocks.Count <= 0)
				{
					_GameOver = true;
					return;
				}
			}

			if ((keysPressed.Contains(Keys.Down) && GameOperations.Instance.TickCycleCompleted(gameTime, _TimeLastMove, _MoveCoolDown / 2)) 
			    || downTickCompleted)
			{
				if (moveTickCompleted) { _TimeLastMove = totalTime; }
				if (downTickCompleted) { _TimeLastTick = totalTime; }

				if (FormationOperations.Instance.CanPerformMove(ref _ActiveBlocks, ref _Board, MoveType.Down))
				{
					IEnumerable<CellEntity> temp = _ActiveBlocks;
					IList<CellEntity> newActiveBlocks = new List<CellEntity>();

					foreach (var block in temp)
					{
						int newX = block.X;
						int newY = block.Y + 1;

						FormationOperations.Instance.PerformMove(ref _Board, block.BlockType, block.X, block.Y, newX, newY, _XLocation, _YLocation, _BlockWidth, _BlockHeight);
						newActiveBlocks.Add(new CellEntity { BlockType = block.BlockType, Orientation = block.Orientation, X = newX, Y = newY });
					}

					_ActiveBlocks = newActiveBlocks;
				}
				else
				{
					_ActiveBlocks.Clear();
				}
			}

			if (keysPressed.Contains(Keys.Left) && !keysPressed.Contains(Keys.Up) && moveTickCompleted)
			{
				_TimeLastMove = totalTime;

				if (FormationOperations.Instance.CanPerformMove(ref _ActiveBlocks, ref _Board, MoveType.Left))
				{
					IEnumerable<CellEntity> temp = _ActiveBlocks;
					IList<CellEntity> newActiveBlocks = new List<CellEntity>();

					foreach (var block in temp)
					{
						int newX = block.X - 1;
						int newY = block.Y;

						FormationOperations.Instance.PerformMove(ref _Board, block.BlockType, block.X, block.Y, newX, newY, _XLocation, _YLocation, _BlockWidth, _BlockHeight);
						newActiveBlocks.Add(new CellEntity { BlockType = block.BlockType, Orientation = block.Orientation, X = newX, Y = newY });
					}

					_ActiveBlocks = newActiveBlocks;
				}
			}

			if (keysPressed.Contains(Keys.Right) && !keysPressed.Contains(Keys.Up) && moveTickCompleted)
			{
				_TimeLastMove = totalTime; 

				if (FormationOperations.Instance.CanPerformMove(ref _ActiveBlocks, ref _Board, MoveType.Right))
				{
					IEnumerable<CellEntity> temp = _ActiveBlocks;
					IList<CellEntity> newActiveBlocks = new List<CellEntity>();

					foreach (var block in temp)
					{
						int newX = block.X + 1;
						int newY = block.Y;

						FormationOperations.Instance.PerformMove(ref _Board, block.BlockType, block.X, block.Y, newX, newY, _XLocation, _YLocation, _BlockWidth, _BlockHeight);
						newActiveBlocks.Add(new CellEntity { BlockType = block.BlockType, Orientation = block.Orientation, X = newX, Y = newY });
					}

					_ActiveBlocks = newActiveBlocks;
				}
			}

			if (keysPressed.Contains(Keys.Up) && rotationTickCompleted)
			{
				_TimeLastRotation = totalTime;

				FormationOperations.Instance.RotateFormation(ref _Board, ref _ActiveBlocks, _XLocation, _YLocation, _BlockWidth, _BlockHeight);
			}

			foreach (var block in _ActiveBlocks)
			{
				if (_Board[block.X, block.Y].GetBlockType() != block.BlockType)
				{
					int x = (block.X * _BlockWidth) + _XLocation;
					int y = (block.Y * _BlockHeight) + _YLocation;
					_Board[block.X, block.Y] = new Block(block.BlockType, _BlockWidth, _BlockHeight, x, y);
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

			string displayScore = string.Format("Score: {0}", _Score);
			spriteBatch.DrawString(TetricityGame.FontInterface, displayScore, new Vector2(950, 50), Color.WhiteSmoke);
			string displayRowsCompleted = string.Format("Rows Filled: {0}", _RowsCompleted);
			spriteBatch.DrawString(TetricityGame.FontInterface, displayRowsCompleted, new Vector2(950, 150), Color.WhiteSmoke);

			if (_GamePaused)
			{
				spriteBatch.DrawString(TetricityGame.FontPaused, "** PAUSED **", new Vector2(150, 700), Color.WhiteSmoke);
			}

			if (_GameOver)
			{
				spriteBatch.DrawString(TetricityGame.FontPaused, "GAME OVER!", new Vector2(140, 700), Color.WhiteSmoke);
			}
		}

		// TODO: Move this method to BoardOperations
		void CheckCompletedRows()
		{
			IEnumerable<int> completedRows = BoardOperations.Instance.CheckRowsCompleted(ref _Board);

			if (completedRows.Count() > 0)
			{
				int pointsScored = 0;
				int row = completedRows.First();

				while (row >= 0)
				{
					pointsScored += BoardOperations.Instance.RemoveRow(ref _Board, row, _BlockWidth, _BlockHeight, _XLocation, _YLocation);
					_RowsCompleted++;

					row = BoardOperations.Instance.CheckRowsCompleted(ref _Board).First();
				}

				_Score += pointsScored * completedRows.Count();

				return;
			}
		}
	}
}
