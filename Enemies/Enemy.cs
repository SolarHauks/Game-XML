using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JeuVideo.Enemies;

public abstract class Enemy : GameObject
{
    protected Vector2 StartPosition;    // Position de départ, milieu de la zone de déplacement
    protected int MaxHealth;
    protected int CurrentHealth;
    public int Health
    {
        get => CurrentHealth;
        protected set => CurrentHealth = Math.Clamp(value, 0, MaxHealth);
    }
    
    public Enemy(Texture2D texture, Vector2 position, int maxHealth) : base(texture, position, true) {
        MaxHealth = maxHealth;
        CurrentHealth = maxHealth;
        StartPosition = position;
    }

    protected abstract override void DeplacementHorizontal(double dt);

    protected abstract override void DeplacementVertical(double dt);

    protected abstract override void Animate(Vector2 velocity);
    public abstract void TakeDamage(int damage, Vector2 source);
    
}