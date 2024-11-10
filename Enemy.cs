using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JeuVideo;

public class Enemy : GameObject
{
    public Enemy(Texture2D texture, Vector2 position) 
        : base(texture, position) {
    }

    protected override void DeplacementHorizontal(float dt)
    {
        throw new System.NotImplementedException();
    }

    protected override void DeplacementVertical(float dt)
    {
        throw new System.NotImplementedException();
    }
}