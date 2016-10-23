﻿using GenericList;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System.Linq;

namespace Pong
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferHeight = 900,
                PreferredBackBufferWidth = 500
            };
            Content.RootDirectory = "Content";
        }

        public List<Wall> Walls { get; set; }
        public List<Wall> Goals { get; set; }


        /// <summary >
        /// Bottom paddle object
        /// </ summary >
        public Paddle PaddleBottom { get; private set; }
        /// <summary >
        /// Top paddle object
        /// </ summary >
        public Paddle PaddleTop { get; private set; }
        /// <summary >
        /// Ball object
        /// </ summary >
        public Ball Ball { get; private set; }
        /// <summary >
        /// Background image
        /// </ summary >
        public Background Background { get; private set; }
        /// <summary >
        /// Sound when ball hits an obstacle .
        /// SoundEffect is a type defined in Monogame framework
        /// </ summary >
        public SoundEffect HitSound { get; private set; }
        /// <summary >
        /// Background music . Song is a type defined in Monogame framework
        /// </ summary >
        public Song Music { get; private set; }
        /// <summary >
        /// Generic list that holds Sprites that should be drawn on screen
        /// </ summary >
        private IGenericList<Sprite> SpritesForDrawList = new GenericList<Sprite>();
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Screen bounds details
            var screenBounds = GraphicsDevice.Viewport.Bounds;

            PaddleBottom = new Paddle(GameConstants.PaddleDefaultWidth,
            GameConstants.PaddleDefaulHeight, GameConstants.PaddleDefaulSpeed);
            PaddleBottom.X = 150;
            PaddleBottom.Y = 880;

            PaddleTop = new Paddle(GameConstants.PaddleDefaultWidth,
            GameConstants.PaddleDefaulHeight, GameConstants.PaddleDefaulSpeed);
            PaddleTop.X = 150;
            PaddleTop.Y = 0;

            Ball = new Ball(GameConstants.DefaultBallSize,
            GameConstants.DefaultInitialBallSpeed,
            GameConstants.DefaultBallBumpSpeedIncreaseFactor)
            {
                X = 230,
                Y = 430
            };

            Background = new Background(500 , 900);

            Walls = new List<Wall>()
            {
                new Wall (-GameConstants.WallDefaultSize, 0,
                GameConstants.WallDefaultSize, screenBounds.Height ),
                new Wall (screenBounds.Right ,0 , GameConstants.WallDefaultSize,
                screenBounds.Height),
            };

            Goals = new List<Wall>()
            {
                new Wall (0 , screenBounds.Height , screenBounds.Width,
                GameConstants.WallDefaultSize) ,
                new Wall (screenBounds.Top, -GameConstants.WallDefaultSize,
                screenBounds.Width, GameConstants.WallDefaultSize),
            };

            SpritesForDrawList.Add(Background);
            SpritesForDrawList.Add(PaddleBottom);
            SpritesForDrawList.Add(PaddleTop);
            SpritesForDrawList.Add(Ball);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Initialize new SpriteBatch object which will be used to draw textures .
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            // Set textures
            Texture2D paddleTexture = Content.Load<Texture2D>("paddle");
            PaddleBottom.Texture = paddleTexture;
            PaddleTop.Texture = paddleTexture;
            Ball.Texture = Content.Load<Texture2D>("ball");
            Background.Texture = Content.Load<Texture2D>("background");
            
            // Load sounds
            // Start background music
            HitSound = Content.Load<SoundEffect>("hit");
            //Music = Content.Load<Song>("music");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(Music);
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

            var touchState = Keyboard.GetState();
            // 1st player
            if (touchState.IsKeyDown(Keys.Left))
            {
                PaddleBottom.X = PaddleBottom.X - (float)(PaddleBottom.Speed *
                gameTime.ElapsedGameTime.TotalMilliseconds);
            }
            if (touchState.IsKeyDown(Keys.Right))
            {
                PaddleBottom.X = PaddleBottom.X + (float)(PaddleBottom.Speed *
                gameTime.ElapsedGameTime.TotalMilliseconds);
            }
            // 2nd player
            if (touchState.IsKeyDown(Keys.A))
            {
                PaddleTop.X = PaddleTop.X - (float)(PaddleTop.Speed *
                gameTime.ElapsedGameTime.TotalMilliseconds);
            }
            if (touchState.IsKeyDown(Keys.D))
            {
                PaddleTop.X = PaddleTop.X + (float)(PaddleTop.Speed *
                gameTime.ElapsedGameTime.TotalMilliseconds);
            }

            // check if coordinates fit right into the game screen
            PaddleBottom.X = MathHelper.Clamp(PaddleBottom.X, 0, 500 -
            PaddleBottom.Width);

            PaddleTop.X = MathHelper.Clamp(PaddleTop.X, 0, 500 -
            PaddleTop.Width);

            // ball movement
            var ballPositionChange = Ball.Direction *
            (float)(gameTime.ElapsedGameTime.TotalMilliseconds * Ball.Speed);
            Ball.X += ballPositionChange.X;
            Ball.Y += ballPositionChange.Y;

            // Ball - side walls
            if (Walls.Any(w => CollisionDetector.Overlaps(Ball, w)))
            {
                Ball.Direction = new Vector2(-Ball.Direction.X, Ball.Direction.Y);
                SetNewBallSpeed();
            }
            // Ball - winning walls
            if (Goals.Any(w => CollisionDetector.Overlaps(Ball, w)))
            {
                Ball.X = 230;
                Ball.Y = 430;
                Ball.Speed = GameConstants.DefaultInitialBallSpeed;
                HitSound.Play();
            }
            // Paddle - ball collision
            if (CollisionDetector.Overlaps(Ball, PaddleTop) && Ball.Direction.Y < 0
            || (CollisionDetector.Overlaps(Ball, PaddleBottom) && Ball.Direction.Y > 0))
            {
                Ball.Direction = new Vector2(Ball.Direction.X, -Ball.Direction.Y);
                SetNewBallSpeed();                
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(Color.CornflowerBlue);

            // Start drawing .
            spriteBatch.Begin();
            for (int i = 0; i < SpritesForDrawList.Count; i++)
            {
                SpritesForDrawList.GetElement(i).DrawSpriteOnScreen(spriteBatch);
            }

            // End drawing .
            // Send all gathered details to the graphic card in one batch .
            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void SetNewBallSpeed()
        {
            Ball.Speed = MathHelper.Clamp(Ball.Speed * Ball.BumpSpeedIncreaseFactor, 
                GameConstants.DefaultInitialBallSpeed, 
                GameConstants.MaxBallSpeed);
        }
    }
}
