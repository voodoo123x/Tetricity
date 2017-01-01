using System;
using Microsoft.Xna.Framework;

namespace Tetricity
{
	public class GameOperations : IGameOperations
	{
		static GameOperations _Instance;

		public static GameOperations Instance
		{
			get 
			{
				if (_Instance == null)
				{
					_Instance = new GameOperations();
				}

				return _Instance;
			}
		}

		public bool TickCycleCompleted(GameTime gameTime, float timeLastTick, float tickCoolDown)
		{
			bool tickCompleted = false;
			float totalGameTime = (float)gameTime.TotalGameTime.TotalMilliseconds;
			float nextTick = timeLastTick + tickCoolDown;

			if (timeLastTick <= 0 || nextTick < totalGameTime)
			{
				tickCompleted = true;
			}

			return tickCompleted;
		}
	}
}
