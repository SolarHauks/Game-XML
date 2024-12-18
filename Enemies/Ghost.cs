using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using JeuVideo.Character;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JeuVideo.Enemies;

// Classe représentant un fantôme
[Serializable]
[XmlRoot("ghost", Namespace = "https://www.univ-grenoble-alpes.fr/jeu/ennemi")]
public class Ghost : Enemy
{
    [XmlElement("distance")] public int Distance;   // Distance au joueur maximale pour que le fantome le suive
    [XmlElement("speed")] public int Speed;        // Vitesse du fantome
    
    [XmlIgnore] private Player _player;           // Joueur
    
    public void Load(Vector2 position, Player player)
    {
        _player = player;
        
        Texture2D texture = Globals.Content.Load<Texture2D>("Assets/Enemies/ghost");
        base.Load(texture, position);
    }

    // Si le joueur est à portée, le fantome le suit
    protected override void DeplacementHorizontal(double dt)
    {
        if (CheckPlayerDistance())
        {
            Vector2 directionToTarget = Vector2.Normalize(_player.Position - Position);
            Velocity.X = (float)(directionToTarget.X * Speed * dt);
            Position.X += Velocity.X;
            Direction = Position.X > _player.Position.X ? -1 : 1;
        }
    }

    // Si le joueur est à portée, le fantome le suit
    protected override void DeplacementVertical(double dt)
    {
        if (CheckPlayerDistance())
        {
            Vector2 directionToTarget = Vector2.Normalize(_player.Position - Position);
            Velocity.Y = (float)(directionToTarget.Y * 50 * dt);
            Position.Y += Velocity.Y;
        }
    }
    
    // Vérifie si le joueur est à portée
    private bool CheckPlayerDistance() => (Vector2.Distance(_player.Position, Position) < Distance);

    // Une seule animation ici
    protected override void Animate(Vector2 velocity) { AnimationManager.SetAnimation("fly"); }
    
    // On reimplemente ces deux méthodes pour éviter les collisions
    protected override void CheckCollisionsHorizontal(Dictionary<Vector2, int> collision) { }
    
    protected override void CheckCollisionsVertical(Dictionary<Vector2, int> collision) { }
    
}
