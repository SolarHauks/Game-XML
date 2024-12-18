using System;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JeuVideo.Enemies;

// Classe représentant un serpent
[Serializable]
[XmlRoot("snake", Namespace = "https://www.univ-grenoble-alpes.fr/jeu/ennemi")]
public class Snake : Enemy
{
    [XmlElement("speed")] public int Speed; // Vitesse de déplacement
    [XmlElement("distance")] public int Distance;   // Distance max de déplacement
    
    public void Load(Vector2 position)
    {
        Texture2D texture = Globals.Content.Load<Texture2D>("Assets/Enemies/snake");
        base.Load(texture, position);
    }
    
    // Allers-retours horizontaux
    protected override void DeplacementHorizontal(double dt)
    {
        // On passe par la velocité car on en a besoin pour les collisions
        Velocity.X = Direction * Speed * (float)dt;
        Position.X += Velocity.X;
        
        // Si on atteint la distance max, on change de direction
        if (Math.Abs(Position.X - StartPosition.X) > Distance) { Direction *= -1; }
    }

    // Soumis à la gravité
    protected override void DeplacementVertical(double dt)
    {
        Velocity.Y = 25.0f * (float)dt; // Gravité
        Position.Y += Velocity.Y;
    }

    // Une seule animation
    protected override void Animate(Vector2 velocity)
    {
        AnimationManager.SetAnimation("walk");
    }
    
}   