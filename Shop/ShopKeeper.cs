using System.Collections.Generic;
using JeuVideo.Character;
using JeuVideo.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JeuVideo.Shop;

// Classe ShopKeeper pour gérer le marchand
// On la dérive ici de Sprite et pas de GO car on n'a pas besoin de toute la logique lourde de GO.
// On recode juste une simple méthode Update pour gérer l'animation
// Il n'est pas désérialiser car il n'y en a pas besoin
public class ShopKeeper : GameObject
{
    private readonly ShopMenu _shopMenu;
    private readonly Player _player;
    public bool IsPaused;
    
    public ShopKeeper(Vector2 position, Player player) : 
        base(Globals.Content.Load<Texture2D>("Assets/NPC/shop"), position, true, 1.0f)
    {
        AnimationManager.SetAnimation("idle");
        _shopMenu = new ShopMenu();
        _player = player;
        IsPaused = false;
    }
    
    // Juste pour update l'animation
    public override void Update(Dictionary<Vector2, int> collision)
    {
        base.Update(collision);
        
        _shopMenu.Update(_player);
    }
    
    public void Interact()
    {
        Rectangle shopHitbox = new Rectangle(
            (int)Position.X,
            (int)Position.Y,
            100,
            100
        );
        
        if (shopHitbox.Intersects(_player.Rect))
        {
            _shopMenu.IsActive = !_shopMenu.IsActive;
            IsPaused = !IsPaused;
        }
    }
    
    public override void Draw(Vector2 offset)
    {
        base.Draw(offset);
        
        _shopMenu.Draw();
    }

    protected override void DeplacementHorizontal(double dt)
    {
    }

    protected override void DeplacementVertical(double dt)
    {
    }

    protected override void Animate(Vector2 velocity)
    {
    }
}