using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JeuVideo;

public class Sprite {
    
    protected readonly Texture2D Texture;
    protected Vector2 _position;
    private int _size = 100;
    private const int SizeMin = 10;
    private const int SizeMax = 100;
    public int Direction { get; set; } // -1 for left, 1 for right

    public Texture2D _Texture { get => Texture; init => Texture = value; }

    public int Size
    {
        get => _size; 
        set => _size = value < SizeMin ? SizeMin : value > SizeMax ? SizeMax : value;
    }

    protected Rectangle _Rect => new Rectangle((int) _position.X, (int)_position.Y, _size, _size);

    protected Sprite(Texture2D texture, Vector2 position, int size) {
        _Texture = texture;
        _position = position; 
        Size = size;
    }
    
    public void Draw(SpriteBatch spriteBatch)
    {
        SpriteEffects spriteEffect = SpriteEffects.None;
        if (Direction == -1)
        {
            spriteEffect = SpriteEffects.FlipHorizontally;
        }
        
        // var origin = new Vector2(_texture.Width / 2f, _texture.Height / 2f);
        var origin = Vector2.Zero;
        spriteBatch.Draw(   Texture, // Texture2D,
            _Rect, // Rectangle destinationRectangle,
            null, // Nullable<Rectangle> sourceRectangle,
            Color.White, //  Color,
            0.0f, //  float rotation,
            origin,  // Vector2 origin,
            spriteEffect, // SpriteEffects effects,
            0f ); // float layerDepth
    }
    
}
