using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Threading;

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
        GameObject[] Enemy = new GameObject[8];
        GameObject[] Projectile = new GameObject[2];
        GameObject Background;
        GameObject Background2;
        GameObject Menu;
        Rectangle Window;
        bool Gamestate;
        bool MenuOver = false;
        Random rng = new Random();
        Song Song;
        SoundEffect SonKnockout;
        SoundEffect SonDeath;
        SoundEffect SonTir;
        SoundEffectInstance Knockout;
        SoundEffectInstance Death;
        SoundEffectInstance Tir;
        SpriteFont text;
        double kills = 0;
        double score = 0;



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
            Background2.vitesse.X = -13;
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
                Enemy[i].position.X = Window.Width - 150;
                Enemy[i].position.Y = rng.Next(0, Window.Height - 80);
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
            for (int i = 0; i < Projectile.Length; i++)
            {
                Projectile[i] = new GameObject();
                Projectile[i].estVivant = false;
                if (i == 1)
                {
                    Projectile[i].sprite = Content.Load<Texture2D>("Projectile.png");
                }
                else
                {
                    Projectile[i].sprite = Content.Load<Texture2D>("Projectile2.png");
                }
            }
            //son
            Song song = Content.Load<Song>("Sunny");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.40F;
            MediaPlayer.Play(song);
            SonDeath = Content.Load<SoundEffect>("Sad");
            Death = SonDeath.CreateInstance();
            SonKnockout = Content.Load<SoundEffect>("Knockout");
            Knockout = SonKnockout.CreateInstance();
            SonTir = Content.Load<SoundEffect>("Tir");
            Tir = SonTir.CreateInstance();
            //Texte
            text = Content.Load<SpriteFont>("Font");
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
                    Vaisseau.estVivant = true;
                    MenuOver = false;                   
                    Background.position.X = 0;
                    Background.position.Y = 0;
                    Background.vitesse.X = -15;                   
                    Background2.position.X = 2880;
                    Background2.position.Y = 0;
                    Background2.vitesse.X = -15;
                    Death.Stop();
                    MediaPlayer.Resume();
                    for (int i = Enemy.Length - 1; i >= 0; i--)
                    {
                        Enemy[i].estVivant = true;
                        Enemy[i].position.X = Window.Width - 150;
                        Enemy[i].position.Y = rng.Next(0, Window.Height - 80);
                    }
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    this.Exit();
                }
            }
            if (Gamestate == true)
            {                              
                for (int i = Enemy.Length - 1; i >= 0; i--)
                {
                    Enemy[i].vitesse.X = rng.Next(-21, -17);
                }
                //Quitter
                if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    Gamestate = false;
                    Menu.estVivant = true;
                }
                //Movement Vaisseau
                Vaisseau.vitesse.Y = 0;
                if (Keyboard.GetState().IsKeyDown(Keys.W))
                {
                    Vaisseau.vitesse.Y = -11;
                }                          
                if (Keyboard.GetState().IsKeyDown(Keys.S))
                {
                    Vaisseau.vitesse.Y = 11;
                }

                //tir vaisseau  
                if (rng.Next(0, 2) == 1)
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.Space) && Projectile[0].estVivant == false && Vaisseau.estVivant == true)
                    {
                        Projectile[0].estVivant = true;
                        Projectile[0].position.X = Vaisseau.position.X + 255;
                        Projectile[0].position.Y = Vaisseau.position.Y + 102;
                        Projectile[0].vitesse.X = 75;
                        Tir.Play();
                        Tir.Volume = 0.25F;
                    }
                }
                else
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.Space) && Projectile[1].estVivant == false && Vaisseau.estVivant == true)
                    {
                        Projectile[1].estVivant = true;
                        Projectile[1].position.X = Vaisseau.position.X + 255;
                        Projectile[1].position.Y = Vaisseau.position.Y + 102;
                        Projectile[1].vitesse.X = 50;
                    }
                }
                for (int i = 0; i < Projectile.Length; i++)
                {
                    if (Projectile[i].position.X >= Window.Width)
                    {
                        Projectile[i].estVivant = false;
                        Projectile[i].vitesse.X = 0;
                    }
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
                    if (Enemy[i].position.X <= Window.Left)
                    {
                        Enemy[i].estVivant = false;
                    }
                }
            }
            //mort vaisseau
            for (int i = Enemy.Length - 1; i >= 0; i--)
            {
                if (this.Vaisseau.GetRekt().Intersects(this.Enemy[i].GetRekt()) && Enemy[i].estVivant == true)
                {
                    Vaisseau.estVivant = false;
                    MenuOver = true;
                    Menu.estVivant = true;
                    Gamestate = false;
                    Enemy[i].estVivant = false;
                    MediaPlayer.Pause();
                    Death.Play();
                    kills = 0;             
                }
            }
            //Border Enemy & respawn
            for (int i = Enemy.Length - 1; i >= 0; i--)
            {
                if (Enemy[i].estVivant == true && Enemy[i].position.X <= Window.Left)
                {
                    Enemy[i].estVivant = false;
                    Enemy[i].position.X = Window.Width - 150;
                    Enemy[i].position.Y = rng.Next(0, Window.Height - 80);
                }
                if (Enemy[i].estVivant == false && gameTime.TotalGameTime.Seconds % 3 == 0)
                {
                    Enemy[i].estVivant = true;
                    Enemy[i].position.X = Window.Width - 150;
                    Enemy[i].position.Y = rng.Next(0, Window.Height - 80);
                }
            }
            //Mort Enemy & projectile
            for (int i = Enemy.Length - 1; i >= 0; i--)
            {
                for (int i2 = Projectile.Length - 1; i2 >= 0; i2--)
                {
                    if (this.Projectile[i2].GetRekt().Intersects(this.Enemy[i].GetRekt()) && Enemy[i].estVivant == true)
                    {
                        Enemy[i].estVivant = false;
                        Projectile[i2].estVivant = false;
                        MediaPlayer.Pause();
                        Knockout.Play();
                        Knockout.Volume = 0.40F;
                        MediaPlayer.Resume();
                        kills++;
                    }
                }
            }
            //GameOver
            if (Gamestate == false && MenuOver == true)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    this.Exit();
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    Gamestate = true;
                    Menu.estVivant = false;
                    MenuOver = false;
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
            for (int i = 0; i < Projectile.Length; i++)
            {
                Projectile[i].position += Projectile[i].vitesse;
            }
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
            if (MenuOver == true)
            {
                spriteBatch.DrawString(text, "Play again ?", new Vector2(Window.Center.X - 100, Window.Bottom - 250), Color.White);
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
                    if (Enemy[i].estVivant == false)
                    {
                        Enemy[i].sprite = Content.Load<Texture2D>("Death.png");
                        spriteBatch.Draw(Enemy[i].sprite, Enemy[i].position);
                        if (rng.Next(0, 2) == 1)
                        {
                            Enemy[i].sprite = Content.Load<Texture2D>("Enemy2.png");
                        }
                        else
                        {
                            Enemy[i].sprite = Content.Load<Texture2D>("Enemy1.png");
                        }
                    }
                }
                //Vaisseau 
                
                if (Vaisseau.estVivant == true)
                {
                    spriteBatch.Draw(Vaisseau.sprite, Vaisseau.position);
                //projectile
                for (int i = 0; i < Projectile.Length; i++)
                {
                    if (Projectile[i].estVivant == true)
                    {
                        spriteBatch.Draw(Projectile[i].sprite, Projectile[i].position);
                    }
                }
                //Vaisseau Mort
                if (Vaisseau.estVivant == false)
                    {
                        Vaisseau.sprite = Content.Load<Texture2D>("DeathV.png");
                        spriteBatch.Draw(Vaisseau.sprite, Vaisseau.position);
                        Vaisseau.sprite = Content.Load<Texture2D>("Vaisseau.png");
                    }
                }

                //texte
                if (Gamestate == true)
                {    
                    if (score < kills)
                    {
                        score = kills;
                    }                           
                    spriteBatch.DrawString(text, kills.ToString(), new Vector2(50, 50), Color.White);
                    spriteBatch.DrawString(text, "highscore : " + score.ToString(), new Vector2(300, 50), Color.White);
                }
                else
                {
                    kills = 0;
                }
            }            
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
