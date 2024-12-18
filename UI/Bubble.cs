using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JeuVideo.UI;

public class Bubble
{
    private readonly Texture2D _texture;
    
    // Permet de split la texture de bulle pour pouvoir la scale selon la taille du texte
    private readonly Rectangle[] _sourceRectangles;
    private readonly Rectangle[] _destinationRectangles;
    
    public bool Visible { get; set; } = false;
    private int Width { get; set; }
    private int Height { get; set; }
    private const int CornerSize = 12;
    private readonly SpriteFont _font;
    private string _text = "";
    private Vector2 _textPosition;
    public Color TextColor { get; set; } = Color.White;

    public Bubble(Texture2D texture, SpriteFont font)
    {
        _font = font;
        _texture = texture;
        _sourceRectangles = new Rectangle[9];
        _destinationRectangles = new Rectangle[9];

        /*
        012
        345
        678
        */

        _sourceRectangles[0] = new(0, 0, CornerSize, CornerSize);
        _sourceRectangles[1] = new(CornerSize, 0, _texture.Width - 2 * CornerSize, CornerSize);
        _sourceRectangles[2] = new(_texture.Width - CornerSize, 0, CornerSize, CornerSize);
        _sourceRectangles[3] = new(0, CornerSize, CornerSize, _texture.Height - 2 * CornerSize);
        _sourceRectangles[4] = new(CornerSize, CornerSize, _texture.Width - 2 * CornerSize, _texture.Height - 2 * CornerSize);
        _sourceRectangles[5] = new(_texture.Width - CornerSize, CornerSize, CornerSize, _texture.Height - 2 * CornerSize);
        _sourceRectangles[6] = new(0, _texture.Height - CornerSize, CornerSize, CornerSize);
        _sourceRectangles[7] = new(CornerSize, _texture.Height - CornerSize, _texture.Width - 2 * CornerSize, CornerSize);
        _sourceRectangles[8] = new(_texture.Width - CornerSize, _texture.Height - CornerSize, CornerSize, CornerSize);
    }

    private void CalculateDestinationRectangles()
    {
        Vector2 screenSize = Globals.ScreenSize;
        
        var textSize = _font.MeasureString(_text);
        Width = (int)screenSize.X - 24;
        Height = (int)textSize.Y + 2 * CornerSize;
        
        int w = Width - 2 * CornerSize;
        int h = Height - 2 * CornerSize;
        int x = 12;
        int y = (int)(screenSize.Y - Height - 12);

        _textPosition = new(x + CornerSize, y + CornerSize);

        _destinationRectangles[0] = new(x, y, CornerSize, CornerSize);
        _destinationRectangles[1] = new(x + CornerSize, y, w, CornerSize);
        _destinationRectangles[2] = new(x + Width - CornerSize, y, CornerSize, CornerSize);
        _destinationRectangles[3] = new(x, y + CornerSize, CornerSize, h);
        _destinationRectangles[4] = new(x + CornerSize, y + CornerSize, w, h);
        _destinationRectangles[5] = new(x + Width - CornerSize, y + CornerSize, CornerSize, h);
        _destinationRectangles[6] = new(x, y + Height - CornerSize, CornerSize, CornerSize);
        _destinationRectangles[7] = new(x + CornerSize, y + Height - CornerSize, w, CornerSize);
        _destinationRectangles[8] = new(x + Width - CornerSize, y + Height - CornerSize, CornerSize, CornerSize);
    }

    public void SetText(string text)
    {
        _text = text;
        CalculateDestinationRectangles();
    }

    public void Draw()
    {
        for (int i = 0; i < _sourceRectangles.Length; i++)
        {
            Globals.SpriteBatch.Draw(_texture, _destinationRectangles[i], _sourceRectangles[i], Color.White);
        }

        Globals.SpriteBatch.DrawString(_font, _text, _textPosition, TextColor);
    }
}