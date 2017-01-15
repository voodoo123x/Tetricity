using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tetricity
{
	public class Block : BaseSprite, IBlock
	{
		BlockType _BlockType;

		public Block(BlockType blockType, int width, int height, int xLocation, int yLocation)
		{
			_BlockType = blockType;
			this.height = height;
			this.width = width;
			this.xLocation = xLocation;
			this.yLocation = yLocation;
			texture = new Texture2D(TetricityGame.Graphics.GraphicsDevice, this.width, this.height);
			Color[] blockColor = new Color[this.width * this.height];

			switch (_BlockType)
			{
				case BlockType.Blue:
					texture = TetricityGame.TextureBlueBlock;
					break;

				case BlockType.Green:
					texture = TetricityGame.TextureGreenBlock;
					break;
					
				case BlockType.Purple:
					texture = TetricityGame.TexturePurpleBlock;
					break;

				case BlockType.Red:
					texture = TetricityGame.TextureRedBlock;
					break;

				case BlockType.Yellow:
					texture = TetricityGame.TextureYellowBlock;
					break;

				case BlockType.Empty:
					for (int i = 0; i < blockColor.Length; i++)
					{
						blockColor[i] = Color.Transparent;
					}
					texture.SetData(blockColor);
					break;
					
				case BlockType.Wall:
					texture = TetricityGame.TextureGreyBlock;
					break;
			}
		}

		public override void Update(Keys[] keysPressed, GameTime gameTime)
		{
			// No updating needed for blocks
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			if (_BlockType == BlockType.Empty)
				spriteBatch.Draw(texture, new Vector2(xLocation, yLocation));
			else
				spriteBatch.Draw(texture, new Vector2(xLocation + 2, yLocation + 2), null, Color.White, 0f, Vector2.Zero, 2.2f, SpriteEffects.None, 0f);
		}

		public BlockType GetBlockType()
		{
			return _BlockType;
		}
	}
}
