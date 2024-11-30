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
    
    // Attaques
    private double _lastAttackTime; // Temps de la dernière attaque
    private const double AttackCooldown = 0.5; // Cooldown de l'attaque
    
    // Reception de dégats
    private double _lastDamageTime; // Temps du dernier dégât
    
    // Ressources : vie et mana
    private readonly QuantityBar _healthBar; // Barre de vie
    private readonly QuantityBar _manaBar; // Barre de mana
    
    private readonly int _maxHealth;
    private int _currentHealth;
    
    private readonly int _maxMana;
    private int _currentMana;

    private double _lastRegenTime;
    private int Health
    {
        get => _currentHealth;
        set {
            _currentHealth = Math.Clamp(value, 0, _maxHealth);
            _healthBar.Set(_currentHealth);
        }
    }
    
    private int Mana
    {
        get => _currentMana;
        set {
            _currentMana = Math.Clamp(value, 0, _maxMana);
            _manaBar.Set(_currentMana);
        }
    }
    
    public Player(Texture2D texture, Vector2 position, EffectsManager effets) : base(texture, position, true) {
        Velocity = new Vector2();
        _grounded = false;
        _effectsManager = effets;
        
        _maxHealth = 100;
        _currentHealth = _maxHealth;
        
        _maxMana = 100;
        _currentMana = _maxMana;
        
        _healthBar = new QuantityBar(_maxHealth, Color.Red, new Vector2(10, 10));
        _manaBar = new QuantityBar(_maxMana, Color.Blue, new Vector2(10, 30));
        
        _lastAttackTime = -AttackCooldown; // Initialise le temps de la dernière attaque pour pouvoir attaquer dès le début
        _lastDamageTime = 0;
        
        _lastRegenTime = 0;
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
        
        Regen();    // Gère la régénération du joueur
        
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
    
    // Attaque du joueur
    private void Attack(List<Enemy> enemies)
    {
        if (!CanAttack()) return;

        // Hitbox de l'attaque
        Rectangle hitbox = new Rectangle(
            (int)Position.X + (Direction == 1 ? 16 : -16),
            (int)Position.Y,
            32,
            32
        );

        ApplyDamage(enemies, hitbox, 20);

        // Effet visuel de l'attaque
        _effectsManager.PlayEffect("slash", Position, Direction);
        AnimationManager.SetAnimation("slash");
    }

// Attaque spéciale du joueur, plus puissante mais coutant du mana
    private void SpecialAttack(List<Enemy> enemies)
    {
        if (!CanAttack(20)) return;

        Mana -= 20; // Coût en mana

        // Hitbox de l'attaque
        Rectangle hitbox = new Rectangle(
            (int)Position.X + (Direction == 1 ? 16 : -48),
            (int)Position.Y,
            64,
            32
        );

        ApplyDamage(enemies, hitbox, 50);

        // Effet visuel
        Vector2 decalage = new Vector2((Direction == 1 ? 32 : -32), 0);
        _effectsManager.PlayEffect("slash", Position + decalage, Direction);
        AnimationManager.SetAnimation("slash");
    }

    // Vérifie si le joueur peut attaquer
    private bool CanAttack(int manaCost = 0)
    {
        double currentTime = Globals.GameTime.TotalGameTime.TotalSeconds;
        if (currentTime - _lastAttackTime < AttackCooldown || _currentMana < manaCost) return false;

        _lastAttackTime = currentTime;
        return true;
    }

    // Applique les dégâts aux ennemis dans la hitbox
    private void ApplyDamage(List<Enemy> enemies, Rectangle hitbox, int damage)
    {
        foreach (var enemy in enemies)
        {
            if (enemy is not Spike && hitbox.Intersects(enemy.Rect))
            {
                enemy.TakeDamage(damage, Position);
            }
        }
    }

    // Gestion des animations du joueur
    protected override void Animate(Vector2 velocity)
    {
        string currentAnim = AnimationManager.GetCurrentAnimation();    // Animation en cours de lecture
        
        // On ne change pas d'animation si on est en train de jouer l'attaque
        if (currentAnim == "slash" && AnimationManager.IsPlaying()) { return; }

        if (_grounded) {
            if (velocity.X != 0 && currentAnim != "run") {
                AnimationManager.SetAnimation("run");
            } else if (velocity.X == 0 && currentAnim != "idle") {
                AnimationManager.SetAnimation("idle");
            }
        } else {
            string newAnim = velocity.Y > 1 ? "fall" : "jump";
            if (currentAnim != newAnim) {
                AnimationManager.SetAnimation(newAnim);
            }
        }
    }
    
    // Gère les dégâts subis par le joueur lorsqu'il entre en collision avec un ennemi.
    // enemies - Liste des ennemis présents dans le jeu.
    private void TakeDamage(List<Enemy> enemies)
    {
        foreach (Enemy enemy in enemies)
        {
            // On ne prend pas de dégâts si on vient d'en prendre -> instant d'invulnérabilité
            double currentTime = Globals.GameTime.TotalGameTime.TotalSeconds;
            if (Rect.Intersects(enemy.Rect) && (currentTime - _lastDamageTime > 1))
            {
                if (enemy is Boss)     // Cas du boss
                {
                    if (((Boss) enemy).CurrentState == Boss.BossState.Attacking)
                    {
                        Health -= 35;
                        int attackDirection = Position.X < enemy.Rect.X ? -1 : 1;
                        Position.X += attackDirection * 20;
                        _lastDamageTime = Globals.GameTime.TotalGameTime.TotalSeconds;
                        Console.Out.WriteLine("Player hit! Health: " + Health);
                    }
                }
                else    // Cas des autres ennemis
                {
                    Health -= 20;
                    int attackDirection = Position.X < enemy.Rect.X ? -1 : 1;
                    Position.X += attackDirection * 20;
                    _lastDamageTime = Globals.GameTime.TotalGameTime.TotalSeconds;
                    Console.Out.WriteLine("Player hit! Health: " + Health);
                }
            }
        }
    }
    
    public void TakeDamage(int damage)
    {
        Health -= damage;
    }

    private void Regen()
    {
        double currentTime = Globals.GameTime.TotalGameTime.TotalSeconds;
        if (currentTime - _lastRegenTime > 1)
        {
            _lastRegenTime = currentTime;
            Health += 2;
            Mana += 2;
        }
    }
    
    public override void Draw(Vector2 offset)
    {
        base.Draw(offset);
        _healthBar.Draw();
        _manaBar.Draw();
    }
    
}