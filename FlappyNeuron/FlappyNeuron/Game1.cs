using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NeuralNetwork;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlappyNeuron
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Random random = new Random();
        BinaryStep binaryStep = new BinaryStep();

        List<Pipe> pipes = new List<Pipe>();
        Bird[] birds = new Bird[100];
        float x = 0;
        float lastPipe = 0;

        float[] inputs = new float[2];

        int generation = 0;
        int fitness = 0;

        SpriteFont arial;

        float timeScale = 1;

        //Pipe 
        // Top Pipe
        //Bird
        //NerualNetwork Brain; 231
        //fitness: distance/time alive (also include y distance to gap)


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 1600;
            graphics.PreferredBackBufferHeight = 900;
            /* 
               Control: 'Draws: 597, Updates: 601'
               Test 1: 'Draws: 49011, Updates: 49012'
               Test 2: 'Draws: 597, Updates: 1002'
            */
            //Test 1:
            //this.IsFixedTimeStep = false;
            //graphics.SynchronizeWithVerticalRetrace = false;

            //Test 2:
            //this.TargetElapsedTime = TimeSpan.FromSeconds(1.0f / 100.0f);

        }

        protected override void Initialize()
        {
            Bird.Gravity = 2000 / 60f;
            Bird.JumpPower = -700 / 60f;
            Bird.XSpeed = 600 / 60f;
            Bird.Scale = .3f;
            Pipe.gapHeightOffset = 200;
            Pipe.pairSeparationWidth = 750;

            IsMouseVisible = true;

            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Texture2D birdTexture = Content.Load<Texture2D>("bird");
            Texture2D pipeTexture = Content.Load<Texture2D>("pipe");
            arial = Content.Load<SpriteFont>("Arial");

            pipes.Add(new Pipe(random.Next(200, 700), pipeTexture));
            pipes.Add(new Pipe(random.Next(200, 700), pipeTexture));
            pipes[1].Position.X += Pipe.pairSeparationWidth;
            lastPipe = Pipe.pairSeparationWidth;
            for (int i = 0; i < birds.Length; i++)
            {
                birds[i] = new Bird(new Vector2(100, 500), birdTexture, new NeuralNetwork.NeuralNetwork(2, new NeuralNetwork.IActivation[][]
                {
                    Enumerable.Repeat(binaryStep, 2).ToArray(),
                    Enumerable.Repeat(binaryStep, 3).ToArray(),
                    Enumerable.Repeat(binaryStep, 1).ToArray(),
                }));
                birds[i].Brain.Randomize(random);
            }
        }
        
        protected override void UnloadContent()
        {

        }

        bool killed = false;
        int deadBirds = 0;
        protected override void Update(GameTime gameTime)
        {
            deadBirds = 0;
            KeyboardState keyboardState = Keyboard.GetState();
            timeScale += keyboardState.IsKeyDown(Keys.Up) && timeScale < 30 ? .01f : 0;

            timeScale -= keyboardState.IsKeyDown(Keys.Down) && timeScale > .1 ? .01f : 0;
            this.TargetElapsedTime = TimeSpan.FromSeconds(1.0f / (timeScale*60));

            if (keyboardState.IsKeyDown(Keys.Enter))
            {
                if (!killed)
                {
                    killed = true;
                    foreach(Bird bird in birds)
                    {
                        if (bird.fitness < 0)
                        {
                            bird.fitness = (int)(x + 900 - Math.Abs(bird.Position.Y - pipes[0].Position.Y));
                        }
                    }
                }
            }
            else
            {
                killed = false;
            }

            x += Bird.XSpeed;

            if(x - lastPipe - Pipe.pairSeparationWidth > 0)
            {
                lastPipe = x;
                pipes.Add(new Pipe(random.Next(200, 700), pipes[0].Texture));
            }

            for(int i = 0; i < pipes.Count; i++)
            {
                pipes[i].Update(gameTime);

                if (pipes[i].Position.X < -200)
                {
                    pipes.Remove(pipes[i]);
                    i--;
                }
            }

            foreach (Bird bird in birds)
            {
                if (bird.fitness == -1)
                {
                    int pipeIndex = 1;
                    if(bird.Position.X < pipes[0].Position.X + pipes[0].Texture.Width)
                    {
                        pipeIndex = 0;
                    }
                
                    double output = bird.Brain.Compute(new double[]
                    {
                        bird.Position.Y - pipes[pipeIndex].Position.Y,
                        pipes[pipeIndex].Position.X - bird.Position.X
                    })[0];

                    bool jump = output == 1;
                    bird.Update(gameTime, jump);
                    Rectangle birdBox = new Rectangle((int)bird.Position.X, (int)bird.Position.Y, (int)(bird.Texture.Width * Bird.Scale), (int)(bird.Texture.Height * Bird.Scale));
                    if (birdBox.Intersects(pipes[pipeIndex].topBox) || birdBox.Intersects(pipes[pipeIndex].bottomBox) || birdBox.Y < 0 || birdBox.Y > 900)
                    {
                        bird.fitness = x + 900 - Math.Abs(bird.Position.Y - pipes[0].Position.Y);
                    }
                    fitness = (int) (x + 900 - Math.Abs(bird.Position.Y - pipes[0].Position.Y));
                }
                else
                {
                    deadBirds++;
                    bird.Tint = Color.Red;
                }
            }
            if(deadBirds == birds.Length)
            {
                generation++;

                Array.Sort(birds, (a, b) => b.fitness.CompareTo(a.fitness));
                int crossStart = (int)(birds.Length * .2);
                int randomStart = (int)(birds.Length * .8);
                for (int i = crossStart; i < randomStart; i++)
                {
                    NeuralNetwork.NeuralNetwork toCross = birds[random.Next(0, crossStart)].Brain;
                    birds[i].Brain.Crossover(random, toCross);
                    birds[i].Brain.Mutate(random, .15);
                }
                for (int i = randomStart; i < birds.Length; i++)
                {
                    birds[i].Brain.Randomize(random);
                }

                x = 0;
                Texture2D pipeTexture = pipes[0].Texture;
                pipes.Clear();
                pipes.Add(new Pipe(random.Next(200, 700), pipeTexture));
                pipes.Add(new Pipe(random.Next(200, 700), pipeTexture));
                pipes[1].Position.X += Pipe.pairSeparationWidth;
                lastPipe = Pipe.pairSeparationWidth;
                foreach (Bird bird in birds)
                {
                    bird.ySpeed = 0;
                    bird.Position = new Vector2(100, 500);
                    bird.fitness = -1;
                    bird.Tint = Color.White;
                }
            }
            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.BackToFront);

            foreach (Bird bird in birds)
            {
                bird.Draw(spriteBatch);
            }
            foreach (Pipe pipe in pipes)
            {
                pipe.Draw(spriteBatch);
            }

            spriteBatch.DrawString(arial, $"Generation: {generation}, Dead: {deadBirds} Fitness: {fitness}", new Vector2(500, 10), Color.Blue);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
