using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using JeuVideo.Character;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JeuVideo.Enemies;

[Serializable]
[XmlRoot("ghost", Namespace = "https://www.univ-grenoble-alpes.fr/jeu/ennemi")]
public class Ghost : Enemy
{
    [XmlElement("distance")] public int Distance;
    [XmlElement("speed")] public int Speed;
    [XmlElement("hitboxRatio")] public float HitboxRatio;
    
    [XmlIgnore] private Player _player;
    
    public void Load(Vector2 position, Player player)
    {
        _player = player;
        
        Texture2D texture = Globals.Content.Load<Texture2D>("Assets/Enemies/ghost");
        base.Load(texture, position, HitboxRatio);
    }

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

    protected override void DeplacementVertical(double dt)
    {
        if (CheckPlayerDistance())
        {
            Vector2 directionToTarget = Vector2.Normalize(_player.Position - Position);
            Velocity.Y = (float)(directionToTarget.Y * 50 * dt);
            Position.Y += Velocity.Y;
        }
    }
    
    private bool CheckPlayerDistance()
    {
        return (Vector2.Distance(_player.Position, Position) < Distance);
    }

    protected override void Animate(Vector2 velocity)
    {
        AnimationManager.SetAnimation("fly");
    }
    
    // On reimplemente ces deux méthodes pour éviter les collisions
    protected override void CheckCollisionsHorizontal(Dictionary<Vector2, int> collision)
    {
    }
    
    protected override void CheckCollisionsVertical(Dictionary<Vector2, int> collision)
    {
    }
    
}
