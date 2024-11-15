using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace JeuVideo;

/// Représente un joueur dans le jeu.
/// Hérite de la classe GameObject.
public class Player : GameObject
{
    private KeyboardState _keystate;
    
    private bool _grounded; // Si le joueur est au sol
    
    private KeyboardState _prevKeystate; // Etat du clavier à la frame d'avant
    
    public Player(Texture2D texture, Vector2 position) : base(texture, position) {
        Velocity = new Vector2();
        _grounded = false;
    }
    
    /// Met à jour l'état du joueur.
    /// param keystate : L'état actuel du clavier.
    /// param tile : Les informations sur les tiles pour la détection des collisions.
    /// param gameTime : Le temps écoulé depuis la dernière frame.
    public void Update(KeyboardState keystate, Tile tile, GameTime gameTime, List<Enemy> enemies) {
        
        _keystate = keystate; // Sauvegarde l'état du clavier (ie : les touches actuellement pressées)
        
        base.Update(tile, gameTime); // Met à jour la position du joueur
        
        if (_keystate.IsKeyDown(Keys.C) && !_prevKeystate.IsKeyDown(Keys.C)) {
            Attack(enemies);
        }
        
        if (_keystate.IsKeyDown(Keys.Z) && !_prevKeystate.IsKeyDown(Keys.Z))
        {
            Position.X = 10;
            Position.Y = 10 ;
        }
        
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
                    Position.Y = collision.Top - VerticalSize;
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
        
        // Déplacements horizontaux
        if (_keystate.IsKeyDown(Keys.Left)) {
            Velocity.X = -horizontalSpeed;  // Vitesse horizontale
            Direction = -1; // Direction
        }
        
        if (_keystate.IsKeyDown(Keys.Right)) {
            Velocity.X = horizontalSpeed; // Vitesse horizontale
            Direction = 1; // Direction
        }
        
        Position.X += (int)Velocity.X;  // Déplacement horizontal
    }

    protected override void DeplacementVertical(float dt)
    {
        // Gestion des déplacements horizontaux : saut et gravité
        Velocity.Y += 35.0f * dt;   // Gravité
        Velocity.Y = Math.Min(25.0f, Velocity.Y);   // Limite la vitesse de chute
        
        // Si le joueur est au sol et que la touche espace est pressée -> on saute
        // Evite de sauter quand on est dans les airs
        if (_grounded && _keystate.IsKeyDown(Keys.Space) && !_prevKeystate.IsKeyDown(Keys.Space)) {
            Velocity.Y = -600 * dt;
        }
        
        Position.Y += (int)Velocity.Y; // Déplacement vertical
    }

    private void Attack(List<Enemy> enemies)
    {
        Console.WriteLine("test");
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
    }
    
}