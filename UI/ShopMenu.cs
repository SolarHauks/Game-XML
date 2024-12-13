using JeuVideo.Character;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JeuVideo.UI;

public class ShopMenu : Menu
{
    public ShopMenu() : base("shopMenu")
    {
        Texture2D healthButtonTexture = Globals.Content.Load<Texture2D>("Assets/GUI/healthPot");
        Texture2D manaButtonTexture = Globals.Content.Load<Texture2D>("Assets/GUI/manaPot");
        
        // Position des boutons => position du menu + décalage
        Buttons.Add(new Button(healthButtonTexture, new Vector2(DestRectangle.X + 20 * 1, ScreenSize.Y / 2 - 8)));
        Buttons.Add(new Button(manaButtonTexture, new Vector2(DestRectangle.X + 16 * 5, ScreenSize.Y / 2 - 8)));
    }

    public void Update(Player player)
    {
        base.Update();

        if (Buttons[0].IsClicked && player.ResourceManager.GoldCounter.Money >= 10)
        {
            player.ResourceManager.AddMaxHealth(10);
            player.ResourceManager.GoldCounter.RemoveMoney(10);
        }

        if (Buttons[1].IsClicked && player.ResourceManager.GoldCounter.Money >= 15)
        {
            player.ResourceManager.AddMaxMana(10);
            player.ResourceManager.GoldCounter.RemoveMoney(15);
        }
    }
}