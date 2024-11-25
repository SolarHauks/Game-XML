using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JeuVideo;

public abstract class Enemy : GameObject
{
    protected Vector2 _startPosition;    // Position de départ, milieu de la zone de déplacement
    protected int _maxHealth;
    protected int _currentHealth;
    public int Health
    {
        get => _currentHealth;
        set
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

    protected abstract override void DeplacementHorizontal(double dt);

    protected abstract override void DeplacementVertical(double dt);

    protected abstract override void Animate(Vector2 velocity);
    public abstract void TakeDamage(int damage, Vector2 source);
    
}