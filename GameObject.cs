using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JeuVideo;

public class GameObject : Sprite
{
    protected Vector2 Velocity;
    
    protected GameObject(Texture2D texture, Vector2 position, int size) 
        : base(texture, position, size) {
    }
}