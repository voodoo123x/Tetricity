using Microsoft.Xna.Framework.Graphics;

namespace Tetricity
{
	public interface IBlock
	{
		void Draw(SpriteBatch spriteBatch);

		BlockType GetBlockType();
	}
}
