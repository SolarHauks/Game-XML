using System;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JeuVideo.Enemies;

[Serializable]
public abstract class Enemy : GameObject
{
    [XmlIgnore] protected Vector2 StartPosition;    // Position de départ, milieu de la zone de déplacement
    [XmlElement("health")] public int _maxHealth;
    [XmlIgnore] private int _currentHealth;
    [XmlIgnore] public int Health
    {
        get => _currentHealth;
        private set => _currentHealth = Math.Clamp(value, 0, _maxHealth);
    }

    protected Enemy(Texture2D texture, Vector2 position, int maxHealth, float hitboxRatio) : base(texture, position, true, hitboxRatio) {
        _maxHealth = maxHealth;
        _currentHealth = maxHealth;
        StartPosition = position;
    }
    
    // Constructeur sans paramètre pour la sérialisation
    protected Enemy() { }
    
    public void Load(Texture2D texture, Vector2 position, float hitboxRatio)
    {
        base.Load(texture, position, true, hitboxRatio);
        _currentHealth = _maxHealth;
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