using System;
using Microsoft.Xna.Framework;

namespace JeuVideo;

public class Camera(Vector2 position)
{
    public Vector2 Position = position;

    // Suit la cible spécifiée en ajustant la position de la caméra.
    // Calcule un décalage à appliquer à tous les objets dessinés pour que la cible soit au centre de l'écran.
    // On a désactiver ici le suivi vertical
    // param : target - Le rectangle représentant la cible à suivre.
    // <param : screenSize - La taille de l'écran en tant que vecteur 2D.
    public void Follow(Rectangle target, Vector2 screenSize)
    {
        float targetX = -target.X + (screenSize.X / 2) - (target.Width / 2.0f);
        Position = new Vector2(
            Math.Min(targetX, 0),
            0
        );
    }
}