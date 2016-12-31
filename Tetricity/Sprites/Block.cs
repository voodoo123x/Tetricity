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
			_Texture = new Texture2D(TetricityGame.graphics.GraphicsDevice, _Width, _Height);
			Color[] blockColor = new Color[_Width * _Height];

			switch (_BlockType)
			{
				case BlockType.Blue:
					for (int i = 0; i < blockColor.Length; i++)
					{
						blockColor[i] = Color.Blue;
					}
					break;

				case BlockType.Green:
					for (int i = 0; i < blockColor.Length; i++)
					{
						blockColor[i] = Color.Green;
					}
					break;
					
				case BlockType.Purple:
					for (int i = 0; i < blockColor.Length; i++)
					{
						blockColor[i] = Color.Purple;
					}
					break;

				case BlockType.Red:
					for (int i = 0; i < blockColor.Length; i++)
					{
						blockColor[i] = Color.Red;
					}
					break;

				case BlockType.Empty:
					for (int i = 0; i < blockColor.Length; i++)
					{
						blockColor[i] = Color.Transparent;
					}
					break;
					
				case BlockType.Wall:
					for (int i = 0; i < blockColor.Length; i++)
					{
						blockColor[i] = Color.WhiteSmoke;
					}
					break;
			}

			_Texture.SetData(blockColor);
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(_Texture, new Vector2(_XLocation, _YLocation));
		}

		public BlockType GetBlockType()
		{
			return _BlockType;
		}
	}
}
