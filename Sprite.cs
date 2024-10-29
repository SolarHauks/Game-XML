using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JeuVideo;

// Représente un objet graphique
// S'occupe de tout ce qui est en rapport avec l'affichage
public class Sprite {
    
    protected Vector2 Position; // Position
    protected int Size; // Taille
    
    // attribut calculé, rectangle de l'objet
    protected Rectangle Rect => new Rectangle((int) Position.X, (int)Position.Y, Size, Size);
    
    // Direction (dans le sens du côté dans lequel il regarde)
    protected int Direction { get; set; } // -1 for left, 1 for right

    protected Texture2D Texture { get; } // Texture de l'objet
    
    protected Sprite(Texture2D texture, Vector2 position, int size) {
        Texture = texture;
        Position = position;
        this.Size = size;
    }
    
    public void Draw(SpriteBatch spriteBatch)
    {
        // Si on regarde à gauche, retourne l'image horizontalement. Sinon la laisse telle quelle
        SpriteEffects spriteEffect = (Direction == -1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
        
        // var origin = new Vector2(Texture.Width / 2f, Texture.Height / 2f); // A garder pour plus tard peut etre
        Vector2 origin = Vector2.Zero; // temporaire
        spriteBatch.Draw( Texture, // Texture2D,
            Rect, // Rectangle destinationRectangle,
            null, // Nullable<Rectangle> sourceRectangle,
            Color.White, //  Color,
            0.0f, //  float rotation,
            origin,  // Vector2 origin,
            spriteEffect, // SpriteEffects effects,
            0f ); // float layerDepth
    }
    
}
