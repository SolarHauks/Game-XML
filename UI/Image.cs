using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JeuVideo.UI;

public class Image : Sprite
{
    private readonly Texture2D _texture;
    private readonly Rectangle _rectangle;
    private SpriteFont _font;
    private Vector2 _textPosition;

    public Image(Texture2D texture, Vector2 position) : base(texture, position,
        false)
    {
        _texture = texture;

        int width = _texture.Width;
        int height = _texture.Height;
        _rectangle = new Rectangle((int)position.X - width / 2, (int)position.Y - height / 2, width, height);

    }

    public void Draw()
    {
        Globals.SpriteBatch.Draw(_texture, _rectangle, Color.White);
    }
    
}