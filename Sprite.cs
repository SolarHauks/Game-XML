using System;
using JeuVideo.Animation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace JeuVideo;

// Représente un objet graphique
// S'occupe de tout ce qui est en rapport avec l'affichage
public abstract class Sprite {
    
    private readonly Vector2 _displaySize; // Taille affichée
    protected Vector2 Position; // Position
    
    private Texture2D Texture { get; } // Texture de l'objet
    protected readonly AnimationManager AnimationManager; // Gestionnaire d'animations
    
    // Direction (dans le sens du côté dans lequel il regarde)
    protected int Direction { get; set; } // -1 for left, 1 for right
    
    
    protected Sprite(Texture2D texture, Vector2 position) {
        Texture = texture;
        Position = position;    // Position initiale de l'objet
        Direction = 1;
        
        AnimationManager = new AnimationManager(texture);
        Vector2 size = AnimationManager.GetSize();
        _displaySize = new Vector2((int)(Math.Ceiling(size.X / 16.0) * 16), (int)(Math.Ceiling(size.Y / 16.0) * 16));
    }
    
    public void Draw(SpriteBatch spriteBatch, Vector2 offset)
    {
        // Si on regarde à gauche, retourne l'image horizontalement. Sinon la laisse telle quelle
        SpriteEffects spriteEffect = (Direction == -1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

        // Rectangle de destination, -> position du joueur + décalage lié à la caméra
        Vector2 displayPosition = Position + offset;
        Rectangle dRect = new Rectangle(
            (int)displayPosition.X, 
            (int)displayPosition.Y, 
            (int)_displaySize.X, 
            (int)_displaySize.Y);
        
        Rectangle sRect = AnimationManager.GetSourceRectangle();
        
        Vector2 origin = Vector2.Zero;
        
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
