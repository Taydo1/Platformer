using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Platformer.Core;

using System.Collections.Generic;

namespace Platformer
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        public const int WINDOW_WIDTH = 1080;
        public const int WINDOW_HEIGHT = 720;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D playerTexture;
        Texture2D blocTexture;

        List<GameObject> solidBlocList;

        Player player;        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
            graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
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
            player = new Player(300, 100, 60, 100);

            solidBlocList = new List<GameObject>();
            solidBlocList.Add(new GameObject(128, 256));
            solidBlocList.Add(new GameObject(256, 384));
            solidBlocList.Add(new GameObject(384, 384));
            base.Initialize();

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            playerTexture = Content.Load<Texture2D>("images/perso");
            player.Texture = playerTexture;

            blocTexture = Content.Load<Texture2D>("images/bloc");

            for (int i = 0; i < solidBlocList.Count; i++)
            {
                solidBlocList[i].Texture = blocTexture;
            }
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            player.DetectMove(Keyboard.GetState());
            player.Update(gameTime, solidBlocList);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            player.Draw(spriteBatch);
            for(int i = 0; i < solidBlocList.Count; i++)
            {
                solidBlocList[i].Draw(spriteBatch);
            }
            spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
