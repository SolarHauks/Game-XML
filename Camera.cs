using Microsoft.Xna.Framework;

namespace JeuVideo;

public class Camera
{
    public Vector2 Position;
    
    public Camera(Vector2 position)
    {
        Position = position;
    }
    
    // Suit la cible spécifiée en ajustant la position de la caméra.
    // Calcule un décalage à appliquer à tous les objets dessinés pour que la cible soit au centre de l'écran.
    // param : target - Le rectangle représentant la cible à suivre.
    // <param : screenSize - La taille de l'écran en tant que vecteur 2D.
    public void Follow(Rectangle target, Vector2 screenSize)
    {
        Position = new Vector2(
            
            -target.X + (screenSize.X / 2) - (target.Width / 2.0f),
            // -target.Y + (screenSize.Y / 2) - (target.Height / 2.0f)
            0
            
        );
    }
}