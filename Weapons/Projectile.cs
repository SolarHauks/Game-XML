using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JeuVideo.Weapons;

public class Projectile : GameObject
{
    private readonly int _speed;
    private readonly Vector2 _angle;
    private readonly double _spawnTime;
    
    public bool IsAlive { get; set; }   // Permet de savoir si le projectile est encore en vie

    public Projectile(Texture2D texture, Vector2 position, int speed, Vector2 angle, int direction) : base(texture, position, false)
    {
        Direction = direction;
        this._speed = speed;
        _angle = angle;
        IsAlive = true;
        _spawnTime = Globals.GameTime.TotalGameTime.TotalSeconds;
    }
    
    protected override void DeplacementHorizontal(double dt)
    {
        Velocity.X = _angle.X * _speed * Direction * (float)dt;
        Position.X += Velocity.X;
    }

    protected override void DeplacementVertical(double dt)
    {
        Velocity.Y = _angle.Y * _speed;
        Position.Y += (float) (Velocity.Y * dt);
    }
    
    public new void Update(Dictionary<Vector2, int> collision)
    {
        if (!IsAlive) { return; }
        
        base.Update(collision);
        
        TimeAlive();
    }

    // Si le projectile est en vie depuis plus de 3 secondes, on le tue
    // Evite de s'emmerder avec des limites d'écran, qui nécessiterait de faire circuler l'offset jusqu'à là
    // On pourrait sinon stocker la caméra dans Globals et récupérer l'offset comme ça
    private void TimeAlive()
    {
        double currentTime = Globals.GameTime.TotalGameTime.TotalSeconds;
        if (currentTime - _spawnTime > 3)
        {
            IsAlive = false;
        }
    }
    
    protected override void WhenHorizontalCollisions(Rectangle rect)
    {
        IsAlive = false;
    }
    
    protected override void WhenVerticalCollisions(Rectangle rect)
    {
        IsAlive = false;
    }

    protected override void Animate(Vector2 velocity)
    {
        // No animation for projectiles
    }
}