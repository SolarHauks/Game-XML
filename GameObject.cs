using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BasicMonoGame;

public class GameObject : Sprite
{
    protected Vector2 _speed;
    
    public GameObject(Texture2D texture, Vector2 position, int size) 
        : base(texture, position, size) {
    }
}