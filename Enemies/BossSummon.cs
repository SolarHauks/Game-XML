using JeuVideo.Character;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JeuVideo.Enemies;

// Summon invoqué par le boss
// Hérite de GO car n'a pas les propriétés d'un ennemi classique
public class BossSummon : GameObject
{
    private readonly float _speed;  // Vitesse du summon
    private readonly int _timeAlive;    // Temps de vie du summon
    private readonly int _damage;   // Dégâts infligés par le summon
    
    private readonly Vector2 _directionToTarget;    // Direction
    
    private readonly double _spawnTime; // Moment d'apparition du summon
    private bool _isAlive;  // Est-ce que le summon est en vie ?
    public bool IsAlive => (Globals.GameTime.TotalGameTime.TotalSeconds - _spawnTime < _timeAlive && _isAlive);

    public BossSummon(Texture2D texture, Vector2 position, Vector2 targetPosition, int speed, int timeAlive, int damage) 
        : base(texture, position, true, 0.4f)
    {
        // A noter que le calcul de la direction n'est pas précis pour une raison inconnue
        _directionToTarget = Vector2.Normalize(targetPosition - Position);  // Calcul de la direction
        AnimationManager.SetAnimation("spawn");   // Animation de spawn
        _spawnTime = Globals.GameTime.TotalGameTime.TotalSeconds;   // Moment d'apparition
        _isAlive = true;
        
        _speed = speed;
        _timeAlive = timeAlive;
        _damage = damage;
    }
    
    // On se déplace dans la direction calculée au spawn
    protected override void DeplacementHorizontal(double dt)
    {
        // Pendant l'animation de spawn, on ne fait rien
        if (AnimationManager.GetCurrentAnimation() == "spawn" && AnimationManager.IsPlaying()) { return; }
        
        Velocity.X = (float)(_directionToTarget.X * _speed * dt);
        Position.X += Velocity.X;
    }

    protected override void DeplacementVertical(double dt)
    {
        // Pendant l'animation de spawn, on ne fait rien
        if (AnimationManager.GetCurrentAnimation() == "spawn" && AnimationManager.IsPlaying()) { return; }
        
        Velocity.Y = (float)(_directionToTarget.Y * (_speed / 2) * dt);
        Position.Y += Velocity.Y;
    }

    // A l'apparition on joue une animation de spawn, puis on passe à l'animation idle
    protected override void Animate(Vector2 velocity)
    {
        // Pendant l'animation de spawn, on ne fait rien
        if (AnimationManager.GetCurrentAnimation() == "spawn" && AnimationManager.IsPlaying()) { return; }
        
        AnimationManager.SetAnimation("idle");
    }
    
    // En cas de collision on ne fait rien, car le projectile passe à travers les murs
    protected override void WhenHorizontalCollisions(Rectangle rect)
    {
    }
    
    // idem
    protected override void WhenVerticalCollisions(Rectangle rect)
    {
    }
    
    // On vérifie si le summon est en collision avec le joueur, si oui on lui inflige des dégâts
    public void CheckCollisionWithPlayer(Player player)
    {
        if (IsAlive && player.DamageHitbox.Intersects(DamageHitbox))
        {
            player.TakeDamage(_damage);
            _isAlive = false; // Le summon disparaît après avoir infligé des dégâts
        }
    }
    
}