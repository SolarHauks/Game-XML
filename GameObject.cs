using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JeuVideo;

public abstract class GameObject : Sprite
{
    protected Vector2 Velocity;
    private readonly int _horizontalSize; // Taille horizontale
    private readonly int _verticalSize; // Taille verticale
    
    // Hitbox de l'objet, sert pour les collisions
    public Rectangle Rect => new Rectangle((int)Position.X, (int)Position.Y, _horizontalSize, _verticalSize);
    
    protected GameObject(Texture2D texture, Vector2 position) : base(texture, position) {
        Vector2 size = AnimationManager.GetSize();
        _horizontalSize = (int)(Math.Ceiling(size.X / 16.0) * 16);
        _verticalSize = (int)(Math.Ceiling(size.Y / 16.0) * 16);
    }

    public void Update(Tile tile, GameTime gameTime)
    {
        // Delta time, temps depuis la dernière frame
        // Sert pour que le jeu soit indépendant de la vitesse de la machine
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        
        DeplacementHorizontal(dt);
        CheckCollisionsHorizontal(tile);

        DeplacementVertical(dt);
        CheckCollisionsVertical(tile);

        Animate(Velocity);
        
        AnimationManager.Update();
    }
    
    protected abstract void DeplacementHorizontal(float dt);
    
    protected abstract void DeplacementVertical(float dt);

    private void CheckCollisionsHorizontal(Tile tile)
    {
        // Liste des intersections avec les tiles, utile pour les collisions
        var intersections = GetIntersectingTilesHorizontal(Rect);

        // Repositionnement du joueur selon les collisions
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

                if (Rect.Intersects(collision))
                {
                    continue;
                }

                if (Velocity.X > 0.0f)
                {
                    Position.X = collision.Left - Rect.Width;
                }
                else if (Velocity.X < 0.0f)
                {
                    Position.X = collision.Right;
                }
            }
        }
    }

    protected virtual void CheckCollisionsVertical(Tile tile)
    {
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
                }
                else if (Velocity.Y < 0.0f)
                {
                    Position.Y = collision.Bottom;
                    Velocity.Y = 0.0f;
                }
            }
        }
    }

    // Récupère les tiles intersectées par le joueur en direction horizontale.
    // Calcule les tiles intersectées par le joueur en fonction de sa taille et renvoie la liste.
    // param : Le rectangle représentant la position et la taille actuelle du joueur.
    // retour : Une liste de rectangles représentant les tiles intersectées.
    private static List<Rectangle> GetIntersectingTilesHorizontal(Rectangle target)
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
    
    // Récupère les tiles intersectées par le joueur en direction verticale.
    // Calcule les tiles intersectées par le joueur en fonction de sa taille et renvoie la liste.
    // param : Le rectangle représentant la position et la taille actuelle du joueur.
    // retour : Une liste de rectangles représentant les tiles intersectées.
    protected static List<Rectangle> GetIntersectingTilesVertical(Rectangle target)
    {
        List<Rectangle> intersections = new();

        int widthInTiles = (int)Math.Ceiling(target.Width / 16.0f);   // Largeur en tiles
        int heightInTiles = (int)Math.Ceiling(target.Height / 16.0f);    // Hauteur en tiles

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
    
    // Anime l'objet de jeu
    // <param : velocity - La vélocité de l'objet de jeu.
    protected abstract void Animate(Vector2 velocity);

}