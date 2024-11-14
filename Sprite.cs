using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JeuVideo;

// Représente un objet graphique
// S'occupe de tout ce qui est en rapport avec l'affichage
public class Sprite {
    
    protected Vector2 Position; // Position
    protected readonly int HorizontalSize; // Taille horizontale
    protected readonly int VerticalSize; // Taille verticale
    private readonly int _displayHorizontalSize; // Taille horizontale
    private readonly int _displayVerticalSize; // Taille verticale
    private Vector2 _size; // Taille d'une frame de l'objet
    
    protected AnimationManager AnimationManager; // Gestionnaire d'animations
    
    // attribut calculé, rectangle de l'objet. Sert pour l'affichage et pour les collisions (= hitbox)
    public Rectangle Rect => new Rectangle((int)Position.X, (int)Position.Y, HorizontalSize, VerticalSize);
    private Rectangle DispRect => new Rectangle((int)Position.X, (int)Position.Y, _displayHorizontalSize, _displayVerticalSize);
    
    // Direction (dans le sens du côté dans lequel il regarde)
    protected int Direction { get; set; } // -1 for left, 1 for right

    protected Texture2D Texture { get; } // Texture de l'objet
    
    protected Sprite(Texture2D texture, Vector2 position) {
        Texture = texture;
        AnimationManager = new AnimationManager(texture);
        Position = position;
        HorizontalSize = Texture.Width;
        _displayHorizontalSize = (((int)_size.X / 16) + 1) * 16; 
        VerticalSize = Texture.Height;
        _displayVerticalSize = (((int)_size.Y / 16) + 1) * 16;
        _size = AnimationManager.GetSize();
    }
    
    public void Draw(SpriteBatch spriteBatch, Vector2 offset)
    {
        // Si on regarde à gauche, retourne l'image horizontalement. Sinon la laisse telle quelle
        SpriteEffects spriteEffect = (Direction == -1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

        // Rectangle de destination, -> position du joueur + décalage lié à la caméra
        Rectangle dRect = DispRect;
        dRect.Offset(offset);
        
        if(this is Enemy)
            Console.WriteLine(dRect);
        
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
