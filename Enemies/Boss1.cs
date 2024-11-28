using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JeuVideo.Enemies;

public class Boss1(Texture2D texture, Vector2 position, int maxHealth, Player player) : Enemy(texture, position, maxHealth)
{
    public bool IsDying { get; private set; }
    public bool IsAttacking { get; private set; }
    public bool IsSpecial { get; private set; }
    private int _deathCounter;
    private int _specialCounter;
    private int _attackCounter; // Compteur pour l'attaque
    private int _interval=0;    //interval pour le comportement spécial
    private int distance = 100;
    private double _time=0;




    protected override void DeplacementHorizontal(double dt)
    {
        if (IsDying) return;
        if (IsSpecial) return;
        if (IsAttacking) return;

        
        Velocity.X = Direction * 20 * (float)dt;
        Position.X += Velocity.X;
        if (Position.X > StartPosition.X + 50)
        {
            Direction = -1;
        } else if (Position.X < StartPosition.X - 50)
        {
            Direction = 1;
        }
    }
    protected override void DeplacementVertical(double dt)
    {
        if (IsDying) return;
        if (IsSpecial) return;
        if (IsAttacking) return;

        
        Velocity.Y = 25.0f * (float)dt;   // Gravité
        Position.Y += Velocity.Y;
    }
    
    protected override void Animate(Vector2 velocity)
    {
        if (Health <= 0)
        {
            // Gestion de l'animation de mort
            if (!IsDying)
            {
                // Lancement de l'animation de mort, stop des autres comportements
                AnimationManager.SetAnimation("death");
                IsDying = true;
            } 
            else
            {
                // Compteur pour laisser le temps à l'animation
                // Ici ce n'est pas fait très proprement, il faudrait tenir compte du deltaTime (ou faire avec le temps passé)
                // Solution temporaire, peut etre à revoir
                _deathCounter++;
                if (_deathCounter >= 269) //15*nb de frames de l'animation ()
                {
                    // Fin de l'animation de mort, on supprime le boss de la liste d'ennemis (fait dans Game1.cs)
                    IsDying = false;
                }
            }
        } 
        //On lance l'animation du spécial toutes les 7 secondes
        else if (_interval==7*60) // Comportement spécial
        {
            // Gestion de l'animation spéciale
            if (!IsSpecial)
            {
                // Lancement de l'animation spéciale, stop des autres comportements
                AnimationManager.SetAnimation("special");
                IsSpecial = true;
            }
            else
            {
                // Compteur pour laisser le temps à l'animation
                _specialCounter++;
                if (_specialCounter > 15 * 12)
                {
                    IsSpecial = false;
                    _specialCounter = 0;
                    _interval = 0;
                }
            }
        }
        //Pour l'attaque on fait en sorte qu'il ne la fasse que si le joueur est à portée
        else if (Math.Abs(player.Position.X - Position.X) < /*Globals.ScreenSize.X*/ distance && Math.Abs(player.Position.Y - Position.Y) < /*Globals.ScreenSize.Y*/ distance)
        {
            if (Globals.GameTime.TotalGameTime.TotalSeconds-_time>3)
            {
                if (!IsAttacking)
                {
                    // Lancement de l'animation d'attaque, stop des autres comportements
                    AnimationManager.SetAnimation("attack");
                    IsAttacking = true;
                    _time = Globals.GameTime.TotalGameTime.TotalSeconds;

                }
                else
                {
                    _attackCounter++;
                    if (_attackCounter > 15 * 13)
                    {
                        IsAttacking = false;
                        _attackCounter = 0;

                    }
                }
            }
        }
        else  // Reste des comportements
        {
            AnimationManager.SetAnimation("fly");
            _interval++;

        }
    }
    
}