using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace JeuVideo.UI;

public class Button(Texture2D texture, Vector2 position) : Sprite(texture, position, false)
{
    private Rectangle _rectangle = new((int)position.X, (int)position.Y, 16*2, 16*2);
    private MouseState _previousMouseState;
    private readonly Texture2D _texture = texture;

    public bool IsClicked { get; private set; }

    public void Update()
    {
        MouseState currentMouseState = Mouse.GetState();
        Point mousePosition = new Point(currentMouseState.X, currentMouseState.Y);

        if (_rectangle.Contains(mousePosition) && currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
        {
            IsClicked = true;
        }
        else
        {
            IsClicked = false;
        }

        _previousMouseState = currentMouseState;
    }
    
    public void Draw()
    {
       Globals.SpriteBatch.Draw(_texture, _rectangle, Color.White);
    }
    
}