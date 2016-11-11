using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Project1
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        GameObject Hero;
        GameObject Enemy;
        GameObject Projectile;
        Rectangle fenetre;
        Texture2D Background;
        Random rng = new Random();
        int rotate = 0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
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
            this.graphics.PreferredBackBufferHeight = graphics.GraphicsDevice.DisplayMode.Height;
            this.graphics.PreferredBackBufferWidth = graphics.GraphicsDevice.DisplayMode.Width;
            this.graphics.ToggleFullScreen();
            fenetre = new Rectangle(0, 0, graphics.GraphicsDevice.DisplayMode.Width, graphics.GraphicsDevice.DisplayMode.Height);
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
            //background
            Background = Content.Load<Texture2D>("Background.jpg");
            //hero
            Hero = new GameObject();
            Hero.estVivant = true;
            Hero.position.X = 830;
            Hero.position.Y = 850;
            Hero.sprite = Content.Load<Texture2D>("Hero.png");
            //enemy
            Enemy = new GameObject();
            Enemy.estVivant = true;
            Enemy.position.X = fenetre.Center.X-100;
            Enemy.position.Y = fenetre.Top-10;
            Enemy.sprite = Content.Load<Texture2D>("Enemy.png");
            Enemy.vitesse.X = 5;
            //projectile
            Projectile = new GameObject();
            Projectile.estVivant = false;
            Projectile.sprite = Content.Load<Texture2D>("Projectile.png");
            Projectile.vitesse.Y = 15;
            
            
            

            // TODO: use this.Content to load your game content here
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
            //déplacement
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            { 
                Hero.vitesse.X -= 1;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {              
                Hero.vitesse.X += 1;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                Hero.vitesse.Y -= 1;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                Hero.vitesse.Y += 1;
            }
            //limite de map
            if (Hero.position.X+225 >= fenetre.Right)
            {
                Hero.vitesse.X = 0;
                Hero.position.X = fenetre.Right-250;               
            }
            if (Hero.position.X+80 <= fenetre.Left)
            {
                Hero.vitesse.X = 0;
                Hero.position.X = fenetre.Left-30;
            }
            if (Hero.position.Y+200 >= fenetre.Bottom)
            {
                Hero.vitesse.Y = 0;
                Hero.position.Y = fenetre.Bottom - 201;
            }
            if (Hero.position.Y+80 <= fenetre.Top)
            {
                Hero.vitesse.Y = 0;
                Hero.position.Y = fenetre.Top - 65;
            }
            //limite de map enemy et mouvement
            if (Enemy.position.X+150 >= fenetre.Right)
            {
                Enemy.vitesse.X -= 7;
            }
            if (Enemy.position.X-5 <= fenetre.Left)
            {
                Enemy.vitesse.X += 7;
            }
            //attaque de l'enemy
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                Projectile.estVivant = true;
                Projectile.position.X = Enemy.position.X;
                Projectile.position.Y = Enemy.position.Y + 100;
            }
            rotate += 1;
            
                // TODO: Add your update logic here
                UpdateHero();
            UpdateEnemy();
            UpdateProjectile();

            base.Update(gameTime);
        }
        public void UpdateHero()
        {
            Hero.position += Hero.vitesse;
        }
        public void UpdateEnemy()
        {
            Enemy.position += Enemy.vitesse;
        }
        public void UpdateProjectile()
        {
            Projectile.position += Projectile.vitesse;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            spriteBatch.Draw(Background, new Rectangle(0, 0, graphics.GraphicsDevice.DisplayMode.Width, graphics.GraphicsDevice.DisplayMode.Height), Color.White);

            spriteBatch.Draw(Hero.sprite, Hero.position, Color.White);
            if (Hero.estVivant == false)
            {
                spriteBatch.Draw(Hero.sprite, Hero.position, Color.Red);
            }
            spriteBatch.Draw(Enemy.sprite, Enemy.position, Color.White);

            if (Projectile.estVivant == true)
            {
                spriteBatch.Draw(Projectile.sprite, Projectile.position, rotation:rotate/7);
            }





            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
