using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Threading;

namespace Game2
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        new Rectangle Window;
        GameObject Mage;
        GameObject[] Enemy = new GameObject[3];
        GameObject Sort;
        Texture2D Background;
        Random rng = new Random();
        int NbEnemy = 0;
        bool Victoire = false;
        SpriteFont Text;
        SoundEffect sonRekt;
        SoundEffect sonFuck;
        SoundEffectInstance Rekt;
        SoundEffectInstance Fuck;


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
            Background = Content.Load<Texture2D>("Background.jpg");
            //Enemy
            for (int i = Enemy.Length - 1; i >= 0; i--)
            {
                Enemy[i] = new GameObject();
                Enemy[i].estVivant = false;
                Enemy[i].position.X = Window.Right - 250;
                Enemy[i].position.Y = Window.Center.Y;
                Enemy[i].vitesse.X = rng.Next(-13, -7);
                Enemy[i].vitesse.Y = rng.Next(-10, 9);
                Enemy[i].sprite = Content.Load<Texture2D>("Enemy.png");
            }
            //Hero
            Mage = new GameObject();
            Mage.estVivant = true;
            Mage.position.X = Window.Left;
            Mage.position.Y = Window.Center.Y;
            Mage.sprite = Content.Load<Texture2D>("Hero.png");
            //Sort
            Sort = new GameObject();
            Sort.estVivant = false;
            Sort.position.X = Mage.position.X + 100;
            Sort.position.Y = Mage.position.Y + 50;
            Sort.vitesse.X = 50;
            Sort.sprite = Content.Load<Texture2D>("Sort.png");
            //Text 
            Text = Content.Load<SpriteFont>("Font");
            //Sounds
            sonRekt = Content.Load<SoundEffect>("Song\\rekt");
            Rekt = sonRekt.CreateInstance();
            sonFuck = Content.Load<SoundEffect>("Song\\fuck");
            Fuck = sonFuck.CreateInstance();
            Song song = Content.Load<Song>("Song\\mix");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.05F;         
            MediaPlayer.Play(song);
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
            //End Game defeat
            if (Mage.estVivant == false)
            {
                MediaPlayer.Stop();
                Fuck.Play();
                Thread.Sleep(2000);
                this.Exit();
            }
            //End Game victoire
            if (!Enemy[0].estVivant && !Enemy[1].estVivant && !Enemy[2].estVivant && Mage.estVivant && gameTime.TotalGameTime.Seconds >= 3)
            {
                MediaPlayer.Stop();
                Rekt.Play();
                Thread.Sleep(2000);
                this.Exit();
            }
            //if (Mage.estVivant == true)
            //spawn enemy
            for (int i = Enemy.Length - 1; i >= 0; i--)
            {
                if (NbEnemy * 1 < gameTime.TotalGameTime.Seconds && NbEnemy < Enemy.Length)
                {
                    Enemy[NbEnemy].estVivant = true;
                    NbEnemy++;              
                }
            }
            //déplacement
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                Mage.vitesse.X -= 1;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                Mage.vitesse.X += 1;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                Mage.vitesse.Y -= 1;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                Mage.vitesse.Y += 1;
            }
            //limite de map
            if (Mage.position.X + 125 >= Window.Right)
            {
                Mage.vitesse.X = 0;
                Mage.position.X = Window.Right - 126;
            }
            if (Mage.position.X <= Window.Left)
            {
                Mage.vitesse.X = 0;
                Mage.position.X = Window.Left + 1;
            }
            if (Mage.position.Y + 150 >= Window.Bottom)
            {
                Mage.vitesse.Y = 0;
                Mage.position.Y = Window.Bottom - 151;
            }
            if (Mage.position.Y <= Window.Top)
            {
                Mage.vitesse.Y = 0;
                Mage.position.Y = Window.Top + 1;
            }
            //limite de map enemy
            for (int i = Enemy.Length - 1; i >= 0; i--)
            {
                if (Enemy[i].position.Y <= Window.Top)
                {
                    Enemy[i].vitesse.Y = rng.Next(5, 10);
                }
                if (Enemy[i].position.Y >= Window.Bottom - 175)
                {
                    Enemy[i].vitesse.Y = rng.Next(-11, -6);
                }
                if (Enemy[i].position.X <= Window.Left)
                {
                    Enemy[i].vitesse.X = rng.Next(5, 10);
                }
                if (Enemy[i].position.X >= Window.Right - 175)
                {
                    Enemy[i].vitesse.X = rng.Next(-11, -6);
                }
            }
            //Mage attaque
            if (Mage.estVivant == true && Keyboard.GetState().IsKeyDown(Keys.Space) && Sort.estVivant == false)
            {
                Sort.estVivant = true;
                Sort.position.X = Mage.position.X + 75;
                Sort.position.Y = Mage.position.Y - 40;
            }        
            if (Sort.position.X >= Window.Right)
            {
                Sort.estVivant = false;
            }
            //Colision et mort des enemies
            for (int i = Enemy.Length - 1; i >= 0; i--)
            {
                if (this.Sort.GetRekt().Intersects(this.Enemy[i].GetRekt()) && Enemy[i].estVivant == true)
                {
                    Enemy[i].estVivant = false;
                    Sort.estVivant = false;
                    Rekt.Play();
                }
            }
            //Colision Mage Enemy
            for (int i = Enemy.Length - 1; i >= 0; i--)
            {
                if (Mage.estVivant == true && Enemy[i].estVivant == true)
                {
                    if (this.Mage.GetRekt().Intersects(this.Enemy[i].GetRekt()))
                    {
                        Mage.estVivant = false;
                        Rekt.Play();
                    }
                }
            }          
            

            // TODO: Add your update logic here
            base.Update(gameTime);
            UpdateMage();
            UpdateEnemy();
            UpdateSort();
        }
        public void UpdateMage()
        {
            Mage.position += Mage.vitesse;
        }
        public void UpdateEnemy()
        {
            for (int i = Enemy.Length - 1; i >= 0; i--)
            {
                Enemy[i].position += Enemy[i].vitesse;
            }
        }
        public void UpdateSort()
        {
            Sort.position += Sort.vitesse;
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
            //Background
            spriteBatch.Draw(Background, new Rectangle(0, 0, graphics.GraphicsDevice.DisplayMode.Width, graphics.GraphicsDevice.DisplayMode.Height), Color.White);
            //Enemy              
            for (int i = 0; i <= 2; i++)
            {
                if (Enemy[i].estVivant == true)
                {
                    spriteBatch.Draw(Enemy[i].sprite, Enemy[i].position);
                }
            }
            
            //Mage
            if (Mage.estVivant == true)
            {
                spriteBatch.Draw(Mage.sprite, Mage.position);              
            }
            if(Mage.estVivant == true && Sort.estVivant == true)
            {
                spriteBatch.Draw(Sort.sprite, Sort.position);
            }
            //Text
            spriteBatch.DrawString(Text, gameTime.TotalGameTime.TotalSeconds.ToString(), new Vector2(50, 50), Color.Black);
            if (Mage.estVivant == false)
            {
                spriteBatch.DrawString(Text, gameTime.TotalGameTime.TotalSeconds.ToString(), new Vector2(825, 370), Color.Black);
            }
            if (!Enemy[0].estVivant && !Enemy[1].estVivant && !Enemy[2].estVivant && Mage.estVivant && gameTime.TotalGameTime.Seconds >= 3)
            {
                spriteBatch.DrawString(Text, gameTime.TotalGameTime.TotalSeconds.ToString(), new Vector2(825, 370), Color.Black);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
