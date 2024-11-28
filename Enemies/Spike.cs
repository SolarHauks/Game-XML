using System;
using JeuVideo.Animation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JeuVideo.Enemies;
public class Spike(Texture2D texture, Vector2 position, Player player) : Enemy(texture, position, 0)
{
    protected override void DeplacementHorizontal(double dt)
    {
    }

    protected override void DeplacementVertical(double dt)
    {
    }

    protected override void Animate(Vector2 velocity)
    {
        /*string currentAnim = AnimationManager.GetCurrentAnimation();

        float tolerance = 0;
        if (_player.Position.Y < Position.Y && Math.Abs(_player.Position.X - Position.X) <= tolerance && currentAnim == "idle")
        {
            AnimationManager.SetAnimation("activate");
        }
        else
        {*/
        AnimationManager.SetAnimation("idle");
        /*}*/
    }
}

   

    
