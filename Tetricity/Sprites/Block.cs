using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tetricity
{
	public class Block : IBlock
	{
		BlockType _BlockType;
		Texture2D _Texture;
		int _Height;
		int _Width;
		int _XLocation;
		int _YLocation;

		public Block(BlockType blockType, int width, int height, int xLocation, int yLocation)
		{
			_BlockType = blockType;
			_Height = height;
			_Width = width;
			_XLocation = xLocation;
			_YLocation = yLocation;
			_Texture = new Texture2D(TetricityGame.Graphics.GraphicsDevice, _Width, _Height);
			Color[] blockColor = new Color[_Width * _Height];

			switch (_BlockType)
			{
				case BlockType.Blue:
					_Texture = TetricityGame.TextureBlueBlock;
					break;

				case BlockType.Green:
					_Texture = TetricityGame.TextureGreenBlock;
					break;
					
				case BlockType.Purple:
					_Texture = TetricityGame.TexturePurpleBlock;
					break;

				case BlockType.Red:
					_Texture = TetricityGame.TextureRedBlock;
					break;

				case BlockType.Empty:
					for (int i = 0; i < blockColor.Length; i++)
					{
						blockColor[i] = Color.Transparent;
					}
					_Texture.SetData(blockColor);
					break;
					
				case BlockType.Wall:
					_Texture = TetricityGame.TextureGreyBlock;
					break;
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			if (_BlockType == BlockType.Empty)
				spriteBatch.Draw(_Texture, new Vector2(_XLocation, _YLocation));
			else
				spriteBatch.Draw(_Texture, new Vector2(_XLocation + 2, _YLocation + 2), null, Color.White, 0f, Vector2.Zero, 2.2f, SpriteEffects.None, 0f);
		}

		public BlockType GetBlockType()
		{
			return _BlockType;
		}
	}
}
