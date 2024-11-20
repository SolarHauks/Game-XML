using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JeuVideo;

public class Enemy : GameObject
{
    private readonly Vector2 _startPosition;    // Position de départ, milieu de la zone de déplacement
    
    private readonly int _maxHealth;
    private int _currentHealth;

    public int Health
    {
        get => _currentHealth;
        private set
        {
            if (value < 0) _currentHealth = 0;
            else if (value > _maxHealth) _currentHealth = _maxHealth;
            else _currentHealth = value;
        }
    }
    
    public Enemy(Texture2D texture, Vector2 position, int maxHealth) : base(texture, position, true) {
        _maxHealth = maxHealth;
        _currentHealth = maxHealth;
        _startPosition = position;
    }

    protected override void DeplacementHorizontal(double dt)
    {
        // On passe par la velocité car on en a besoin pour les collisions
        Velocity.X = Direction * 50 * (float)dt;
        Position.X += Velocity.X;
        if (Position.X > _startPosition.X + 50 || Position.X < _startPosition.X - 50)
        {
            Direction *= -1;
        }
    }

    protected override void DeplacementVertical(double dt)
    {
        Velocity.Y = 25.0f * (float)dt;   // Gravité
        Position.Y += Velocity.Y;
    }

    protected override void Animate(Vector2 velocity)
    {
        AnimationManager.SetAnimation("walk");
    }

    public void TakeDamage(int damage, Vector2 source)
    {
        Health -= damage;
        Position.X += (Position.X < source.X ? -8 : 8);
    }
}