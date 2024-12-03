using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace JeuVideo.UI;

public class Button : Sprite
{
    private MouseState _previousMouseState;
    private readonly Texture2D _texture;
    private Rectangle _rectangle;

    public bool IsClicked { get; private set; }

    public Button(Texture2D texture, Vector2 position) : base(texture, position, false)
    {
        _texture = texture;
        int width = _texture.Width;
        int height = _texture.Height;
        _rectangle = new Rectangle((int)position.X - width / 2, (int)position.Y - height / 2, width*2, height*2);
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
    
    public void Draw()
    {
       Globals.SpriteBatch.Draw(_texture, _rectangle, Color.White);
    }
    
}