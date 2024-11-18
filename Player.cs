using System;
using System.Collections.Generic;
using JeuVideo.Effects;
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
    
    private EffectsManager _effectsManager; // Gestionnaire des effets
    
    public Player(Texture2D texture, Vector2 position, EffectsManager effets) : base(texture, position) {
        Velocity = new Vector2();
        _grounded = false;
        _effectsManager = effets;
    }
    
    /// Met à jour l'état du joueur.
    /// param keystate : L'état actuel du clavier.
    /// param tile : Les informations sur les tiles pour la détection des collisions.
    /// param gameTime : Le temps écoulé depuis la dernière frame.
    public void Update(Tile tile, GameTime gameTime, List<Enemy> enemies) {
        
        KeyboardState keystate = Keyboard.GetState();    // Récupère l'état du clavier (ie : les touches actuellement pressées)
        
        base.Update(tile, gameTime); // Met à jour la position du joueur
        
        if (keystate.IsKeyDown(Keys.C) && !_prevKeystate.IsKeyDown(Keys.C)) {
            Attack(enemies);
        }
        
        // Reset de la position du joueur, uniquement pour les tests
        if (keystate.IsKeyDown(Keys.Z) && !_prevKeystate.IsKeyDown(Keys.Z))
        {
            Position.X = 20;
            Position.Y = 10 ;
        }
        
        Animate(Velocity); // Gère l'animation du joueur
        
        _prevKeystate = keystate; // Sauvegarde l'état du clavier pour la frame suivante
        
        // Limites
        // CheckLimits(); // Vérifie que le joueur ne sorte pas de l'écran. Inutile si on utilise la caméra
    }

    
    // Vérifie que le joueur ne sorte pas des limites de l'écran.
    // Replace le joueur s'il dépasse les limites horizontales ou verticales.
    // A noter que cette fonction est inutile si on utilise la caméra, car le joueur sera alors toujours au centre de l'écran
    // ATTENTION : cette fonction n'a pas été mis à jour et utilise encore les dimensions de la texture pour fonctionner
    // => A maj si on veut l'utliser
    /*private void CheckLimits()
    {
        // Limite horizontale
        // Si le joueur dépasse à droite ou à gauche, on le replace
        if (Position.X > _screenSize.X - Texture.Width / 2.0f)
        {
            Position.X = _screenSize.X - Texture.Width / 2.0f;
        }
        else if (Position.X < Texture.Width / 2.0f)
        {
            Position.X = Texture.Width / 2.0f;
        }
        
        // Limite verticale
        // Si le joueur dépasse en haut ou en bas, on le replace
        if (Position.Y > _screenSize.Y - Texture.Height / 2.0f)
        {
            Position.Y = _screenSize.X - Texture.Height / 2.0f;
        }
        else if (Position.Y < Texture.Height / 2.0f)
        {
            Position.Y = Texture.Height / 2.0f;
        }
    }*/
    
    protected override void CheckCollisionsVertical(Tile tile)
    {
        _grounded = false;
        List<Rectangle> intersections = GetIntersectingTilesVertical(Rect); // Récupère les tiles intersectés par le joueur

        // Pour chaque tile que le joueur intersect, on vérifit si il y a collision avec une tile du layer 'collisions'
        // Si c'est la cas, on replace le joueur
        foreach (var rect in intersections)
        {
            if (tile.Collisions.TryGetValue(new Vector2(rect.X, rect.Y), out _))
            {
                Rectangle collision = new Rectangle(
                    rect.X * 16,
                    rect.Y * 16,
                    16,
                    16
                );

                if (!Rect.Intersects(collision))
                {
                    continue;
                }

                // colliding with the top face
                if (Velocity.Y > 0.0f)
                {
                    Position.Y = collision.Top - Rect.Height;
                    Velocity.Y = 1.0f; // counter snap to ground
                    _grounded = true;
                }
                else if (Velocity.Y < 0.0f)
                {
                    Position.Y = collision.Bottom;
                    Velocity.Y = 0.0f;
                }
            }
        }
    }
    
    protected override void DeplacementHorizontal(float dt)
    {
        Velocity.X = 0.0f;  // Reset la vitesse horizontale, supprime l'inertie
        float horizontalSpeed = 250 * dt;   // Vitesse horizontale
        KeyboardState keystate = Keyboard.GetState();
        
        // Déplacements horizontaux
        if (keystate.IsKeyDown(Keys.Left)) {
            Velocity.X = -horizontalSpeed;  // Vitesse horizontale
            Direction = -1; // Direction
        }
        
        if (keystate.IsKeyDown(Keys.Right)) {
            Velocity.X = horizontalSpeed; // Vitesse horizontale
            Direction = 1; // Direction
        }
        
        Position.X += (int)Velocity.X;  // Déplacement horizontal
    }

    protected override void DeplacementVertical(float dt)
    {
        KeyboardState keystate = Keyboard.GetState();
        
        // Gestion des déplacements horizontaux : saut et gravité
        Velocity.Y += 35.0f * dt;   // Gravité
        Velocity.Y = Math.Min(25.0f, Velocity.Y);   // Limite la vitesse de chute
        
        // Si le joueur est au sol et que la touche espace est pressée -> on saute
        // Evite de sauter quand on est dans les airs
        if (_grounded && keystate.IsKeyDown(Keys.Space) && !_prevKeystate.IsKeyDown(Keys.Space)) {
            Velocity.Y = -600 * dt;
        }
        
        Position.Y += (int)Velocity.Y; // Déplacement vertical
    }

    private void Attack(List<Enemy> enemies)
    {
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
    
}