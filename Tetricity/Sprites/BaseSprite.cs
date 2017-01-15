using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tetricity
{
	public abstract class BaseSprite : ISprite
	{
		protected int xLocation;
		protected int yLocation;
		protected int width;
		protected int height;
		protected Texture2D texture;

		public abstract void Update(Keys[] keysPressed, GameTime gameTime);

		public virtual void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(texture, new Vector2(xLocation, yLocation));
		}
	}
}
