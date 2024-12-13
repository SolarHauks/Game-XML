using System.Collections.Generic;
using JeuVideo.Character;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JeuVideo.Enemies;

public class Ghost(Texture2D texture, Vector2 position, int maxHealth, Player player) : Enemy(texture, position, maxHealth, 1.0f)
{
    private const int Distance = 250;

    protected override void DeplacementHorizontal(double dt)
    {
        if (CheckPlayerDistance())
        {
            Vector2 directionToTarget = Vector2.Normalize(player.Position - Position);
            Velocity.X = (float)(directionToTarget.X * 50 * dt);
            Position.X += Velocity.X;
            Direction = Position.X > player.Position.X ? -1 : 1;
        }
    }

    protected override void DeplacementVertical(double dt)
    {
        if (CheckPlayerDistance())
        {
            Vector2 directionToTarget = Vector2.Normalize(player.Position - Position);
            Velocity.Y = (float)(directionToTarget.Y * 50 * dt);
            Position.Y += Velocity.Y;
        }
    }
    
    private bool CheckPlayerDistance()
    {
        return (Vector2.Distance(player.Position, Position) < Distance);
    }

    protected override void Animate(Vector2 velocity)
    {
        AnimationManager.SetAnimation("fly");
    }
    
    // On reimplemente ces deux méthodes pour éviter les collisions
    protected override void CheckCollisionsHorizontal(Dictionary<Vector2, int> collision)
    {
    }
    
    protected override void CheckCollisionsVertical(Dictionary<Vector2, int> collision)
    {
    }
    
}
