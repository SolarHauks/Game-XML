using System;
using System.Collections.Generic;
using JeuVideo.Effects;
using JeuVideo.Enemies;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace JeuVideo;

/// Représente un joueur dans le jeu.
/// Hérite de la classe GameObject.
public class Player : GameObject
{
    private bool _grounded; // Si le joueur est au sol
    
    private KeyboardState _prevKeystate; // Etat du clavier à la frame d'avant
    
    private readonly EffectsManager _effectsManager; // Gestionnaire des effets
    
    private double _lastAttackTime; // Temps de la dernière attaque
    private const double AttackCooldown = 0.5; // Cooldown de l'attaque
    
    private double _lastDamageTime; // Temps du dernier dégât
    
    private readonly int _maxHealth;
    private int _currentHealth;
    
    private readonly int _maxMana;
    private int _currentMana;
    private int Health
    {
        get => _currentHealth;
        set => _currentHealth = Math.Clamp(value, 0, _maxHealth);
    }
    
    public Player(Texture2D texture, Vector2 position, EffectsManager effets) : base(texture, position, true) {
        Velocity = new Vector2();
        _grounded = false;
        _effectsManager = effets;
        
        _maxHealth = 100;
        _currentHealth = _maxHealth;
        
        _maxMana = 100;
        _currentMana = _maxMana;
        
        _lastAttackTime = -AttackCooldown; // Initialise le temps de la dernière attaque pour pouvoir attaquer dès le début
        _lastDamageTime = 0;
    }
    
    /// Met à jour l'état du joueur.
    /// param keystate : L'état actuel du clavier.
    /// param tile : Les informations sur les tiles pour la détection des collisions.
    /// param gameTime : Le temps écoulé depuis la dernière frame.
    public void Update(Dictionary<Vector2, int> collision, List<Enemy> enemies) {
        
        KeyboardState keystate = Keyboard.GetState();    // Récupère l'état du clavier (ie : les touches actuellement pressées)
        
        base.Update(collision); // Met à jour la position du joueur
        
        // Attaque
        if (keystate.IsKeyDown(Keys.C) && !_prevKeystate.IsKeyDown(Keys.C)) {
            Attack(enemies);
        }
        
        // Attaque Spé
        if (keystate.IsKeyDown(Keys.V) && !_prevKeystate.IsKeyDown(Keys.V)) {
            SpecialAttack(enemies);
        }
        
        // Reset de la position du joueur, uniquement pour les tests
        if (keystate.IsKeyDown(Keys.A) && !_prevKeystate.IsKeyDown(Keys.A))
        {
            Position.X = 20;
            Position.Y = 10 ;
        }
        
        Animate(Velocity); // Gère l'animation du joueur
        
        TakeDamage(enemies); // Gère les dégâts du joueur
        
        _prevKeystate = keystate; // Sauvegarde l'état du clavier pour la frame suivante
    }

    // On réimplemente la détection verticales des collisions pour le joueur pour integrer le saut
    protected override void CheckCollisionsVertical(Dictionary<Vector2, int> collision)
    {
        _grounded = false;
        List<Rectangle> intersections = GetIntersectingTilesVertical(Rect); // Récupère les tiles intersectés par le joueur

        // Pour chaque tile que le joueur intersect, on vérifit si il y a collision avec une tile du layer 'collisions'
        // Si c'est la cas, on replace le joueur
        foreach (var rect in intersections)
        {
            if (collision.TryGetValue(new Vector2(rect.X, rect.Y), out _))
            {
                Rectangle collisionTile = new Rectangle(
                    rect.X * 16,
                    rect.Y * 16,
                    16,
                    16
                );

                if (!Rect.Intersects(collisionTile))
                {
                    continue;
                }

                // colliding with the top face
                if (Velocity.Y > 0.0f)
                {
                    Position.Y = collisionTile.Top - Rect.Height;
                    Velocity.Y = 1.0f; // counter snap to ground
                    _grounded = true;
                }
                else if (Velocity.Y < 0.0f)
                {
                    Position.Y = collisionTile.Bottom;
                    Velocity.Y = 0.0f;
                }
            }
        }
    }
    
