using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JeuVideo.Enemies;

public class Spike(Texture2D texture, Vector2 position) : Enemy(texture, position, 100)
{
    protected override void DeplacementHorizontal(double dt)
    {
    }

    protected override void DeplacementVertical(double dt)
    {
    }

    protected override void Animate(Vector2 velocity)
    {
        AnimationManager.SetAnimation("idle");
    }
}

   

    
