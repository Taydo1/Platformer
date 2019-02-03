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
        KeyboardState previousState;

        Texture2D[] playerTexture = new Texture2D[13];
        Texture2D blockTexture;
        Texture2D skyTexture;
        Texture2D trampolineTexture;
        Texture2D iceTexture;
        Texture2D spikeTexture;
        Texture2D shotTexture;
        Texture2D checkpointTexture;
        Texture2D monsterTexture;
        static Texture2D lineTexture;

        static public Vector2 mapSize = Vector2.Zero;
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
            previousState = Keyboard.GetState();
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

            for(int i = 0; i < playerTexture.Length; i++)
            {
                playerTexture[i] = Content.Load<Texture2D>("images/perso"+CompleteNumber(i, 2));
            }
            lineTexture = Content.Load<Texture2D>("images/line");
            blockTexture = Content.Load<Texture2D>("images/block");
            skyTexture = Content.Load<Texture2D>("images/sky");
            trampolineTexture = Content.Load<Texture2D>("images/trampoline");
            iceTexture = Content.Load<Texture2D>("images/ice");
            spikeTexture = Content.Load<Texture2D>("images/spike");
            shotTexture = Content.Load<Texture2D>("images/shot");
            checkpointTexture = Content.Load<Texture2D>("images/checkpoint");
            monsterTexture = Content.Load<Texture2D>("images/monster0");

            player.Texture = playerTexture;
            for (int i = 0; i < map.Count; i++)
            {
                if(map[i] is Trampoline) { map[i].Texture = new[] { trampolineTexture }; }
                else if(map[i] is Ice) { map[i].Texture = new[] { iceTexture }; }
                else if(map[i] is Spike) { map[i].Texture = new[] { spikeTexture }; }
                else if(map[i] is Block) { map[i].Texture = new[] { blockTexture }; }
                else if(map[i] is Shot) { map[i].Texture = new[] { iceTexture }; }
                else if(map[i] is Sky) { map[i].Texture = new[] { skyTexture }; }
                else if(map[i] is Checkpoint) { map[i].Texture = new[] { checkpointTexture }; }
                else if(map[i] is Monster) { map[i].Texture = new[] { monsterTexture }; }

                if (map[i].NeedBackground)
                {
                    map[i].BackgroundTexture = skyTexture;
                }
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

            KeyboardState state = Keyboard.GetState();
            player.Update(gameTime, map, shift);
            player.DetectMove(state, GamePad.GetState(PlayerIndex.One), map, gameTime, shotTexture);
            player.UpdateShift(ref shift);

            //Console.WriteLine(player.IsAlive);


            if (shift.Y < Constants.WindowVertTileNum - mapSize.Y)
            {
                shift.Y = Constants.WindowVertTileNum - mapSize.Y;
            }

            for(int i = 0; i < map.Count; i++)
            {
                map[i].Update(gameTime, map, shift);
            }

            previousState = state;

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
                if(!(map[i] is MobileGameObject))map[i].Draw(spriteBatch, shift);
            }
            for(int i = 0; i < map.Count; i++)
            {
                if((map[i] is MobileGameObject))map[i].Draw(spriteBatch, shift);
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

        public string CompleteNumber(int number, int digitNumber)
        {
            string result = number.ToString();
            while(result.Length< digitNumber)
            {
                result = "0" + result;
            }
            return result;
        }

        public void LoadLevel(int levelNumber)
        {
            mapSize = Vector2.Zero;

            map = new List<GameObject>();
            string levelFile = System.IO.File.ReadAllText("Content/levels/level"+ levelNumber+".txt");
            levelFile = levelFile.Replace("  ", " ");
            levelFile = levelFile.Replace("  ", " ");

            string[] levelTemp = levelFile.Split('\n');
            string[][] level = new string[levelTemp.Length][];
            for (int i = 0; i < levelTemp.Length; i++)
            {
                level[i] = levelTemp[i].Split();
                for (int j = 0; j < level[i].Length; j++)
                {
                    int tile;
                    if(!Int32.TryParse(level[i][j], out tile)){
                        continue;
                    }
                    switch (tile)
                    {
                        case -1:
                            player = new Player(j+0.0001f, i);
                            map.Add(new Sky(j, i));
                            break;
                        case 0:
                            map.Add(new Sky(j, i));
                            break;
                        case 1:
                            map.Add(new Block(j, i, false));
                            break;
                        case 2:
                            map.Add(new Trampoline(j, i));
                            break;
                        case 3:
                            map.Add(new Ice(j, i));
                            break;
                        case 10:
                            map.Add(new Checkpoint(j, i));
                            break;
                        case 100:
                        case 101:
                        case 102:
                        case 103:
                            map.Add(new Spike(j, i,tile-100));
                            break;
                        case 200:
                            map.Add(new Monster(j + 0.0001f, i));
                            map.Add(new Sky(j, i));
                            break;
                    }
                    System.Console.Write(CompleteNumber(tile, 3) + "  ");
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

            Player.UpdateScrollBoxes();
            GameObject.UpdateScreen();
        }
    }
}
