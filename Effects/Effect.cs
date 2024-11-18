using JeuVideo.Animation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JeuVideo.Effects;

public class Effect
{
    private Vector2 _position;
    private int _direction;
    private readonly Vector2 _size;
    private readonly Texture2D _texture;
    private readonly AnimationManager _anim;
    
    public string GetName { get; }

    public Effect(string textureName)
    {
        GetName = textureName;
        _texture = Globals.Content.Load<Texture2D>("Assets/Effects/" + textureName);
        
        _anim = new AnimationManager(_texture);
        _size = _anim.GetSize();
        _anim.SetAnimation(textureName);
        _position = new Vector2(0, -50);
    }

    public void Play(Vector2 position, int direction)
    {
        _position = position;
        _direction = direction;
        _anim.SetAnimation(GetName);
    }
    
    public void Update() => _anim.Update();
    
    public void Draw(Vector2 offset)
    {
        if (!_anim.IsPlaying()) return;
        
        SpriteBatch spriteBatch = Globals.SpriteBatch;
        
        Rectangle sRect = _anim.GetSourceRectangle();
        
        Rectangle dRect = new Rectangle(
            (int)_position.X+(_direction == 1 ? 16 : -16), 
            (int)_position.Y,
            (int)_size.X,
            (int)_size.Y);
        dRect.Offset(offset);
        
        SpriteEffects spriteEffect = (_direction == -1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
        
        Vector2 origin = Vector2.Zero;
        
        spriteBatch.Draw( 
            _texture, // Texture2D,
            dRect, // Rectangle destinationRectangle,
            sRect, // Nullable<Rectangle> sourceRectangle,
            Color.White, //  Color,
            0.0f, //  float rotation,
            origin,  // Vector2 origin,
            spriteEffect, // SpriteEffects effects,
            0f ); // float layerDepth
    }
}