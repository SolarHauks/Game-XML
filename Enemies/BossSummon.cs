using JeuVideo.Character;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JeuVideo.Enemies;

public class BossSummon : GameObject
{
    private const float Speed = 100;
    private const int TimeAlive = 5;
    
    private readonly Vector2 _directionToTarget;
    
    private readonly double _spawnTime;
    private bool _isAlive;
    public bool IsAlive => (Globals.GameTime.TotalGameTime.TotalSeconds - _spawnTime < TimeAlive && _isAlive);

    public BossSummon(Texture2D texture, Vector2 position, Vector2 targetPosition) : base(texture, position, true, 0.4f)
    {
        _directionToTarget = Vector2.Normalize(targetPosition - Position);
        AnimationManager.SetAnimation("spawn");
        _spawnTime = Globals.GameTime.TotalGameTime.TotalSeconds;
        _isAlive = true;
    }
    
    protected override void DeplacementHorizontal(double dt)
    {
        // Pendant l'animation de spawn, on ne fait rien
        if (AnimationManager.GetCurrentAnimation() == "spawn" && AnimationManager.IsPlaying()) { return; }
        
        Velocity.X = (float)(_directionToTarget.X * Speed * dt);
        Position.X += Velocity.X;
    }

    protected override void DeplacementVertical(double dt)
    {
        // Pendant l'animation de spawn, on ne fait rien
        if (AnimationManager.GetCurrentAnimation() == "spawn" && AnimationManager.IsPlaying()) { return; }
        
        Velocity.Y = (float)(_directionToTarget.Y * (Speed / 2) * dt);
        Position.Y += Velocity.Y;
    }

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
    
    public void CheckCollisionWithPlayer(Player player)
    {
        if (IsAlive && player.DamageHitbox.Intersects(DamageHitbox))
        {
            player.TakeDamage(20);
            _isAlive = false; // Le summon disparaît après avoir infligé des dégâts
        }
    }
    
}