using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JeuVideo.Effects;

public class Effect : Sprite
{
    private string GetName { get; }
    
    public Effect(string textureName) : base(Globals.Content.Load<Texture2D>("Assets/Effects/" + textureName), new Vector2(0, -50))
    {
        AnimationManager.SetAnimation(textureName);
        GetName = textureName;
    }

    public void Play(Vector2 position, int direction)
    {
        if (AnimationManager.IsPlaying()) return;
        
        Position = position;
        Position.X += (Direction == 1 ? 16 : -16);  // Pour l'instant fixé à 16, à adapter par la suite
        Direction = direction;
        AnimationManager.SetAnimation(GetName);
    }
    
    public void Update() => AnimationManager.Update();
    
    public new void Draw(Vector2 offset)
    {
        if (AnimationManager.IsPlaying())
        {
            base.Draw(offset);
        }
        
        
    }
}