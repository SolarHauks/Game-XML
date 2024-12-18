using System;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JeuVideo.Enemies;

// Classe représentant un pic
[Serializable]
[XmlRoot("spike", Namespace = "https://www.univ-grenoble-alpes.fr/jeu/ennemi")]
public class Spike : Enemy
{
    public void Load(Vector2 position)
    {
        Texture2D texture = Globals.Content.Load<Texture2D>("Assets/Enemies/spike");
        base.Load(texture, position);
    }

    // Le spike ne se déplace pas
    protected override void DeplacementHorizontal(double dt) { }

    protected override void DeplacementVertical(double dt) { }

    // Une seule animation. Elle est ici car elle est obligatoire
    protected override void Animate(Vector2 velocity)
    {
        AnimationManager.SetAnimation("idle");
    }
}

   

    
