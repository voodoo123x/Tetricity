using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tetricity
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class TetricityGame : Game
	{
		public static GraphicsDeviceManager Graphics;
		public static SpriteFont FontInterface;
		public static SpriteFont FontPaused;
		public static Texture2D TextureBlueBlock;
		public static Texture2D TextureGreenBlock;
		public static Texture2D TextureGreyBlock;
		public static Texture2D TexturePurpleBlock;
		public static Texture2D TextureRedBlock;
		public static Texture2D TextureYellowBlock;

		SpriteBatch _SpriteBatch;
		
		public TetricityGame()
		{
			Graphics = new GraphicsDeviceManager(this);
			Graphics.PreferredBackBufferWidth = 1600;
			Graphics.PreferredBackBufferHeight = 1500;
			Graphics.ApplyChanges();

			Content.RootDirectory = "Content";
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			// TODO: Add your initialization logic here
			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			_SpriteBatch = new SpriteBatch(GraphicsDevice);

			//TODO: use this.Content to load your game content here 
			FontInterface = Content.Load<SpriteFont>("FontInterface");
			FontPaused = Content.Load<SpriteFont>("FontPaused");
			TextureBlueBlock = Content.Load<Texture2D>("Blocks/element_blue_square");
			TextureGreenBlock = Content.Load<Texture2D>("Blocks/element_green_square");
			TextureGreyBlock = Content.Load<Texture2D>("Blocks/element_grey_square");
			TexturePurpleBlock = Content.Load<Texture2D>("Blocks/element_purple_square");
			TextureRedBlock = Content.Load<Texture2D>("Blocks/element_red_square");
			TextureYellowBlock = Content.Load<Texture2D>("Blocks/element_yellow_square");
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			// For Mobile devices, this logic will close the Game when the Back button is pressed
			// Exit() is obsolete on iOS
#if !__IOS__ && !__TVOS__
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();
#endif

			// TODO: Add your update logic here
			GameBoard.Instance.Update(Keyboard.GetState().GetPressedKeys(), gameTime);

			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			var backgroundColor = new Color(72, 74, 76);
			Graphics.GraphicsDevice.Clear(backgroundColor);

			_SpriteBatch.Begin();

			//TODO: Add your drawing code here
			GameBoard.Instance.Draw(_SpriteBatch);

			_SpriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
