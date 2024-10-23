using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace JeuVideo;

// core of the MonoGame project, with several critical sections necessary for the game to run:

// heart of the MonoGame project
public class Game1 : Microsoft.Xna.Framework.Game
{
    // attributs : provide easy access to the various components of MonoGame
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    
    private Vector2 ballPosition;
    private Player player;

    // tell the project how to start, and add key variables
    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    // initialize the game upon its startup
    // The Initialize method is called after the constructor but before the main game loop (Update/Draw).
    // This is where you can query any required services and load any non-graphic related content.
    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        base.Initialize();
    }

    // used to add and remove assets from the running game from the Content project
    // It is called only once per game, within the Initialize method, before the main game loop starts.
    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        
        Texture2D ballTexture = Content.Load<Texture2D>("ball");
        ballPosition = new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);
        player = new Player(ballTexture, ballPosition, 50, _graphics);
    }

    // called on a regular interval to update the game state, e.g. take player inputs, move ships, or animate entities
    // The Update method is called multiple times per second,
    // and it is used to update your game state (checking for collisions, gathering input, playing audio, etc.).
    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        player.Update(gameTime);
        base.Update(gameTime);
    }

    // called on a regular interval to take the current game state and draw the game entities to the screen
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            player.Draw(_spriteBatch);
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}