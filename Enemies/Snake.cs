using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JeuVideo.Enemies;

public class Snake(Texture2D texture, Vector2 position, int maxHealth) : Enemy(texture, position, maxHealth, 1.0f)
{
    protected override void DeplacementHorizontal(double dt)
    {
        // On passe par la velocité car on en a besoin pour les collisions
        Velocity.X = Direction * 50 * (float)dt;
        Position.X += Velocity.X;
        if (Position.X > StartPosition.X + 48)  // 48 = 3 tiles
        {
            Direction = -1;
        }
        else if (Position.X < StartPosition.X - 48)
        {
            Direction = 1;
        }
    }

    protected override void DeplacementVertical(double dt)
    {
        Velocity.Y = 25.0f * (float)dt; // Gravité
        Position.Y += Velocity.Y;
    }

    protected override void Animate(Vector2 velocity)
    {
        AnimationManager.SetAnimation("walk");
    }
    
}   