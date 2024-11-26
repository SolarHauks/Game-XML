using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace JeuVideo;

// Classe statique contenant des variables globales pour le jeu.
// Permet leur accès dans l'entièreté du projet sans avoir à faire circuler en paramètre.
public static class Globals
{
    public static ContentManager Content { get; set; }  // Permet de charger des textures
    public static SpriteBatch SpriteBatch { get; set; } // Utile dans tous les Draw()
    public static GameTime GameTime { get; set; }   // Pour deltaTime
    public static GraphicsDevice GraphicsDevice { get; set; }  // Pour les textures
    public static Vector2 ScreenSize { get; set; }  // Taille de l'écran
    
    // Texture utilisée pour le debug, reglangle rouge
    // Sert seulement pour la fonction DrawRectHollow
    public static Texture2D DebugTexture { get; set; }  
    
    // Fonction de debug, dessine les contours d'un rectangle passé en paramètre
    public static void DrawRectHollow(Rectangle rect) {
        SpriteBatch spriteBatch = Globals.SpriteBatch;
        Texture2D texture = Globals.DebugTexture;
        int thickness = 1;
        
        spriteBatch.Draw(
            texture,
            new Rectangle(
                rect.X,
                rect.Y,
                rect.Width,
                thickness
            ),
            Color.White
        );
        spriteBatch.Draw(
            texture,
            new Rectangle(
                rect.X,
                rect.Bottom - thickness,
                rect.Width,
                thickness
            ),
            Color.White
        );
        spriteBatch.Draw(
            texture,
            new Rectangle(
                rect.X,
                rect.Y,
                thickness,
                rect.Height
            ),
            Color.White
        );
        spriteBatch.Draw(
            texture,
            new Rectangle(
                rect.Right - thickness,
                rect.Y,
                thickness,
                rect.Height
            ),
            Color.White
        );
    }
}