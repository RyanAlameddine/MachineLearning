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
        Bird[] birds = new Bird[1];
        float x = 0;
        float lastPipe = 0;

        float[] inputs = new float[2];

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
            Bird.XSpeed = 500 / 60f;
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

        protected override void Update(GameTime gameTime)
        {

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
                    inputs[0] = pipes[pipeIndex].Position.X + pipes[pipeIndex].Texture.Width - bird.Position.X;
                    inputs[1] = Math.Abs(pipes[pipeIndex].Position.Y - bird.Position.Y);
                    //TODO RUN THROUGH NEURAL NET

                    bool jump = false;
                    bird.Update(gameTime, jump);
                    Rectangle birdBox = new Rectangle((int)bird.Position.X, (int)bird.Position.Y, (int)(bird.Texture.Width * Bird.Scale), (int)(bird.Texture.Height * Bird.Scale));
                    if (birdBox.Intersects(pipes[pipeIndex].topBox) || birdBox.Intersects(pipes[pipeIndex].bottomBox))
                    {
                        bird.fitness = x + 900 - Math.Abs(bird.Position.Y - pipes[0].Position.Y);
                    }
                }
                else
                {
                    bird.Tint = Color.Red;
                }
            }
            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            spriteBatch.Begin();

            foreach (Bird bird in birds)
            {
                bird.Draw(spriteBatch);
            }
            foreach (Pipe pipe in pipes)
            {
                pipe.Draw(spriteBatch);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
