using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace JeuVideo;

// core of the MonoGame project, with several critical sections necessary for the game to run:

// heart of the MonoGame project
public class Game : Microsoft.Xna.Framework.Game
{
    // attributs : provide easy access to the various components of MonoGame
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    // tell the project how to start, and add key variables
    public Game()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    // initialize the game upon its startup
    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();
    }

    // used to add and remove assets from the running game from the Content project
    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
    }

    // called on a regular interval to update the game state, e.g. take player inputs, move ships, or animate entities
    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here

        base.Update(gameTime);
    }

    // called on a regular interval to take the current game state and draw the game entities to the screen
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here

        base.Draw(gameTime);
    }
}