using System.Collections.Generic;
using System.Linq;
using JeuVideo.Enemies;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JeuVideo.Weapons;

public class Weapon
{
    private int _damage;
    private float _fireRate;
    private int _nbBulletFired;
    private double _lastAttackTime; // Temps de la dernière attaque

    private Texture2D _projectileTexture;
    
    private readonly List<Projectile> _projectiles = new(); 

    public Weapon()
    {
        _damage = 20;
        _fireRate = 0.5f;
        _lastAttackTime = -_fireRate; // Permet de tirer dès le début
        _nbBulletFired = 1;
        
        // _projectileTexture = Globals.Content.Load<Texture2D>("snake"); // load de la texture des projectiles
        _projectileTexture = Globals.Content.Load<Texture2D>("Assets/Weapons/bullet"); // load de la texture des projectiles
    }
    
    public void Fire(Vector2 position, int direction)
    {
        double currentTime = Globals.GameTime.TotalGameTime.TotalSeconds;
        if (currentTime - _lastAttackTime < _fireRate) { return; }
        
        _lastAttackTime = currentTime;  // Mise à jour du temps de la dernière attaque

        int speed = 250;    // temporaire
        Vector2 angle = new Vector2(1, 0); // temporaire
        
        for (int i=0; i<_nbBulletFired; i++)
        {
            _projectiles.Add(new Projectile(_projectileTexture, position, speed, angle, direction));
        }
    }
    
    public void Update(Dictionary<Vector2, int> collision, List<Enemy> enemies, Vector2 playerPosition)
    {
        foreach (Projectile proj in _projectiles)
        {
            proj.Update(collision);
        }
        
        CleanBullets();
        
        DealingDamage(enemies, playerPosition);
    }
    
    private void CleanBullets()
    {
        foreach (Projectile proj in _projectiles.ToList())
        {
            if (!proj.IsAlive)
            {
                _projectiles.Remove(proj);
            }
        }
    }
    
    public void Draw(Vector2 offset)
    {
        foreach (Projectile proj in _projectiles)
        {
            proj.Draw(offset);
        }
    }

    private void DealingDamage(List<Enemy> enemies, Vector2 playerPosition)
    {
        foreach (Projectile proj in _projectiles)
        {
            Rectangle hitbox = proj.Rect;
            
            foreach (var enemy in enemies)
            {
                if (hitbox.Intersects(enemy.Rect))
                {
                    enemy.TakeDamage(_damage, playerPosition);
                    proj.IsAlive = false;
                }
            }
        }
    }
}