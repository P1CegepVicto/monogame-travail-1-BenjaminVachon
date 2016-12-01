using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace VachonBenjaminJeu3
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        GameObject Vaisseau;
        GameObject[] Enemy = new GameObject[5];
        GameObject Projectile;
        GameObject Background;
        GameObject Background2;
        GameObject Menu;
        Rectangle Window;
        bool Gamestate;
        Random rng = new Random();



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
            Window = new Rectangle(0, 0, graphics.GraphicsDevice.DisplayMode.Width, graphics.GraphicsDevice.DisplayMode.Height);
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
            //Background
            Background = new GameObject();
            Background.sprite = Content.Load<Texture2D>("Galaxy.jpg");
            Background.position.X = 0;
            Background.position.Y = 0;
            Background.vitesse.X = -15;
            Background2 = new GameObject();
            Background2.sprite = Content.Load<Texture2D>("Galaxy.jpg");
            Background2.position.X = 2880;
            Background2.position.Y = 0;
            Background2.vitesse.X = -15;
            //Menu
            Menu = new GameObject();
            Menu.sprite = Content.Load<Texture2D>("Play.png");
            Menu.estVivant = true;
            Menu.position.X = Window.Width / 2 - 310;
            Menu.position.Y = Window.Height / 2 - 300;
            //Enemy
            for (int i = Enemy.Length - 1; i >= 0; i--)
            {
                Enemy[i] = new GameObject();
                Enemy[i].estVivant = true;
                Enemy[i].position.X = Window.Right - 150;
                Enemy[i].position.Y = rng.Next(0, Window.Height - 80);
                Enemy[i].vitesse.X = rng.Next(-16, -11);
                if (rng.Next(0, 2) == 1)
                {
                    Enemy[i].sprite = Content.Load<Texture2D>("Enemy2.png");
                }
                else
                {
                    Enemy[i].sprite = Content.Load<Texture2D>("Enemy1.png");
                }
            }
            //Hero
            Vaisseau = new GameObject();
            Vaisseau.estVivant = true;
            Vaisseau.position.X = Window.Left;
            Vaisseau.position.Y = Window.Center.Y;
            Vaisseau.sprite = Content.Load<Texture2D>("Vaisseau.png");
            //Projectile
            Projectile = new GameObject();
            Projectile.estVivant = false;
            Projectile.sprite = Content.Load<Texture2D>("Projectile.png");
            //son
            //menu
            Gamestate = false;

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
            if (Gamestate == false && Menu.estVivant == true)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    Gamestate = true;
                    Menu.estVivant = false;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    this.Exit();
                }
            }
            if (Gamestate == true)
            {
                //Quitter
                if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    Gamestate = false;
                    Menu.estVivant = true;
                }
                //Movement Vaisseau
                if (Keyboard.GetState().IsKeyDown(Keys.W))
                {
                    Vaisseau.vitesse.Y = -8;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.S))
                {
                    Vaisseau.vitesse.Y = 8;
                }
                //tir vaisseau
                if (Keyboard.GetState().IsKeyDown(Keys.Space) && Projectile.estVivant == false && Vaisseau.estVivant == true)
                {
                    Projectile.estVivant = true;
                    Projectile.position.X = Vaisseau.position.X + 255;
                    Projectile.position.Y = Vaisseau.position.Y + 102;
                    Projectile.vitesse.X = 50;
                }
                if (Projectile.position.X >= Window.Width)
                {
                    Projectile.estVivant = false;
                    Projectile.vitesse.X = 0;
                }
                //Border Vaisseau
                if (Vaisseau.position.Y + 227 >= Window.Bottom)
                {
                    Vaisseau.vitesse.Y = 0;
                    Vaisseau.position.Y = Window.Bottom - 228;
                }
                if (Vaisseau.position.Y <= Window.Top)
                {
                    Vaisseau.vitesse.Y = 0;
                    Vaisseau.position.Y = Window.Top + 1;
                }
                //Background suivi
                if (Background2.position.X == Window.Left)
                {
                    Background.position.X = Background2.position.X + Background2.sprite.Width;
                }
                if (Background.position.X == Window.Left)
                {
                    Background2.position.X = Background.position.X + Background.sprite.Width;
                }
                //Border Enemy
                for (int i = Enemy.Length - 1; i >= 0; i--)
                {
                    if (Enemy[i].position.X >= Window.Left)
                    {
                        Enemy[i].estVivant = false;
                    }
                }
            }


            base.Update(gameTime);
            UpdateBackground();
            UpdateBackground2();
            UpdateVaisseau();
            UpdateProjectile();
            UpdateEnemy();

        }
        public void UpdateBackground()
        {
            Background.position += Background.vitesse;
        }
        public void UpdateBackground2()
        {
            Background2.position += Background2.vitesse;
        }
        public void UpdateVaisseau()
        {
            Vaisseau.position += Vaisseau.vitesse;
        }
        public void UpdateProjectile()
        {
            Projectile.position += Projectile.vitesse;
        }
        public void UpdateEnemy()
        {
            for (int i = Enemy.Length - 1; i >= 0; i--)
            {
                Enemy[i].position += Enemy[i].vitesse;
            }
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            //menu
            if (Gamestate == false)
            {
                spriteBatch.Draw(Menu.sprite, Menu.position);
            }
            //Jeu
            if (Gamestate == true)
            {
                //Background
                spriteBatch.Draw(Background.sprite, Background.position);
                spriteBatch.Draw(Background2.sprite, Background2.position, effects: SpriteEffects.FlipHorizontally);
                //Enemy
                for (int i = Enemy.Length - 1; i >= 0; i--)
                {
                    if (Enemy[i].estVivant == true)
                    {
                        spriteBatch.Draw(Enemy[i].sprite, Enemy[i].position);
                    }
                }
                //Vaisseau  
                if (Vaisseau.estVivant == true)
                {
                    spriteBatch.Draw(Vaisseau.sprite, Vaisseau.position);
                    if (Projectile.estVivant == true)
                    {
                        spriteBatch.Draw(Projectile.sprite, Projectile.position);
                    }
                }
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
