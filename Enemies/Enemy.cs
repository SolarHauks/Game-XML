using System;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JeuVideo.Enemies;

[Serializable]
public abstract class Enemy : GameObject
{
    [XmlIgnore] protected Vector2 StartPosition;    // Position de départ, milieu de la zone de déplacement
    [XmlIgnore] private int _currentHealth;
    [XmlElement("hitboxRatio")] public float HitboxRatio;
    [XmlElement("health")] public int MaxHealth;
    [XmlElement("damage")] public int DamageDealt;  // Dégâts infligés par l'ennemi au joueur

    [XmlIgnore] public int Health
    {
        get => _currentHealth;
        private set => _currentHealth = Math.Clamp(value, 0, MaxHealth);
    }

    protected Enemy(Texture2D texture, Vector2 position, int maxHealth, float hitboxRatio) : base(texture, position, true, hitboxRatio) {
        MaxHealth = maxHealth;
        _currentHealth = maxHealth;
        StartPosition = position;
    }
    
    protected Enemy() { } // Constructeur sans paramètre pour la sérialisation
    
    // Méthode de chargement de l'ennemi pour tout ce qui doit être fait après la désérialisation
    public void Load(Texture2D texture, Vector2 position)
    {
        base.Load(texture, position, true, HitboxRatio);
        _currentHealth = MaxHealth;
        StartPosition = position;
    }

    protected abstract override void DeplacementHorizontal(double dt);

    protected abstract override void DeplacementVertical(double dt);

    protected abstract override void Animate(Vector2 velocity);
    
    public virtual void TakeDamage(int damage, Vector2 source)
    {
        Health -= damage;
        Position.X += (Position.X < source.X ? -8 : 8);
    }
    
}