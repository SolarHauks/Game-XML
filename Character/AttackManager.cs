using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using JeuVideo.Animation;
using JeuVideo.Effects;
using JeuVideo.Enemies;
using Microsoft.Xna.Framework;

namespace JeuVideo.Character;

[Serializable]
[XmlRoot("attack", Namespace = "https://www.univ-grenoble-alpes.fr/jeu/character")]
public class AttackManager
{
    [XmlIgnore] private double _lastAttackTime;
    [XmlIgnore] public EffectsManager EffectsManager;   // Public car doit etre set de player
    [XmlIgnore] public AnimationManager AnimationManager;
    
    [XmlElement("attackCooldown")] public float AttackCooldown;
    [XmlElement("attackHitbox")] public int AttackHitbox;
    [XmlElement("attackDamage")] public int AttackDamage;
    [XmlElement("specialHitbox")] public int SpecialHitbox;
    [XmlElement("specialDamage")] public int SpecialDamage;
    [XmlElement("specialCost")] public int SpecialCost;
    
    public void Load(AnimationManager animationManager, EffectsManager effectsManager)
    {
        AnimationManager = animationManager;
        EffectsManager = effectsManager;
        _lastAttackTime = -AttackCooldown;
    }

    public bool CanAttack(int currentMana = 0, bool isSpecial = false)
    {
        double currentTime = Globals.GameTime.TotalGameTime.TotalSeconds;
        return currentTime - _lastAttackTime >= AttackCooldown && currentMana >= (isSpecial ? SpecialCost : 0);
    }

    public void Attack(List<Enemy> enemies, Vector2 position, int direction)
    {
        if (!CanAttack()) return;

        Rectangle hitbox = new Rectangle(
            (int)position.X + (direction == 1 ? 16 : -16),
            (int)position.Y,
            AttackHitbox,
            32
        );

        ApplyDamage(enemies, hitbox, AttackDamage);
        EffectsManager.PlayEffect("slash", position, direction);
        Console.WriteLine(AnimationManager == null);
        AnimationManager.SetAnimation("slash");
        _lastAttackTime = Globals.GameTime.TotalGameTime.TotalSeconds;
    }

    public void SpecialAttack(List<Enemy> enemies, Vector2 position, int direction)
    {
        Rectangle hitbox = new Rectangle(
            (int)position.X + (direction == 1 ? 16 : -48),
            (int)position.Y,
            SpecialHitbox,
            32
        );

        ApplyDamage(enemies, hitbox, SpecialDamage);
        Vector2 decalage = new Vector2((direction == 1 ? 32 : -32), 0); // décalage de l'effet de slash
        EffectsManager.PlayEffect("slash", position + decalage, direction);
        AnimationManager.SetAnimation("slash");
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