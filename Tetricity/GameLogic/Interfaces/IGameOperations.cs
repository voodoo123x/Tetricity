using Microsoft.Xna.Framework;

namespace Tetricity
{
	public interface IGameOperations
	{
		bool TickCycleCompleted(GameTime gameTime, float timeLastTick, float tickCoolDown);
	}
}
