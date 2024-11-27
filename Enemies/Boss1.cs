﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JeuVideo.Enemies;

public class Boss1(Texture2D texture, Vector2 position, int maxHealth) : Enemy(texture, position, maxHealth)
{
    protected override void DeplacementHorizontal(double dt)
    {
        Velocity.X = Direction * 20 * (float)dt;
        Position.X += Velocity.X;
        if (Position.X > StartPosition.X + 50)
        {
            Direction = -1;
        } else if (Position.X < StartPosition.X - 50)
        {
            Direction = 1;
        }
    }
    protected override void DeplacementVertical(double dt)
    {
        Velocity.Y = 25.0f * (float)dt;   // Gravité
        Position.Y += Velocity.Y;
    }
    
    protected override void Animate(Vector2 velocity)
    {
        AnimationManager.SetAnimation("fly");
    }
    
    
    
    public override void TakeDamage(int damage, Vector2 source)
    {
        Health -= damage;
        Position.X += (Position.X < source.X ? -8 : 8);
        Console.Out.WriteLine("Enemy hit! Health: " + Health);
    }
}