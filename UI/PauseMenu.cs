using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JeuVideo.UI;

public class PauseMenu
{
    // boutons du menu
    private readonly ClickableArea _playClickableArea;
    // private readonly Button _optionButton;
    private readonly ClickableArea _quitClickableArea;
    
    // textures du menu
    private readonly Texture2D _texture;
    private readonly Texture2D _bgTexture;
    
    // Etat du jeu
    public bool IsPaused { get; set; }
    
    // Taille de l'écran
    private readonly Vector2 _screenSize;
    
    // Rectangle de destination du sprite du menu
    private Rectangle DestRectangle => new((int)(_screenSize.X / 2 - 64), (int)(_screenSize.Y / 2 - 64), 128, 128);

    public PauseMenu(Texture2D texture)
    {
        _texture = texture; // Tileset gui contenant le menu
        
        // Texture du fond gris semi transparent
        _bgTexture = new Texture2D(Globals.GraphicsDevice, 1, 1);
        _bgTexture.SetData(new Color[] { new(0, 0, 0, 128) });

        //_srcRectangle = new Rectangle(0, 192, 64, 64);  // Position du menu dans le tileset gui

        IsPaused = false;
        
        _screenSize = Globals.ScreenSize;
        
        // Position des boutons => position du menu + décalage
        _playClickableArea = new ClickableArea(new Vector2(DestRectangle.X + 17*2, DestRectangle.Y + 9*2));
        _quitClickableArea = new ClickableArea(new Vector2(DestRectangle.X + 17*2, DestRectangle.Y + 41*2));
    }
    
    public void Update(Game1 game)
    {
        if (!IsPaused) { return; }
        
        _playClickableArea.Update();
        _quitClickableArea.Update();

        if (_playClickableArea.IsClicked)
        {
            // Restart the game
            IsPaused = false;
        }

        if (_quitClickableArea.IsClicked)
        {
            // Exit the game
            game.Exit();
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (!IsPaused) return;
        
        // Dessin du fond gris semi transparent
        spriteBatch.Draw(_bgTexture, new Rectangle(0, 0, (int)_screenSize.X, (int)_screenSize.Y), Color.White);
            
        // Dessin du menu
        spriteBatch.Draw(
            _texture,
            DestRectangle,
            Color.White);
    }
}