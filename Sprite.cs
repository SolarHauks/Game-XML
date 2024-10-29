using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JeuVideo;

public class Sprite {
    
    protected Vector2 _position;
    protected int _size;
    protected Rectangle _Rect => new Rectangle((int) _position.X, (int)_position.Y, _size, _size);
    
    protected int Direction { get; set; } // -1 for left, 1 for right
    
    public Texture2D Texture { get; }
    
    protected Sprite(Texture2D texture, Vector2 position, int size) {
        Texture = texture;
        _position = position;
        this._size = size;
    }
    
    public void Draw(SpriteBatch spriteBatch)
    {
        SpriteEffects spriteEffect = (Direction == -1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
        
        // var origin = new Vector2(Texture.Width / 2f, Texture.Height / 2f);
        Vector2 origin = Vector2.Zero;
        spriteBatch.Draw( Texture, // Texture2D,
            _Rect, // Rectangle destinationRectangle,
            null, // Nullable<Rectangle> sourceRectangle,
            Color.White, //  Color,
            0.0f, //  float rotation,
            origin,  // Vector2 origin,
            spriteEffect, // SpriteEffects effects,
            0f ); // float layerDepth
    }
    
}
