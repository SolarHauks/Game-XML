using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace JeuVideo;

public class GameObject : Sprite
{
    protected float _speed;
    
    public GameObject(Texture2D texture, Vector2 position, int size) 
        : base(texture, position, size) {
    }
}