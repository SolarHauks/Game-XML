using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace JeuVideo.Menu;

public class Button
{
    private Rectangle _rectangle;
    private MouseState _previousMouseState;

    public bool IsClicked { get; private set; }

    public Button(Vector2 position)
    {
        _rectangle = new Rectangle((int)position.X, (int)position.Y, 30*2, 14*2);
    }

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
    
}