using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BasicMonoGame;

public class Player : GameObject
{
    private GraphicsDeviceManager _graphics;
    
    public Player(Texture2D texture, Vector2 position, int size, GraphicsDeviceManager graphics) 
        : base(texture, position, size) {
        _speed = Vector2.Zero;
        _graphics = graphics;
    }
    
    public void Update(GameTime gameTime) {
        
        // Déplacements
        var kstate = Keyboard.GetState();
        
        if (kstate.IsKeyDown(Keys.Up)) {
            _speed.X += 0.1f;
        }
        if (kstate.IsKeyDown(Keys.Down)) {
            _speed.X -= 0.1f;
        }
        if (kstate.IsKeyDown(Keys.Right)) {
            _speed.Y += 0.05f;
        }
        if (kstate.IsKeyDown(Keys.Left)) {
            _speed.Y -= 0.05f;
        }

        _position.X += _speed.X;
        _position.Y += _speed.Y;
        if (_speed.X > 0) _speed.X -= 0.05f;
        if (_speed.X < 0) _speed.X += 0.05f;
        if (_speed.Y > 0) _speed.Y -= 0.05f;
        if (_speed.Y < 0) _speed.Y += 0.05f;
        
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
        
        // Taille
        if (kstate.IsKeyDown(Keys.Z))
        {
            _Size += 1;
        }
        
        if (kstate.IsKeyDown(Keys.S))
        {
            _Size -= 1;
        }
    }
}