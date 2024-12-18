using JeuVideo.Character;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JeuVideo.UI;

// Menu de la boutique
public class ShopMenu : Menu
{
    // Constantes pour les prix et les ajouts de vie et de mana
    private const int HealthPrice = 10;
    private const int ManaPrice = 15;
    private const int HealthAdd = 10;
    private const int ManaAdd = 10;

    public ShopMenu() : base("shopMenu")
    {
        // Bouton de la potion de vie
        Texture2D healthButtonTexture = Globals.Content.Load<Texture2D>("Assets/GUI/healthPot");
        // Bouton de la potion de mana
        Texture2D manaButtonTexture = Globals.Content.Load<Texture2D>("Assets/GUI/manaPot");
        
        // Position des boutons => position du menu + décalage
        Buttons.Add(new Button(healthButtonTexture, new Vector2(DestRectangle.X + 20 * 1, ScreenSize.Y / 2 - 8)));
        Buttons.Add(new Button(manaButtonTexture, new Vector2(DestRectangle.X + 16 * 5, ScreenSize.Y / 2 - 8)));
    }

    public void Update(Player player)
    {
        base.Update();

        //Pour chaque bouton, au click on vérifit que le joueur a assez d'argent pour acheter l'item
        //Si c'est le cas, on ajoute la vie ou la mana et on retire l'argent
        if (Buttons[0].IsClicked && player.ResourceManager.GoldCounter.Money >= HealthPrice)
        {
            player.ResourceManager.AddMaxHealth(HealthAdd);
            player.ResourceManager.GoldCounter.RemoveMoney(HealthPrice);
        }

        if (Buttons[1].IsClicked && player.ResourceManager.GoldCounter.Money >= ManaPrice)
        {
            player.ResourceManager.AddMaxMana(ManaAdd);
            player.ResourceManager.GoldCounter.RemoveMoney(ManaPrice);
        }
    }
}