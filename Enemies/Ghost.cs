using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JeuVideo.Enemies;

public class Ghost(Texture2D texture, Vector2 position, int maxHealth, Player player) : Enemy(texture, position, maxHealth)
{
    protected override void DeplacementHorizontal(double dt)
    {
        
        Debug.WriteLine("Dans l'écran !!");
        Vector2 directionToPlayer = player.Position - Position;
        directionToPlayer.Normalize();
        Velocity.X = directionToPlayer.X * 50 * (float)dt;
        Position.X += Velocity.X;
        if (Position.X > player.Position.X)
        {
            Direction = -1;
        }
        else if (Position.X < player.Position.X)
        {
            Direction = 1;
        }
    
    }

    protected override void DeplacementVertical(double dt)
    { 
        
        if (Position.Y < player.Position.Y)
        {
            Velocity.Y = 25.0f * (float)dt;   
            Position.Y += Velocity.Y;
        } else if (Position.Y > player.Position.Y)
        {
            Velocity.Y = -25.0f * (float)dt;   
            Position.Y += Velocity.Y;
        }
    }

    protected override void Animate(Vector2 velocity)
    {
        AnimationManager.SetAnimation("fly");
    }
    
    protected override void CheckCollisionsHorizontal(Dictionary<Vector2, int> collision)
    {
    }
    
    protected override void CheckCollisionsVertical(Dictionary<Vector2, int> collision)
    {
    }
    
    
}
