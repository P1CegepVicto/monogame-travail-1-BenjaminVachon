using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Threading;

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
        GameObject ProjectileH;
        GameObject ProjectileE;
        Texture2D Victory;
        Texture2D Defeat;
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
            //victoire
            Victory = Content.Load<Texture2D>("Victory.png");
            //défaite
            Defeat = Content.Load<Texture2D>("Defeat.png");
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
            Enemy.vitesse.X = 15;
            //projectile1
            ProjectileE = new GameObject();
            ProjectileE.estVivant = false;
            ProjectileE.sprite = Content.Load<Texture2D>("Projectile1.png");
            ProjectileE.vitesse.Y = 15;
            //projectile2
            ProjectileH = new GameObject();
            ProjectileH.estVivant = false;
            ProjectileH.sprite = Content.Load<Texture2D>("Projectile2.png");
            ProjectileH.vitesse.Y = -15;

            
            
            

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
            //Exit
            if (Hero.estVivant == false || Enemy.estVivant == false)
            {
                Thread.Sleep(1500);
                this.Exit();
            }
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
                Enemy.vitesse.X = rng.Next(-20,-10);
            }
            if (Enemy.position.X-5 <= fenetre.Left)
            {
                Enemy.vitesse.X = rng.Next(10,20);
            }
            //attaque de l'enemy
            if (Enemy.estVivant == true && ProjectileE.estVivant == false)
            {
                ProjectileE.estVivant = true;
                ProjectileE.position.X = Enemy.position.X;
                ProjectileE.position.Y = Enemy.position.Y + 100;
            }
            if (ProjectileE.position.Y >= fenetre.Bottom)
            {
                ProjectileE.estVivant = false;
            }
            if (Hero.position.X+50  <= ProjectileE.position.X+100 && Hero.position.X+200 >= ProjectileE.position.X)
            {
                if (Hero.position.Y+200 >= ProjectileE.position.Y && Hero.position.Y-25 <= ProjectileE.position.Y)
                {
                    Hero.estVivant = false;
                }
            }
            //attaque du Hero
            if (Hero.estVivant == true && Keyboard.GetState().IsKeyDown(Keys.Space) && ProjectileH.estVivant == false)
            {
                ProjectileH.estVivant = true;
                ProjectileH.position.X = Hero.position.X+100;
                ProjectileH.position.Y = Hero.position.Y+50;
            }
            if (ProjectileH.position.Y <= fenetre.Top)
            {
                ProjectileH.estVivant = false;
            }
            if (Enemy.position.X <= ProjectileH.position.X + 100 && Enemy.position.X + 225 >= ProjectileH.position.X)
            {
                if (Enemy.position.Y + 200 >= ProjectileH.position.Y && Enemy.position.Y <= ProjectileH.position.Y)
                {
                    Enemy.estVivant = false;
                }
            }
            //colision Hero-Enemy
            if (Hero.position.X + 50 <= Enemy.position.X + 150 && Hero.position.X + 200 >= Enemy.position.X)
            {
                if (Hero.position.Y + 200 >= Enemy.position.Y && Hero.position.Y - 25 <= Enemy.position.Y)
                {
                    Hero.estVivant = false;
                }
            }

            rotate += 1;
            
            // TODO: Add your update logic here
            UpdateHero();
            UpdateEnemy();
            UpdateProjectileE();
            UpdateProjectileH();

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
        public void UpdateProjectileE()
        {
            ProjectileE.position += ProjectileE.vitesse;
        }
        public void UpdateProjectileH()
        {
            ProjectileH.position += ProjectileH.vitesse;
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
            if (Enemy.estVivant == false)
            {
                spriteBatch.Draw(Enemy.sprite, Enemy.position, Color.Red);
            }
            if (ProjectileE.estVivant == true)
            {
                spriteBatch.Draw(ProjectileE.sprite, ProjectileE.position, rotation: rotate / 7);
            }
            if (ProjectileH.estVivant == true)
            {
                spriteBatch.Draw(ProjectileH.sprite, ProjectileH.position, rotation: rotate / 7
                    );
            }

            if (Hero.estVivant == true && Enemy.estVivant == false)
            {
                spriteBatch.Draw(Victory, new Rectangle(0, 0, graphics.GraphicsDevice.DisplayMode.Width, graphics.GraphicsDevice.DisplayMode.Height), Color.White);
            }
            if (Hero.estVivant == false && Enemy.estVivant == true)
            {
                spriteBatch.Draw(Defeat, new Rectangle(0, 0, graphics.GraphicsDevice.DisplayMode.Width, graphics.GraphicsDevice.DisplayMode.Height), Color.White);
            }


            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
