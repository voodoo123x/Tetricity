using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tetricity
{
	public interface ISprite
	{
		void Update(Keys[] keysPressed, GameTime gameTime);

		void Draw(SpriteBatch spriteBatch);
	}
}
