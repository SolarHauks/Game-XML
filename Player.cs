using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace JeuVideo;

// Représente le joueur
// Continent le code spécifique au joueur
public class Player : GameObject
{
    private readonly GraphicsDeviceManager _graphics;
    
    private bool _grounded; // Si le joueur est au sol
    
    private List<Rectangle> _intersections; // Liste des intersections avec les tiles, utile pour les collisions
    private KeyboardState _prevKeystate; // Etat du clavier à la frame d'avant
    
    public Player(Texture2D texture, Vector2 position, int size, GraphicsDeviceManager graphics) 
        : base(texture, position, size) {
        Velocity = new Vector2();
        _graphics = graphics;
        _grounded = false;
        Direction = -1;
        _intersections = [];
    }
    
    public void Update(KeyboardState keystate, Tile tile, GameTime gameTime) {
        
        // Déplacements
        
        // Delta time, temps depuis la dernière frame
        // Sert pour que le jeu soit indépendant de la vitesse de la machine
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        
        Velocity.X = 0.0f;  // Reset la vitesse horizontale, supprime l'inertie
        Velocity.Y += 35.0f * dt;   // Gravité
        
        Velocity.Y = Math.Min(25.0f, Velocity.Y);   // Limite la vitesse de chute

        float horizontalSpeed = 300 * dt;   // Vitesse horizontale
        
        // Déplacements horizontaux
        if (keystate.IsKeyDown(Keys.Left)) {
            Velocity.X = -horizontalSpeed;  // Vitesse horizontale
            Direction = -1; // Direction
        }
        
        if (keystate.IsKeyDown(Keys.Right)) {
            Velocity.X = horizontalSpeed; // Vitesse horizontale
            Direction = 1; // Direction
        }
        
        // Jump
        // Si le joueur est au sol et que la touche espace est pressée -> on saute
        // Evite de sauter quand on est dans les airs
        if (_grounded && keystate.IsKeyDown(Keys.Space) && !_prevKeystate.IsKeyDown(Keys.Space)) {
            Velocity.Y = -600 * dt;
        }
        
        Position.X += (int)Velocity.X;  // Déplacement horizontal
        _intersections = GetIntersectingTilesHorizontal(Rect);  // Récupère les tiles intersectés par le joueur

        // Repositionnement du joueur selon les collisions
        // Pour chaque tile que le joueur intersect, on vérifit si il y a collision avec une tile du layer 'collisions'
        // Si c'est la cas, on replace le joueur
        foreach (var rect in _intersections)
        {
            if (tile.Collisions.TryGetValue(new Vector2(rect.X, rect.Y), out _))
            {
                Rectangle collision = new Rectangle(
                    rect.X * 16,
                    rect.Y * 16,
                    16,
                    16
                );

                if (Rect.Intersects(collision))
                {
                    continue;
                }

                if (Velocity.X > 0.0f)
                {
                    Position.X = collision.Left - Size;
                }
                else if (Velocity.X < 0.0f)
                {
                    Position.X = collision.Right;
                }
            }
        }

        Position.Y += (int)Velocity.Y; // Déplacement vertical
        _intersections = GetIntersectingTilesVertical(Rect); // Récupère les tiles intersectés par le joueur

        _grounded = false;
        // Pour chaque tile que le joueur intersect, on vérifit si il y a collision avec une tile du layer 'collisions'
        // Si c'est la cas, on replace le joueur
        foreach (var rect in _intersections)
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
                    Position.Y = collision.Top - Size;
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
        
        _prevKeystate = keystate; // Sauvegarde l'état du clavier pour la frame suivante
        
        // Limites
        CheckLimits(); // Vérifie que le joueur ne sorte pas de l'écran
    }

    // Vérifie que le joueur ne sorte pas de l'écran
    private void CheckLimits()
    {
        // Limite horizontale
        // Si le joueur dépasse à droite ou à gauche, on le replace
        if (Position.X > _graphics.PreferredBackBufferWidth - Texture.Width / 2.0f)
        {
            Position.X = _graphics.PreferredBackBufferWidth - Texture.Width / 2.0f;
        }
        else if (Position.X < Texture.Width / 2.0f)
        {
            Position.X = Texture.Width / 2.0f;
        }
        
        // Limite verticale
        // Si le joueur dépasse en haut ou en bas, on le replace
        if (Position.Y > _graphics.PreferredBackBufferHeight - Texture.Height / 2.0f)
        {
            Position.Y = _graphics.PreferredBackBufferHeight - Texture.Height / 2.0f;
        }
        else if (Position.Y < Texture.Height / 2.0f)
        {
            Position.Y = Texture.Height / 2.0f;
        }
    }
    
    // Récupère les tiles intersectés par le joueur en horizontal
    // On calcule les tiles intersectées par le joueur en fonction de sa taille, et on en renvoit la liste
    private List<Rectangle> GetIntersectingTilesHorizontal(Rectangle target)
    {
        List<Rectangle> intersections = new();

        int widthInTiles = (target.Width - (target.Width % 16)) / 16;   // Largeur en tiles
        int heightInTiles = (target.Height - (target.Height % 16)) / 16;    // Hauteur en tiles

        // Remplis la liste des tiles intersectées par le joueur
        for (int x = 0; x <= widthInTiles; x++) {
            for (int y = 0; y <= heightInTiles; y++) {
                
                intersections.Add(new Rectangle(
                    (target.X + x * 16) / 16,
                    (target.Y + y * (16 - 1)) / 16,
                    16,
                    16
                ));
                
            }
        }

        return intersections;
    }

    // Récupère les tiles intersectés par le joueur en vertical
    // On calcule les tiles intersectées par le joueur en fonction de sa taille, et on en renvoit la liste
    private List<Rectangle> GetIntersectingTilesVertical(Rectangle target)
    {
        List<Rectangle> intersections = new();

        int widthInTiles = (target.Width - (target.Width % 16)) / 16;   // Largeur en tiles
        int heightInTiles = (target.Height - (target.Height % 16)) / 16;    // Hauteur en tiles

        // Remplis la liste des tiles intersectées par le joueur
        for (int x = 0; x <= widthInTiles; x++) {
            for (int y = 0; y <= heightInTiles; y++) {

                intersections.Add(new Rectangle(
                    (target.X + x * (16 - 1)) / 16,
                    (target.Y + y * 16) / 16,
                    16,
                    16
                ));

            }
        }

        return intersections;
    }
}