using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Platformer.Core;

using System.Collections.Generic;
using System;

namespace Platformer
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D playerTexture;
        Texture2D blockTexture;
        Texture2D skyTexture;
        static Texture2D lineTexture;

        Vector2 mapSize = Vector2.Zero;
        Vector2 shift;

        List<GameObject> map;

        Player player;        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";


            graphics.PreferredBackBufferWidth = Constants.WindowWidth;
            graphics.PreferredBackBufferHeight = Constants.WindowHeight;

            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += new EventHandler<EventArgs>(this.ResizeWindow);

            /*graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            graphics.IsFullScreen = true;*/
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

            shift = Vector2.Zero;

            LoadLevel(1);

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
            lineTexture = Content.Load<Texture2D>("images/line");
            blockTexture = Content.Load<Texture2D>("images/block");
            skyTexture = Content.Load<Texture2D>("images/sky");

            player.Texture = playerTexture;
            for (int i = 0; i < map.Count; i++)
            {
                if(map[i] is Bloc) { map[i].Texture = blockTexture; }
                else if(map[i] is Sky) { map[i].Texture = skyTexture; }
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
            player.Update(gameTime, map, ref shift);

            if (shift.Y < Constants.WindowVertTileNum - mapSize.Y)
            {
                shift.Y = Constants.WindowVertTileNum - mapSize.Y;
            }


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
            for(int i = 0; i < map.Count; i++)
            {
                map[i].Draw(spriteBatch, shift);
            }
            player.Draw(spriteBatch, shift);
            spriteBatch.End();


            base.Draw(gameTime);
        }

        public static void DrawLine(SpriteBatch sb, Vector2 start, Vector2 end)
        {

            Vector2 edge = end - start;
            // calculate angle to rotate line
            float angle =
                (float)Math.Atan2(edge.Y, edge.X);


            sb.Draw(lineTexture,
                new Rectangle(// rectangle defines shape of line and position of start of line
                    (int)start.X,
                    (int)start.Y,
                    (int)edge.Length(), //sb will strech the texture to fill this rectangle
                    1), //width of line, change this to make thicker line
                null,
                Color.Red, //colour of line
                angle,     //angle of line (calulated above)
                new Vector2(0, 0), // point in line about which to rotate
                SpriteEffects.None,
                0);

        }

        public void LoadLevel(int levelNumber)
        {
            mapSize = Vector2.Zero;

            map = new List<GameObject>();
            string levelFile = System.IO.File.ReadAllText("Content/levels/level"+ levelNumber+".txt");

            string[] levelTemp = levelFile.Split('\n');
            string[][] level = new string[levelTemp.Length][];
            for (int i = 0; i < levelTemp.Length; i++)
            {
                level[i] = levelTemp[i].Split();
                for (int j = 0; j < level[i].Length; j++)
                {

                    switch (level[i][j])
                    {
                        case "-1":
                            player = new Player(j, i, 60, 100);
                            map.Add(new Sky(j, i));
                            break;
                        case "0":
                            map.Add(new Sky(j, i));
                            break;
                        case "1":
                            map.Add(new Bloc(j, i));
                            break;
                    }
                    System.Console.Write(level[i][j] + "  ");
                }

                mapSize.X = Math.Max(mapSize.X, levelTemp[i].Length);
                System.Console.WriteLine();

            }
            mapSize.Y = levelTemp.Length;
        }

        public void ResizeWindow(object sender, EventArgs e)
        {
            Constants.WindowWidth = GraphicsDevice.Viewport.Width;
            Constants.WindowHeight = GraphicsDevice.Viewport.Height;
            Constants.WindowHoriTileNum = (float)Constants.WindowWidth / Constants.TileSize;
            Constants.WindowVertTileNum = (float)Constants.WindowHeight / Constants.TileSize;
            Player.UpdateSrollBoxes();
        }
    }
}
