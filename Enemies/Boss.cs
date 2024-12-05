﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JeuVideo.Enemies;

public class Boss(Texture2D texture, Vector2 position, int maxHealth, Player player, Texture2D summonTexture)
    : Enemy(texture, position, maxHealth, 0.7f)
{
    private int _counter; // Compteur pour les animations
    private int _interval;    // Interval pour le comportement spécial
    private const int Distance = 50;    // Distance d'attaque
    private double _lastAttackTime;     // Temps de la dernière attaque
    private bool _isBloqued;    // Indique si le boss est bloqué
    
    private readonly List<BossSummon> _summons = new(); // Liste des invocations du boss

    // Joueur

    public enum BossState
    {
        Normal,
        Special,
        Dying,
        Attacking
    }

    // On passe par une enum pour éviter que plusieurs états ne puissent etre actifs en meme temps, et par propreté
    public BossState CurrentState { get; private set; }

    protected override void DeplacementHorizontal(double dt)
    {
        // Si le boss est en train de mourir, attaque ou fait son spécial => on ne fait rien
        if (_isBloqued) return;
        
        Velocity.X = Direction * 20 * (float)dt;
        Position.X += Velocity.X;
        // Si on dépasse de 50 pixels de la position de départ, on change de direction
        if (Math.Abs(Position.X - StartPosition.X) > 50) { Direction *= -1; }
    }
    protected override void DeplacementVertical(double dt)
    {
        // Si le boss est en train de mourir, attaque ou fait son spécial => on ne fait rien
        if (_isBloqued) return;
        
        Velocity.Y = 25.0f * (float)dt;   // Gravité
        Position.Y += Velocity.Y;
    }
    
    protected override void Animate(Vector2 velocity)
    {
        if (_isBloqued)
        {
            Bloqued();
        }
        else if (Health <= 0)    // Comportement de mort
        {
            DeathAnim();
        }
        else if (_interval==6*60) // Comportement spécial, lancer toutes les 7 secondes
        {
            SpecialAnim();
        }
        // Comportement d'attaque. Ne se fait que si le joueur est à portée
        else if (Vector2.Distance(player.Position, Position) < Distance)
        {
            AttackAnim();
        }
        else  // Reste des comportements
        {
            AnimationManager.SetAnimation("fly");
            _interval++;
        }
    }

    private void Bloqued()
    {
        _counter++;
        if (CurrentState == BossState.Dying && _counter > 268 || 
            CurrentState == BossState.Attacking && _counter > 15 * 13 || 
            CurrentState == BossState.Special && _counter > 15 * 12)
        {
            CurrentState = BossState.Normal;
            _isBloqued = false;
            _counter = _interval = 0;
        }
        else if (CurrentState == BossState.Normal)
        {
            Console.WriteLine("Erreur: boss bloqué mais aucun état actif");
        }
    }
    
    private void DeathAnim()
    {
        // Lancement de l'animation de mort, stop des autres comportements
        AnimationManager.SetAnimation("death");
        CurrentState = BossState.Dying;
        _isBloqued = true;
    }

    private void SpecialAnim()
    {
        // Lancement de l'animation spéciale, stop des autres comportements
        AnimationManager.SetAnimation("summon");
        CurrentState = BossState.Special;
        _isBloqued = true;
        Vector2 position = new Vector2(Position.X + 50, Position.Y);
        _summons.Add(new BossSummon(summonTexture, position, player.Position));
    }

    private void AttackAnim()
    {
        // Cooldown de l'attaque
        double currentTime = Globals.GameTime.TotalGameTime.TotalSeconds;
        if (currentTime - _lastAttackTime > 3)
        {
            // Lancement de l'animation d'attaque, stop des autres comportements
            AnimationManager.SetAnimation("attack");
            CurrentState = BossState.Attacking;
            _isBloqued = true;
            _lastAttackTime = currentTime;
        }
    }
    
    public override void Update(Dictionary<Vector2, int> collision)
    {
        base.Update(collision);
        
        // On update les invocations
        foreach (BossSummon summon in _summons.ToList())
        {
            summon.Update(collision);
            summon.CheckCollisionWithPlayer(player);
            if (!summon.IsAlive) { _summons.Remove(summon); }
        }
    }
    
    public override void Draw(Vector2 offset)
    {
        base.Draw(offset);
        
        // On dessine les invocations
        foreach (BossSummon summon in _summons)
        {
            summon.Draw(offset);
        }
    }
    
}