    protected override void DeplacementHorizontal(double dt)
    {
        Velocity.X = 0.0f;  // Reset la vitesse horizontale, supprime l'inertie
        double horizontalSpeed = 250 * dt;   // Vitesse horizontale
        KeyboardState keystate = Keyboard.GetState();
        
        // Déplacements horizontaux
        if (keystate.IsKeyDown(Keys.Left)) {
            Velocity.X = (float)-horizontalSpeed;  // Vitesse horizontale
            Direction = -1; // Direction
        }
        
        if (keystate.IsKeyDown(Keys.Right)) {
            Velocity.X = (float)horizontalSpeed; // Vitesse horizontale
            Direction = 1; // Direction
        }
        
        Position.X += (int)Velocity.X;  // Déplacement horizontal
    }

    protected override void DeplacementVertical(double dt)
    {
        KeyboardState keystate = Keyboard.GetState();
        
        // Gestion des déplacements horizontaux : saut et gravité
        Velocity.Y += 35.0f * (float)dt;   // Gravité
        Velocity.Y = Math.Min(25.0f, Velocity.Y);   // Limite la vitesse de chute
        
        // Si le joueur est au sol et que la touche espace est pressée -> on saute
        // Evite de sauter quand on est dans les airs
        if (_grounded && keystate.IsKeyDown(Keys.Space) && !_prevKeystate.IsKeyDown(Keys.Space)) {
            Velocity.Y = -600 * (float)dt;
        }
        
        Position.Y += (int)Velocity.Y; // Déplacement vertical
    }
    
    // Gestion des attaques du joueur
    private void Attack(List<Enemy> enemies)
    {
        double currentTime = Globals.GameTime.TotalGameTime.TotalSeconds;
        if (currentTime - _lastAttackTime < AttackCooldown)
        {
            return;
        }
        
        _lastAttackTime = currentTime;  // Mise à jour du temps de la dernière attaque
        
        Rectangle hitbox = new Rectangle(
            (int)Position.X + (Direction == 1 ? 16 : -16),
            (int)Position.Y,
            32,
            32
        );

        foreach (var enemy in enemies)
        {
            if (hitbox.Intersects(enemy.Rect))
            {
                enemy.TakeDamage(20, Position);
            }
        }
        
        _effectsManager.PlayEffect("slash", Position, Direction);
        AnimationManager.SetAnimation("slash");
    }
    
    private void SpecialAttack(List<Enemy> enemies)
    {
        double currentTime = Globals.GameTime.TotalGameTime.TotalSeconds;
        if (currentTime - _lastAttackTime < AttackCooldown || _currentMana < 20)
        {
            return;
        }
        
        _lastAttackTime = currentTime;  // Mise à jour du temps de la dernière attaque

        _currentMana = Math.Max(0, _currentMana - 20);
        
        Rectangle hitbox = new Rectangle(
            (int)Position.X + (Direction == 1 ? 16 : -16),
            (int)Position.Y,
            64,
            64
        );

        foreach (var enemy in enemies)
        {
            if (hitbox.Intersects(enemy.Rect))
            {
                enemy.TakeDamage(50, Position);
            }
        }
        
        _effectsManager.PlayEffect("slash", Position + new Vector2(32,0), Direction);
        AnimationManager.SetAnimation("slash");
    }

    // Gestion des animations du joueur
    protected override void Animate(Vector2 velocity)
    {
        string currentAnim = AnimationManager.GetCurrentAnimation();
        
        if (currentAnim == "slash" && AnimationManager.IsPlaying())
        {
            return; // Do not change animation if attack is playing
        }

        if (_grounded)
        {
            if (velocity.X != 0 && currentAnim != "run") {
                AnimationManager.SetAnimation("run");
            } else if (velocity.X == 0 && currentAnim != "idle") {
                AnimationManager.SetAnimation("idle");
            }
        }
        else
        {
            string newAnim = velocity.Y > 1 ? "fall" : "jump";
            if (currentAnim != newAnim) {
                AnimationManager.SetAnimation(newAnim);
            }
        }
    }
    
    private void TakeDamage(List<Enemy> enemies)
    {
        foreach (Enemy enemy in enemies)
        {
            if (Rect.Intersects(enemy.Rect) && Globals.GameTime.TotalGameTime.TotalSeconds - _lastDamageTime > 1)
            {
                Health -= 20;
                _lastDamageTime = Globals.GameTime.TotalGameTime.TotalSeconds;
                int attackDirection = Position.X < enemy.Rect.X ? -1 : 1;
                Position.X += attackDirection * 20;
                Console.Out.WriteLine("Player hit! Health: " + Health);
            }
        }
    }
    
}