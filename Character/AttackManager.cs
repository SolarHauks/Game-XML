using System.Collections.Generic;
using JeuVideo.Animation;
using JeuVideo.Effects;
using JeuVideo.Enemies;
using Microsoft.Xna.Framework;

namespace JeuVideo.Character;

public class AttackManager
{
    private double _lastAttackTime;
    private const double AttackCooldown = 0.5;
    private readonly EffectsManager _effectsManager;
    private readonly AnimationManager _animationManager;

    public AttackManager(EffectsManager effectsManager, AnimationManager animationManager)
    {
        _effectsManager = effectsManager;
        _animationManager = animationManager;
        _lastAttackTime = -AttackCooldown;
    }

    public bool CanAttack(int manaCost = 0, int currentMana = 0)
    {
        double currentTime = Globals.GameTime.TotalGameTime.TotalSeconds;
        return currentTime - _lastAttackTime >= AttackCooldown && currentMana >= manaCost;
    }

    public void Attack(List<Enemy> enemies, Vector2 position, int direction)
    {
        if (!CanAttack()) return;

        Rectangle hitbox = new Rectangle(
            (int)position.X + (direction == 1 ? 16 : -16),
            (int)position.Y,
            32,
            32
        );

        ApplyDamage(enemies, hitbox, 20);
        _effectsManager.PlayEffect("slash", position, direction);
        _animationManager.SetAnimation("slash");
        _lastAttackTime = Globals.GameTime.TotalGameTime.TotalSeconds;
    }

    public void SpecialAttack(List<Enemy> enemies, Vector2 position, int direction)
    {
        Rectangle hitbox = new Rectangle(
            (int)position.X + (direction == 1 ? 16 : -48),
            (int)position.Y,
            64,
            32
        );

        ApplyDamage(enemies, hitbox, 50);
        Vector2 decalage = new Vector2((direction == 1 ? 32 : -32), 0);
        _effectsManager.PlayEffect("slash", position + decalage, direction);
        _animationManager.SetAnimation("slash");
        _lastAttackTime = Globals.GameTime.TotalGameTime.TotalSeconds;
    }

    private void ApplyDamage(List<Enemy> enemies, Rectangle hitbox, int damage)
    {
        foreach (var enemy in enemies)
        {
            if (enemy is not Spike && hitbox.Intersects(enemy.Rect))
            {
                enemy.TakeDamage(damage, hitbox.Location.ToVector2());
            }
        }
    }
}