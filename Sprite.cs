using System;
using JeuVideo.Animation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace JeuVideo;

// Représente un objet graphique
// S'occupe de tout ce qui est en rapport avec l'affichage
public abstract class Sprite {
    
    private readonly int _horizontalSize; // Taille horizontale
    private readonly int _verticalSize; // Taille verticale
    private readonly int _displayHorizontalSize; // Taille horizontale affichée
    private readonly int _displayVerticalSize; // Taille verticale affichée
    protected Vector2 Position; // Position
    
    protected readonly AnimationManager AnimationManager; // Gestionnaire d'animations
    
    // Hitbox de l'objet, sert pour les collisions
    public Rectangle Rect => new Rectangle((int)Position.X, (int)Position.Y, _horizontalSize, _verticalSize);
    
    // Rectangle de destination, sert pour l'affichage
    private Rectangle DispRect => new Rectangle((int)Position.X, (int)Position.Y, _displayHorizontalSize, _displayVerticalSize);
    
    // Direction (dans le sens du côté dans lequel il regarde)
    protected int Direction { get; set; } // -1 for left, 1 for right

    private Texture2D Texture { get; } // Texture de l'objet
    
    protected Sprite(Texture2D texture, Vector2 position) {
        Texture = texture;
        Position = position;    // Position initiale de l'objet
        Direction = 1;
        
        AnimationManager = new AnimationManager(texture);
        Vector2 size = AnimationManager.GetSize();
        _displayHorizontalSize = (int)(Math.Ceiling(size.X / 16.0) * 16);
        _displayVerticalSize = (int)(Math.Ceiling(size.Y / 16.0) * 16);
        
        _horizontalSize = _displayHorizontalSize;
        _verticalSize = _displayVerticalSize;
    }
    
    public void Draw(SpriteBatch spriteBatch, Vector2 offset)
    {
        // Si on regarde à gauche, retourne l'image horizontalement. Sinon la laisse telle quelle
        SpriteEffects spriteEffect = (Direction == -1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

        // Rectangle de destination, -> position du joueur + décalage lié à la caméra
        Rectangle dRect = DispRect;
        dRect.Offset(offset);
        
        Rectangle sRect = AnimationManager.GetSourceRectangle();
        
        // var origin = new Vector2(Texture.Width / 2f, Texture.Height / 2f); // A garder pour plus tard peut etre
        Vector2 origin = Vector2.Zero; // temporaire
        spriteBatch.Draw( 
            Texture, // Texture2D,
            dRect, // Rectangle destinationRectangle,
            sRect, // Nullable<Rectangle> sourceRectangle,
            Color.White, //  Color,
            0.0f, //  float rotation,
            origin,  // Vector2 origin,
            spriteEffect, // SpriteEffects effects,
            0f ); // float layerDepth
    }
    
}
