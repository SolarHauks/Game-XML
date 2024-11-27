using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JeuVideo.Enemies;

public abstract class Enemy : GameObject
{
    protected Vector2 StartPosition;    // Position de départ, milieu de la zone de déplacement
    private readonly int _maxHealth;
    private int _currentHealth;
    public int Health
    {
        get => _currentHealth;
        private set => _currentHealth = Math.Clamp(value, 0, _maxHealth);
    }

    protected Enemy(Texture2D texture, Vector2 position, int maxHealth) : base(texture, position, true) {
        _maxHealth = maxHealth;
        _currentHealth = maxHealth;
        StartPosition = position;
    }

    protected abstract override void DeplacementHorizontal(double dt);

    protected abstract override void DeplacementVertical(double dt);

    protected abstract override void Animate(Vector2 velocity);

    public virtual void TakeDamage(int damage, Vector2 source)
    {
        Health -= damage;
        Position.X += (Position.X < source.X ? -8 : 8);
        Console.Out.WriteLine("Enemy hit! Health: " + Health);
    }
    
}