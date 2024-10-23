using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace JeuVideo;

public class Player : GameObject
{
    private GraphicsDeviceManager _graphics;
    
    public Player(Texture2D texture, Vector2 position, int size, GraphicsDeviceManager graphics) 
        : base(texture, position, size) {
        _speed = 150f;
        _graphics = graphics;
    }
    
    public void Update(GameTime gameTime) {
        
        // Déplacements
        var kstate = Keyboard.GetState();
        
        // The time since Update was called last.
        float updatedBallSpeed = _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        
        if (kstate.IsKeyDown(Keys.Up)) {
            _position.Y -= updatedBallSpeed;
        }
        
        if (kstate.IsKeyDown(Keys.Down)) {
            _position.Y += updatedBallSpeed;
        }
        
        if (kstate.IsKeyDown(Keys.Left)) {
            _position.X -= updatedBallSpeed;
        }
        
        if (kstate.IsKeyDown(Keys.Right)) {
            _position.X += updatedBallSpeed;
        }
        
        // Limites
        if (_position.X > _graphics.PreferredBackBufferWidth - _texture.Width / 2)
        {
            _position.X = _graphics.PreferredBackBufferWidth - _texture.Width / 2;
        }
        else if (_position.X < _texture.Width / 2)
        {
            _position.X = _texture.Width / 2;
        }

        if (_position.Y > _graphics.PreferredBackBufferHeight - _texture.Height / 2)
        {
            _position.Y = _graphics.PreferredBackBufferHeight - _texture.Height / 2;
        }
        else if (_position.Y < _texture.Height / 2)
        {
            _position.Y = _texture.Height / 2;
        }
    }
}