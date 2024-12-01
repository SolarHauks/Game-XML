using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JeuVideo.Menu;

public class ShopMenu
{
    // boutons du menu
    private readonly Button _healthButton;
    private readonly Button _manaButton;
    
    private readonly Texture2D _bgTexture;  // textures du menu
    
    public bool IsActive { get; set; }  // Etat du menu. Active -> affiché et logique qui tourne
    
    private readonly Vector2 _screenSize;   // Taille de l'écran
    
    // Rectangle de destination du sprite du menu
    private Rectangle DestRectangle => new((int)(_screenSize.X / 2 - 64), (int)(_screenSize.Y / 2 - 64), 128, 128);

    public ShopMenu()
    {
        _bgTexture = Globals.Content.Load<Texture2D>("Assets/GUI/shopMenu"); // Fond du menu
        
        IsActive = false;   // Menu désactivé par défaut
        
        _screenSize = Globals.ScreenSize;
        
        Texture2D healthButtonTexture = Globals.Content.Load<Texture2D>("Assets/GUI/healthPot");
        Texture2D manaButtonTexture = Globals.Content.Load<Texture2D>("Assets/GUI/manaPot");
        
        // Position des boutons => position du menu + décalage
        _healthButton = new Button(healthButtonTexture, new Vector2(DestRectangle.X + 16*1, _screenSize.Y / 2 - 8));
        _manaButton = new Button(manaButtonTexture, new Vector2(DestRectangle.X + 16*5, _screenSize.Y / 2 - 8));
    }
    
    public void Update(Player player)
    {
        if (!IsActive) { return; }
        
        _healthButton.Update();
        _manaButton.Update();

        if (_healthButton.IsClicked) { player.Health += 10; }

        if (_manaButton.IsClicked) { player.Mana += 10; }
    }

    public void Draw()
    {
        if (!IsActive) return;
        
        // Dessin du menu
        Globals.SpriteBatch.Draw(
            _bgTexture,
            DestRectangle,
            Color.White);
        
        // Dessin des boutons
        _healthButton.Draw();
        _manaButton.Draw();
    }
}