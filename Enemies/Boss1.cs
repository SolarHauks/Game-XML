using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JeuVideo.Enemies;

public class Boss1(Texture2D texture, Vector2 position, int maxHealth) : Enemy(texture, position, maxHealth)
{
    public bool IsDying { get; private set; }
    private int _deathCounter;

    protected override void DeplacementHorizontal(double dt)
    {
        if (IsDying) return;
        
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
        if (IsDying) return;
        
        Velocity.Y = 25.0f * (float)dt;   // Gravité
        Position.Y += Velocity.Y;
    }
    
    protected override void Animate(Vector2 velocity)
    {
        if (Health <= 0)
        {
            // Gestion de l'animation de mort
            if (!IsDying)
            {
                // Lancement de l'animation de mort, stop des autres comportements
                AnimationManager.SetAnimation("death");
                IsDying = true;
            } 
            else
            {
                // Compteur pour laisser le temps à l'animation
                // Ici ce n'est pas fait très proprement, il faudrait tenir compte du deltaTime (ou faire avec le temps passé)
                // Solution temporaire, peut etre à revoir
                _deathCounter++;
                if (_deathCounter >= 269)
                {
                    // Fin de l'animation de mort, on supprime le boss de la liste d'ennemis (fait dans Game1.cs)
                    IsDying = false;
                }
            }
        } else  // Reste des comportements
        {
            AnimationManager.SetAnimation("fly");            
        }
    }
    
}