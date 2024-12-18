using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JeuVideo.UI;

public class QuantityBar
{
    private int _currentValue;
    private readonly Vector2 _position;
    
    private Texture2D Bar { get; }

    public QuantityBar(int value, Color color, Vector2 position)
    {
        _currentValue = value;
        _position = position;
        
        Bar = new Texture2D(Globals.GraphicsDevice, 1, 1);
        Bar.SetData([color]);
    }

    public void Set(int value)
    {
        _currentValue = value;
    }

    public void Draw()
    {
        SpriteBatch spriteBatch = Globals.SpriteBatch;
        spriteBatch.Draw(Bar, new Rectangle((int)_position.X, (int)_position.Y, _currentValue, 10), Color.White);
    }
}