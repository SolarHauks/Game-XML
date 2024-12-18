using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using JeuVideo.Animation;
using JeuVideo.Effects;
using JeuVideo.Enemies;
using Microsoft.Xna.Framework;

namespace JeuVideo.Character;

// Classe permettant de gérer les attaques du joueur
[Serializable]
[XmlRoot("attack", Namespace = "https://www.univ-grenoble-alpes.fr/jeu/character")]
public class AttackManager
{
    [XmlIgnore] private double _lastAttackTime; // Moment de la dernière attaque
    [XmlIgnore] public EffectsManager EffectsManager;   // Public car doit etre set de player
    [XmlIgnore] private AnimationManager _animationManager;
    
    [XmlElement("attackCooldown")] public float AttackCooldown; // Temps entre chaque attaque
    [XmlElement("attackHitbox")] public int AttackHitbox;   // Taille de la hitbox de l'attaque
    [XmlElement("attackDamage")] public int AttackDamage;   // Dégats de l'attaque
    [XmlElement("specialHitbox")] public int SpecialHitbox; // Taille de la hitbox du spécial
    [XmlElement("specialDamage")] public int SpecialDamage; // Dégats du spécial
    [XmlElement("specialCost")] public int SpecialCost;    // Coût en mana du spécial
    
    public void Load(AnimationManager animationManager, EffectsManager effectsManager)
    {
        _animationManager = animationManager;
        EffectsManager = effectsManager;
        _lastAttackTime = -AttackCooldown;  // Permet de pouvoir attaquer dès le début
    }

    // Vérifie si le joueur peut attaquer
    public bool CanAttack(int currentMana = 0, bool isSpecial = false)
    {
        double currentTime = Globals.GameTime.TotalGameTime.TotalSeconds;
        return currentTime - _lastAttackTime >= AttackCooldown && currentMana >= (isSpecial ? SpecialCost : 0);
    }

    // Attaque de base
    public void Attack(List<Enemy> enemies, Vector2 position, int direction)
    {
        // La vérification de la possibilité d'attaque est faite dans player
        
        // Hitbox de l'attaque
        Rectangle hitbox = new Rectangle(
            (int)position.X + (direction == 1 ? 16 : -16),
            (int)position.Y,
            AttackHitbox,
            32
        );

        ApplyDamage(enemies, hitbox, AttackDamage); // Applique les dégats aux ennemis
        EffectsManager.PlayEffect("slash", position, direction);    // Joue l'effet de slash
        _animationManager.SetAnimation("slash");    // Joue l'animation de l'attaque sur le joueur
        _lastAttackTime = Globals.GameTime.TotalGameTime.TotalSeconds;  // Met à jour le moment de la dernière attaque
    }

    // Attaque spéciale
    public void SpecialAttack(List<Enemy> enemies, Vector2 position, int direction)
    {
        // La vérification de la possibilité d'attaque est faite dans player
        
        // Vérifie si le joueur peut attaquer
        Rectangle hitbox = new Rectangle(
            (int)position.X + (direction == 1 ? 16 : -48),
            (int)position.Y,
            SpecialHitbox,
            32
        );

        ApplyDamage(enemies, hitbox, SpecialDamage);
        Vector2 decalage = new Vector2((direction == 1 ? 32 : -32), 0); // décalage de l'effet de slash
        // L'effet de slash est décalé pour le spécial, pour marqué la différence
        EffectsManager.PlayEffect("slash", position + decalage, direction);
        _animationManager.SetAnimation("slash");    // Joue l'animation de l'attaque sur le joueur
        _lastAttackTime = Globals.GameTime.TotalGameTime.TotalSeconds;  // Met à jour le moment de la dernière attaque
    }
    
    // Applique les dégats aux ennemis présent dans la hitbox
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